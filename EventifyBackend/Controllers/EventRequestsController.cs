using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventifyBackend.Controllers
{
    [Route("api/event-requests")]
    [ApiController]
    public class EventRequestsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventRequestsController(EventifyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateEventRequest([FromBody] EventRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.RequesterName) || string.IsNullOrEmpty(request.RequesterEmail))
            {
                return BadRequest(new { error = "Title, requester name, and email are required." });
            }

            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;
            request.Status = "Pending";

            _context.EventRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventRequest), new { id = request.Id }, request);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventRequests()
        {
            var requests = await _context.EventRequests.ToListAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEventRequest(int id)
        {
            var request = await _context.EventRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { error = "Event request not found" });
            }
            return Ok(request);
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateEventRequestStatus(int id, [FromBody] EventRequestStatusUpdate statusUpdate)
        {
            if (statusUpdate == null || !new[] { "Accepted", "Denied" }.Contains(statusUpdate.Status))
            {
                return BadRequest(new { error = "Invalid status. Must be 'Accepted' or 'Denied'." });
            }

            var request = await _context.EventRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { error = "Event request not found" });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            request.Status = statusUpdate.Status;
            request.UpdatedAt = DateTime.UtcNow;
            request.ProcessedByUserId = int.Parse(userIdClaim.Value);

            if (statusUpdate.Status == "Accepted")
            {
                var newEvent = new Event
                {
                    Title = request.Title,
                    Description = request.Description,
                    Date = request.Date,
                    UserId = int.Parse(userIdClaim.Value),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Archived = false
                };
                _context.Events.Add(newEvent);
            }

            await _context.SaveChangesAsync();
            return Ok(request);
        }
    }

    public class EventRequestStatusUpdate
    {
        public string Status { get; set; } = string.Empty;
    }
}