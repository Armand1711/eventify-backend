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
            await _context.SaveChangesAsync();;
            return NoContent();
        }
    }
}