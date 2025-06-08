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

        // POST: api/event-requests
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateEventRequest([FromBody] EventRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Title) || string.IsNullOrEmpty(request.RequesterName) || string.IsNullOrEmpty(request.RequesterEmail))
            {
                return BadRequest(new { error = "Title, requester name, and email are required." });
            }

            // Override client-provided values
            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;
            request.Status = "Pending"; // Default status
            request.ProcessedByUserId = null; // Ignore client value, set to null

            _context.EventRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventRequest), new { id = request.Id }, request);
        }

        // GET: api/event-requests
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEventRequests()
        {
            var requests = await _context.EventRequests.ToListAsync();
            return Ok(requests);
        }

        // GET: api/event-requests/{id}
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

        // PUT: api/event-requests/{id}/status
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

            request.Status = statusUpdate.Status;
            request.UpdatedAt = DateTime.UtcNow;
            // No processedByUserId update since it's not used

            await _context.SaveChangesAsync();
            return Ok(request);
        }
    }

    public class EventRequestStatusUpdate
    {
        public string Status { get; set; } = string.Empty;
    }
}