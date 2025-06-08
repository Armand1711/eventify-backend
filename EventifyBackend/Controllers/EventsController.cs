using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Security.Claims;

namespace EventifyBackend.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize] // Require JWT bearer token for all actions here
    public class EventsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventsController(EventifyDbContext context)
        {
            _context = context;
        }

        // GET api/events/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return NotFound();

            // Optionally, check if current user owns the event
            var userId = GetUserIdFromToken();
            if (evt.UserId != userId) return Forbid();

            return Ok(evt);
        }

        // POST api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] Event evt)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            // Override userId with token userId for security
            evt.UserId = userId.Value;
            evt.CreatedAt = DateTime.UtcNow;
            evt.UpdatedAt = DateTime.UtcNow;

            if (evt.Archived)
            {
                // Save as archive
                var archive = new Archive
                {
                    EventId = evt.Id,
                    Title = evt.Title,
                    Description = evt.Description,
                    Date = evt.Date,
                    UserId = evt.UserId,
                    CreatedAt = evt.CreatedAt,
                    UpdatedAt = evt.UpdatedAt
                };
                _context.Archives.Add(archive);
            }
            else
            {
                _context.Events.Add(evt);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, evt);
        }

        // PUT api/events/{id}/archive
        [HttpPut("{id}/archive")]
        public async Task<IActionResult> ArchiveEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return NotFound();
            if (evt.UserId != userId) return Forbid();

            evt.Archived = true;
            evt.UpdatedAt = DateTime.UtcNow;

            var archive = new Archive
            {
                EventId = evt.Id,
                Title = evt.Title,
                Description = evt.Description,
                Date = evt.Date,
                UserId = evt.UserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Archives.Add(archive);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper to extract UserId from JWT token claims
        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return null;
            if (int.TryParse(userIdClaim.Value, out int userId))
                return userId;
            return null;
        }
    }
}
