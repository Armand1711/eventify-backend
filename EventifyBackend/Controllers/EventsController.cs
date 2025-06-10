using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventifyBackend.Controllers
{
    [Route("api/events")]
    [ApiController]
    [Authorize] // Require JWT bearer token for all actions by default
    public class EventsController : ControllerBase
    {
        private readonly EventifyDbContext _context;
        private readonly ILogger<EventsController> _logger;

        public EventsController(EventifyDbContext context, ILogger<EventsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/events/all -- publicly accessible, returns all events
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EventResponse>>> GetAllEvents()
        {
            var events = await _context.Events
                .Select(e => new EventResponse
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    Archived = e.Archived
                }).ToListAsync();

            return Ok(events);
        }

        // GET: api/events -- authorized users only, returns user-specific events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponse>>> GetUserEvents([FromQuery] bool includeArchived = false)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Problem(detail: "User not authenticated", statusCode: StatusCodes.Status401Unauthorized);

            var query = _context.Events.Where(e => e.UserId == userId);

            if (!includeArchived)
                query = query.Where(e => !e.Archived);

            var events = await query.Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt,
                Archived = e.Archived
            }).ToListAsync();

            return Ok(events);
        }

        // GET: api/events/{id} -- any authenticated user can view event (no circular refs)
        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponse>> GetEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                _logger.LogWarning("User not authenticated for GetEvent {Id}", id);
                return Problem(detail: "User not authenticated", statusCode: StatusCodes.Status401Unauthorized);
            }

            var evt = await _context.Events
                .Where(e => e.Id == id)
                .Select(e => new EventResponse
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    Archived = e.Archived
                }).FirstOrDefaultAsync();

            if (evt == null)
            {
                _logger.LogWarning("Event not found for Id {Id}", id);
                return NotFound(new { error = "Event not found" });
            }

            return Ok(evt);
        }

        // POST: api/events -- authorized users only
        [HttpPost]
        public async Task<ActionResult<EventResponse>> CreateEvent([FromBody] EventRequest eventRequest)
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
                Archived = false
            };

            _context.Events.Add(evt);
            try
            {
                await _context.SaveChangesAsync();

                var response = new EventResponse
                {
                    Id = evt.Id,
                    Title = evt.Title,
                    Description = evt.Description,
                    Date = evt.Date,
                    CreatedAt = evt.CreatedAt,
                    UpdatedAt = evt.UpdatedAt,
                    Archived = evt.Archived
                };

                return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, response);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Database update error", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

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

    // Response model to avoid circular reference and Swagger errors
    public class EventResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Archived { get; set; }
    }
}
