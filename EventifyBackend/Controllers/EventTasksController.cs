using Microsoft.AspNetCore.Authorization;
   using Microsoft.AspNetCore.Mvc;
   using Microsoft.EntityFrameworkCore;
   using System.Security.Claims;
   using EventifyBackend.Models;

   namespace EventifyBackend.Controllers
   {
       [Route("api/events/{eventId}/tasks")]
       [ApiController]
       [Authorize]
       public class EventTasksController : ControllerBase
       {
           private readonly EventifyDbContext _context;

           public EventTasksController(EventifyDbContext context)
           {
               _context = context;
           }

           [HttpPost]
           public async Task<IActionResult> CreateTask(int eventId, [FromBody] EventTask task)
           {
               var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
               task.EventId = eventId;
               task.UserId = userId;
               _context.Tasks.Add(task);
               await _context.SaveChangesAsync();
               return StatusCode(201, task);
           }

           [HttpGet]
           public async Task<IActionResult> GetTasks(int eventId)
           {
               var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
               var tasks = await _context.Tasks.Where(t => t.EventId == eventId && t.UserId == userId).ToListAsync();
               return Ok(tasks);
           }

           [HttpGet("{taskId}")]
           public async Task<IActionResult> GetTask(int eventId, int taskId)
           {
               var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
               var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.EventId == eventId && t.UserId == userId);
               if (task == null) return NotFound(new { error = "Task not found" });
               return Ok(task);
           }

           [HttpPut("{taskId}")]
           public async Task<IActionResult> UpdateTask(int eventId, int taskId, [FromBody] EventTask updatedTask)
           {
               var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
               var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.EventId == eventId && t.UserId == userId);
               if (task == null) return NotFound(new { error = "Task not found" });

               task.Title = updatedTask.Title;
               task.Description = updatedTask.Description;
               task.DueDate = updatedTask.DueDate;
               await _context.SaveChangesAsync();
               return Ok(task);
           }

           [HttpDelete("{taskId}")]
           public async Task<IActionResult> DeleteTask(int eventId, int taskId)
           {
               var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
               var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.EventId == eventId && t.UserId == userId);
               if (task == null) return NotFound(new { error = "Task not found" });

               _context.Tasks.Remove(task);
               await _context.SaveChangesAsync();
               return NoContent();
           }
       }
   }