using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EventifyBackend.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventsController(EventifyDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
                return NotFound();
            return evt;
        }

        // POST: api/events
        [HttpPost]
        [Authorize] // Make sure only logged-in users can create events
        public async Task<ActionResult<Event>> CreateEvent([FromBody] Event evt)
        {
            // Get userId from JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            evt.UserId = int.Parse(userIdClaim.Value); // Set the userId automatically
            evt.CreatedAt = DateTime.UtcNow;
            evt.UpdatedAt = DateTime.UtcNow;

            _context.Events.Add(evt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, evt);
        }

        // PUT: api/events/{id}/archive
        [HttpPut("{id}/archive")]
        public async Task<IActionResult> ArchiveEvent(int id)
        {
            var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            if (evt == null)
                return NotFound(new { error = "Event not found" });

            evt.Archived = true;
            evt.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}