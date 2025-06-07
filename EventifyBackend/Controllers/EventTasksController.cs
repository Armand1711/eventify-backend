using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EventifyBackend.Controllers
{
    [Route("api/events/{eventId}/tasks")]
    [ApiController]
    public class EventTasksController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventTasksController(EventifyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(string eventId)
        {
            if (!int.TryParse(eventId, out int parsedEventId))
                return BadRequest("Invalid event ID.");

            var tasks = await _context.EventTasks
                .Where(t => t.EventId == parsedEventId)
                .ToListAsync();
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(string eventId, [FromBody] EventTask task)
        {
            if (!int.TryParse(eventId, out int parsedEventId))
                return BadRequest("Invalid event ID.");

            if (task == null)
                return BadRequest("Task data is required.");

            task.EventId = parsedEventId;
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            _context.EventTasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { eventId = eventId, id = task.Id }, task);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(string eventId, string id)
        {
            if (!int.TryParse(eventId, out int parsedEventId) || !int.TryParse(id, out int taskId))
                return BadRequest("Invalid event ID or task ID.");

            var task = await _context.EventTasks
                .FirstOrDefaultAsync(t => t.EventId == parsedEventId && t.Id == taskId);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string eventId, string id, [FromBody] EventTask task)
        {
            if (!int.TryParse(eventId, out int parsedEventId) || !int.TryParse(id, out int taskId))
                return BadRequest("Invalid event ID or task ID.");

            if (task == null)
                return BadRequest("Task data is required.");

            var existingTask = await _context.EventTasks
                .FirstOrDefaultAsync(t => t.EventId == parsedEventId && t.Id == taskId);
            if (existingTask == null)
                return NotFound();

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.AssignedTo = task.AssignedTo;
            existingTask.Budget = task.Budget;
            existingTask.Completed = task.Completed;
            existingTask.Archived = task.Archived;
            existingTask.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string eventId, string id)
        {
            if (!int.TryParse(eventId, out int parsedEventId) || !int.TryParse(id, out int taskId))
                return BadRequest("Invalid event ID or task ID.");

            var task = await _context.EventTasks
                .FirstOrDefaultAsync(t => t.EventId == parsedEventId && t.Id == taskId);
            if (task == null)
                return NotFound();

            _context.EventTasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}/archive")]
        public async Task<IActionResult> ArchiveTask(string eventId, string id)
        {
            if (!int.TryParse(eventId, out int parsedEventId) || !int.TryParse(id, out int taskId))
                return BadRequest("Invalid event ID or task ID.");

            var task = await _context.EventTasks
                .FirstOrDefaultAsync(t => t.EventId == parsedEventId && t.Id == taskId);
            if (task == null)
                return NotFound();

            task.Archived = true;
            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get total budget for an event (sum of all non-archived tasks)
        [HttpGet("~/api/events/{eventId}/tasks/budget")]
        public async Task<ActionResult<decimal>> GetEventBudget(string eventId)
        {
            if (!int.TryParse(eventId, out int parsedEventId))
                return BadRequest("Invalid event ID.");

            var tasks = await _context.EventTasks
                .Where(t => t.EventId == parsedEventId && !t.Archived)
                .ToListAsync();

            // Assuming budget is stored as a string like "1200" or "R1200"
            decimal totalBudget = tasks
                .Select(t => decimal.TryParse(
                    t.Budget.Replace("R", "").Replace(" ", "").Replace(",", "").Trim(), out var b) ? b : 0)
                .Sum();

            return Ok(totalBudget);
        }

        [HttpGet("~/api/events/{eventId}/tasks/completion")]
        public async Task<IActionResult> GetTaskCompletion(string eventId)
        {
            if (!int.TryParse(eventId, out int parsedEventId))
                return BadRequest("Invalid event ID.");

            var tasks = await _context.EventTasks
                .Where(t => t.EventId == parsedEventId && !t.Archived)
                .ToListAsync();

            int total = tasks.Count;
            int completed = tasks.Count(t => t.Completed);
            int notCompleted = total - completed;

            return Ok(new
            {
                totalTasks = total,
                completedTasks = completed,
                notCompletedTasks = notCompleted
            });
        }

        private async Task CheckAndAutoArchiveEvent(int eventId)
        {
            var tasks = await _context.EventTasks
                .Where(t => t.EventId == eventId && !t.Archived)
                .ToListAsync();

            if (tasks.Count > 0 && tasks.All(t => t.Completed))
            {
                var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
                if (evt != null && !evt.Archived)
                {
                    evt.Archived = true;
                    evt.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}