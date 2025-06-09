using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;

namespace EventifyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventRequestsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventRequestsController(EventifyDbContext context)
        {
            _context = context;
        }

        // POST: api/eventrequests
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostEventRequest([FromBody] EventRequest eventRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Set server-side values
            eventRequest.Id = 0; // Ensure it's treated as new
            eventRequest.CreatedAt = DateTime.UtcNow;
            eventRequest.UpdatedAt = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(eventRequest.Status))
                eventRequest.Status = "Pending";

            try
            {
                _context.EventRequests.Add(eventRequest);
                await _context.SaveChangesAsync();
                return Ok(eventRequest); // return the created object
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Database update error: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/eventrequests
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EventRequest>>> GetEventRequests()
        {
            return await _context.EventRequests.ToListAsync();
        }

        // GET: api/eventrequests/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EventRequest>> GetEventRequest(int id)
        {
            var eventRequest = await _context.EventRequests.FindAsync(id);
            if (eventRequest == null)
                return NotFound();

            return eventRequest;
        }

        // PUT: api/eventrequests/{id}/accept
        [HttpPut("{id}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptEventRequest(int id)
        {
            var request = await _context.EventRequests.FindAsync(id);
            if (request == null) return NotFound();

            request.Status = "Accepted";
            request.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(request);
        }

        // DELETE: api/eventrequests/{id}/deny
        [HttpDelete("{id}/deny")]
        [Authorize]
        public async Task<IActionResult> DenyEventRequest(int id)
        {
            var request = await _context.EventRequests.FindAsync(id);
            if (request == null) return NotFound();

            _context.EventRequests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
