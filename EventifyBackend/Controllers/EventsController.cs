using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Security.Claims;

namespace EventifyBackend.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize] // Require JWT bearer token for all actions by default
    public class EventsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventsController(EventifyDbContext context)
        {
            _context = context;
        }

        // GET api/events/all -- publicly accessible, returns all events
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET api/events?includeArchived=false -- authorized users only, returns user-specific events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetUserEvents([FromQuery] bool includeArchived = false)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var query = _context.Events.Where(e => e.UserId == userId);

            if (!includeArchived)
                query = query.Where(e => !e.Archived);

            var events = await query.ToListAsync();

            return Ok(events);
        }

        // GET api/events/{id} -- any authenticated user can view event and tasks
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var evt = await _context.Events
                .Include(e => e.Tasks)  // Load related tasks
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evt == null) return NotFound();

            // Remove owner check: any signed-in user can view event

            return Ok(evt);
        }

        // GET api/events/{id}/tasks -- public, returns tasks for an event
        [HttpGet("{id}/tasks")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EventTask>>> GetTasksForEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return NotFound();

            var tasks = await _context.EventTasks
                .Where(t => t.EventId == id && !t.Archived)
                .ToListAsync();

            return Ok(tasks);
        }

        // POST api/events/{id}/tasks -- add new task to event, authorized users only
        [HttpPost("{id}/tasks")]
        public async Task<ActionResult<EventTask>> AddTaskToEvent(int id, [FromBody] EventTask task)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            var evt = await _context.Events.FindAsync(id);
            if (evt == null) return NotFound();

            task.EventId = id;
            task.UserId = userId.Value;
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _context.EventTasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasksForEvent), new { id = id }, task);
        }

        // POST api/events -- authorized users only
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] Event evt)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized();

            evt.UserId = userId.Value;
            evt.CreatedAt = DateTime.UtcNow;
            evt.UpdatedAt = DateTime.UtcNow;

            if (evt.Archived)
            {
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

        // PUT api/events/{id}/archive -- authorized users only
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
