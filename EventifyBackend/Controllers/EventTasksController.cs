using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace EventifyBackend.Controllers
{
    [Route("api/eventtasks")]
    [ApiController]
    [Authorize]
    public class EventTasksController : ControllerBase
    {
        private readonly EventifyDbContext _context;
        private readonly ILogger<EventTasksController> _logger;

        public EventTasksController(EventifyDbContext context, ILogger<EventTasksController> logger)
        {
            _context = context;
            _logger = logger;
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
            public string? Budget { get; set; } // Using string as per the latest update
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
            public string? Budget { get; set; } // Using string as per the latest update
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventTaskDto>>> GetEventTasks()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var tasks = await _context.EventTasks
                .Include(t => t.AssignedUser)
                .Where(t => t.UserId == userId || _context.Events.Any(e => e.Id == t.EventId && e.UserId == userId))
                .ToListAsync();

            var response = tasks.Select(t => MapToDto(t));
            return Ok(response);
        }

        [HttpGet("byevent/{eventId}")]
        public async Task<ActionResult<IEnumerable<EventTaskDto>>> GetEventTasksByEvent(int eventId)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var tasks = await _context.EventTasks
                .Include(t => t.AssignedUser)
                .Where(t => t.EventId == eventId) // Removed the UserId-based authorization
                .ToListAsync();

            if (!tasks.Any())
                return NotFound(new { error = "No tasks found for the specified event" });

            var response = tasks.Select(t => MapToDto(t));
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventTaskDto>> GetEventTask(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var task = await _context.EventTasks
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id && (t.UserId == userId || _context.Events.Any(e => e.Id == t.EventId && e.UserId == userId)));

            if (task == null)
                return NotFound(new { error = "Task not found or unauthorized" });

            return Ok(MapToDto(task));
        }

        [HttpPost]
        public async Task<ActionResult<EventTaskDto>> CreateEventTask([FromBody] CreateOrUpdateTaskDto request)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid model state", details = ModelState });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.AssignedToEmail);
            if (user == null)
                return BadRequest(new { error = "Assigned user email not found" });

            var eventExists = await _context.Events.AnyAsync(e => e.Id == request.EventId); // Already updated to allow any event
            if (!eventExists)
                return NotFound(new { error = "Event not found or unauthorized" });

            var newTask = new EventTasks
            {
                Title = request.Title,
                Priority = request.Priority,
                Completed = request.Completed,
                Description = request.Description,
                DueDate = request.DueDate,
                EventId = request.EventId,
                Budget = request.Budget,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = user.Id,
                Archived = false
            };

            _context.EventTasks.Add(newTask);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEventTask), new { id = newTask.Id }, MapToDto(newTask));
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while saving EventTask: {@EventTask}", newTask);
                return StatusCode(500, new { error = "Database update error", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error while saving EventTask: {@EventTask}", newTask);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventTask(int id, [FromBody] CreateOrUpdateTaskDto request)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid model state", details = ModelState });

            var task = await _context.EventTasks.FindAsync(id);
            if (task == null || !await _context.Events.AnyAsync(e => e.Id == task.EventId && e.UserId == userId))
                return NotFound(new { error = "Task not found or unauthorized" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.AssignedToEmail);
            if (user == null)
                return BadRequest(new { error = "Assigned user email not found" });

            task.Title = request.Title;
            task.Priority = request.Priority;
            task.Completed = request.Completed;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.EventId = request.EventId;
            task.Budget = request.Budget;
            task.UserId = user.Id;
            task.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while updating EventTask {Id}: {@EventTask}", id, task);
                return StatusCode(500, new { error = "Database update error", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error while updating EventTask {Id}: {@EventTask}", id, task);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventTask(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var task = await _context.EventTasks.FindAsync(id);
            if (task == null || !await _context.Events.AnyAsync(e => e.Id == task.EventId && e.UserId == userId))
                return NotFound(new { error = "Task not found or unauthorized" });

            _context.EventTasks.Remove(task);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting EventTask {Id}", id);
                return StatusCode(500, new { error = "Database update error", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error while deleting EventTask {Id}", id);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
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
                Budget = t.Budget,
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