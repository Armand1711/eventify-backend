using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Security.Claims;

namespace EventifyBackend.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventsController(EventifyDbContext context)
        {
            _context = context;
        }

        // GET: api/events -- get all events for authenticated user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetUserEvents()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var events = await _context.Events
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/{id} -- get event by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var evt = await _context.Events.FindAsync(id);
            if (evt == null || evt.UserId != userId)
                return NotFound("Event not found");

            return Ok(evt);
        }

        // POST: api/events -- create event
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] Event eventModel)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            if (string.IsNullOrEmpty(eventModel.Title) || eventModel.Date == default)
                return BadRequest("Title and Date are required");

            eventModel.UserId = userId.Value;
            eventModel.CreatedAt = DateTime.UtcNow;
            eventModel.UpdatedAt = DateTime.UtcNow;
            eventModel.Archived = false;

            _context.Events.Add(eventModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = eventModel.Id }, eventModel);
        }

        // DELETE: api/events/{id} -- delete event by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var evt = await _context.Events.FindAsync(id);
            if (evt == null || evt.UserId != userId)
                return NotFound("Event not found");

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper to get user ID from JWT token
        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }
}
