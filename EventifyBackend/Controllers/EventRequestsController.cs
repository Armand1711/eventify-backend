using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;

namespace EventifyBackend.Controllers
{
    [Route("api/eventrequests")]
    [ApiController]
    [Authorize]
    public class EventRequestsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventRequestsController(EventifyDbContext context)
        {
            _context = context;
        }

        // GET: api/eventrequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventRequest>>> GetEventRequests()
        {
            return await _context.EventRequests.ToListAsync();
        }

        // POST: api/eventrequests
        [HttpPost]
        public async Task<ActionResult<EventRequest>> CreateEventRequest([FromBody] EventRequest request)
        {
            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            _context.EventRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventRequests), new { id = request.Id }, request);
        }

        // PUT: api/eventrequests/{id}/accept
        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptRequest(int id)
        {
            var request = await _context.EventRequests.FindAsync(id);
            if (request == null) return NotFound();

            // Move to Events table
            var newEvent = new Event
            {
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Archived = false
            };

            _context.Events.Add(newEvent);
            _context.EventRequests.Remove(request);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/eventrequests/{id}/deny
        [HttpDelete("{id}/deny")]
        public async Task<IActionResult> DenyRequest(int id)
        {
            var request = await _context.EventRequests.FindAsync(id);
            if (request == null) return NotFound();

            _context.EventRequests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
