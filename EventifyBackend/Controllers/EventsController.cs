using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

        // GET: api/events/all -- publicly accessible, returns all events
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        // GET: api/events -- authorized users only, returns user-specific events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetUserEvents([FromQuery] bool includeArchived = false)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Problem(detail: "User not authenticated", statusCode: StatusCodes.Status401Unauthorized);

            var query = _context.Events.Where(e => e.UserId == userId);

            if (!includeArchived)
                query = query.Where(e => !e.Archived);

            var events = await query.ToListAsync();
            return Ok(events);
        }

        // GET: api/events/{id} -- any authenticated user can view event (without full Tasks)
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Problem(detail: "User not authenticated", statusCode: StatusCodes.Status401Unauthorized);

            var evt = await _context.Events
                .Select(e => new Event
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    UserId = e.UserId,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    Archived = e.Archived
                    // Exclude Tasks to avoid circular reference
                })
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evt == null)
                return Problem(detail: "Event not found", statusCode: StatusCodes.Status404NotFound);

            return Ok(evt);
        }

        // POST: api/events -- authorized users only
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] EventRequest eventRequest)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Problem(detail: "User not authenticated", statusCode: StatusCodes.Status401Unauthorized);

            if (eventRequest == null || string.IsNullOrEmpty(eventRequest.Title) || eventRequest.Date == default)
                return Problem(detail: "Title and date are required", statusCode: StatusCodes.Status400BadRequest);

            var evt = new Event
            {
                Title = eventRequest.Title,
                Description = eventRequest.Description,
                Date = eventRequest.Date,
                UserId = userId.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Archived = false // Default value
            };

            _context.Events.Add(evt);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, evt);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Database update error", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        // ... (other methods like AddTaskToEvent remain unchanged)

        // Helper to extract UserId from JWT token claims
        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }

    // Request model for event creation
    public class EventRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}