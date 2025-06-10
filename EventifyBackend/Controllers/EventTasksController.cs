using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Claims;

namespace EventifyBackend.Controllers
{
    [Route("api/eventtasks")]
    [ApiController]
    [Authorize]
    public class EventTasksController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventTasksController(EventifyDbContext context)
        {
            _context = context;
        }

        public class EventTaskDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Priority { get; set; } = "Low";
            public bool Completed { get; set; }
            public string? Description { get; set; }
            public DateTime? DueDate { get; set; }
            public int EventId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public bool Archived { get; set; }
            public string? AssignedToEmail { get; set; }
        }

        public class CreateOrUpdateTaskDto
        {
            public string Title { get; set; } = string.Empty;
            public string Priority { get; set; } = "Low";
            public bool Completed { get; set; } = false;
            public string? Description { get; set; }
            public DateTime? DueDate { get; set; }
            public int EventId { get; set; }
            public string AssignedToEmail { get; set; } = string.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventTaskDto>>> GetEventTasks()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var tasks = await _context.EventTasks
                .Include(t => t.AssignedUser)
                .Where(t => t.UserId == userId || _context.Events.Any(e => e.Id == t.EventId && e.UserId == userId))
                .ToListAsync();

            var response = tasks.Select(t => MapToDto(t));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventTaskDto>> GetEventTask(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var task = await _context.EventTasks
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id && (t.UserId == userId || _context.Events.Any(e => e.Id == t.EventId && e.UserId == userId)));

            if (task == null)
                return NotFound();

            return Ok(MapToDto(task));
        }

        [HttpPost]
        public async Task<ActionResult<EventTaskDto>> CreateEventTask([FromBody] CreateOrUpdateTaskDto request)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.AssignedToEmail);
            if (user == null)
                return BadRequest(new { message = "Assigned user email not found." });

            var eventExists = await _context.Events.AnyAsync(e => e.Id == request.EventId && e.UserId == userId);
            if (!eventExists)
                return NotFound("Event not found or unauthorized");

            var newTask = new EventTasks
            {
                Title = request.Title,
                Priority = request.Priority,
                Completed = request.Completed,
                Description = request.Description,
                DueDate = request.DueDate,
                EventId = request.EventId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = user.Id,
                Archived = false
            };

            _context.EventTasks.Add(newTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventTask), new { id = newTask.Id }, MapToDto(newTask));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventTask(int id, [FromBody] CreateOrUpdateTaskDto request)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _context.EventTasks.FindAsync(id);
            if (task == null || !await _context.Events.AnyAsync(e => e.Id == task.EventId && e.UserId == userId))
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.AssignedToEmail);
            if (user == null)
                return BadRequest(new { message = "Assigned user email not found." });

            task.Title = request.Title;
            task.Priority = request.Priority;
            task.Completed = request.Completed;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.EventId = request.EventId;
            task.UserId = user.Id;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventTask(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var task = await _context.EventTasks.FindAsync(id);
            if (task == null || !await _context.Events.AnyAsync(e => e.Id == task.EventId && e.UserId == userId))
                return NotFound();

            _context.EventTasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static EventTaskDto MapToDto(EventTasks t)
        {
            return new EventTaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Priority = t.Priority,
                Completed = t.Completed,
                Description = t.Description,
                DueDate = t.DueDate,
                EventId = t.EventId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                Archived = t.Archived,
                AssignedToEmail = t.AssignedUser?.Email
            };
        }

        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }
}