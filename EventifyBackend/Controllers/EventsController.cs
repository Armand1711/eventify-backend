using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public class EventDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public DateTime? Date { get; set; }
            public int UserId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public bool Archived { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var events = await _context.Events
                .Where(e => e.UserId == userId && !e.Archived)
                .Include(e => e.Tasks)
                .ToListAsync();

            var eventDtos = events.Select(e =>
            {
                decimal totalBudget = e.Tasks
                    .Where(t => !t.Archived && !string.IsNullOrEmpty(t.Budget))
                    .Sum(t => decimal.TryParse(t.Budget.Replace("R", "").Replace(",", "").Trim(), out var b) ? b : 0);

                decimal usedBudget = e.Tasks
                    .Where(t => t.Completed && !t.Archived && !string.IsNullOrEmpty(t.Budget))
                    .Sum(t => decimal.TryParse(t.Budget.Replace("R", "").Replace(",", "").Trim(), out var b) ? b : 0);

                int progress = totalBudget > 0 ? (int)Math.Round((usedBudget / totalBudget) * 100) : 0;

                return new {
                    id = e.Id,
                    title = e.Title,
                    date = e.Date,
                    status = e.Tasks.All(t => t.Completed) ? "Completed" : "In Progress",
                    progress
                };
            });

            return Ok(eventDtos);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var events = await _context.Events
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    UserId = e.UserId,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    Archived = e.Archived
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var eventItem = await _context.Events
                .Where(e => e.Id == id) // Removed e.UserId == userId to allow any authenticated user
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    UserId = e.UserId,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt,
                    Archived = e.Archived
                })
                .FirstOrDefaultAsync();

            if (eventItem == null)
                return NotFound();

            return Ok(eventItem);
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] EventDto request)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newEvent = new Event
            {
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                UserId = userId.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Archived = false
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            request.Id = newEvent.Id;
            request.CreatedAt = newEvent.CreatedAt;
            request.UpdatedAt = newEvent.UpdatedAt;
            request.Archived = newEvent.Archived;
            return CreatedAtAction(nameof(GetEvent), new { id = newEvent.Id }, request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDto request)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId); // Kept for creator-only updates

            if (eventItem == null)
                return NotFound();

            eventItem.Title = request.Title;
            eventItem.Description = request.Description;
            eventItem.Date = request.Date;
            eventItem.UpdatedAt = DateTime.UtcNow;
            eventItem.Archived = request.Archived;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId); // Kept for creator-only deletes

            if (eventItem == null)
                return NotFound();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }
}