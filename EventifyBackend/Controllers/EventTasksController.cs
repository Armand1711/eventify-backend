using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;

namespace EventifyBackend.Controllers
{
    [Route("api/eventtasks")]
    [ApiController]
    public class EventTasksController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventTasksController(EventifyDbContext context)
        {
            _context = context;
        }

        // GET: api/eventtasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEventTasks()
        {
            var tasks = await _context.EventTasks
                .Include(t => t.AssignedUser)
                .ToListAsync();

            var response = tasks.Select(t => new
            {
                t.Id,
                t.Title,
                t.Priority,
                t.Budget,
                t.Completed,
                t.Description,
                t.DueDate,
                t.EventId,
                t.CreatedAt,
                t.UpdatedAt,
                t.Archived,
                AssignedToEmail = t.AssignedUser?.Email
            });

            return Ok(response);
        }

        // GET: api/eventtasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEventTask(int id)
        {
            var task = await _context.EventTasks
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return NotFound();

            return Ok(new
            {
                task.Id,
                task.Title,
                task.Priority,
                task.Budget,
                task.Completed,
                task.Description,
                task.DueDate,
                task.EventId,
                task.CreatedAt,
                task.UpdatedAt,
                task.Archived,
                AssignedToEmail = task.AssignedUser?.Email
            });
        }

        // POST: api/eventtasks
        [HttpPost]
        public async Task<ActionResult<EventTask>> CreateEventTask([FromBody] CreateOrUpdateTaskDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.AssignedToEmail);
            if (user == null)
                return BadRequest(new { message = "Assigned user email not found." });

            var newTask = new EventTask
            {
                Title = request.Title,
                Priority = request.Priority,
                Budget = request.Budget,
                Completed = request.Completed,
                Description = request.Description,
                DueDate = request.DueDate,
                EventId = request.EventId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = user.Id
            };

            _context.EventTasks.Add(newTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventTask), new { id = newTask.Id }, new
            {
                newTask.Id,
                newTask.Title,
                newTask.Priority,
                newTask.Budget,
                newTask.Completed,
                newTask.Description,
                newTask.DueDate,
                newTask.EventId,
                newTask.CreatedAt,
                newTask.UpdatedAt,
                newTask.Archived,
                AssignedToEmail = user.Email
            });
        }

        // PUT: api/eventtasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventTask(int id, [FromBody] CreateOrUpdateTaskDto request)
        {
            var task = await _context.EventTasks.FindAsync(id);
            if (task == null)
                return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.AssignedToEmail);
            if (user == null)
                return BadRequest(new { message = "Assigned user email not found." });

            task.Title = request.Title;
            task.Priority = request.Priority;
            task.Budget = request.Budget;
            task.Completed = request.Completed;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.EventId = request.EventId;
            task.UpdatedAt = DateTime.UtcNow;
            task.UserId = user.Id;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/eventtasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventTask(int id)
        {
            var task = await _context.EventTasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.EventTasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class CreateOrUpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string Priority { get; set; } = "Low";
        public string Budget { get; set; } = "";
        public bool Completed { get; set; } = false;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int EventId { get; set; }
        public string AssignedToEmail { get; set; } = "";
    }
}
