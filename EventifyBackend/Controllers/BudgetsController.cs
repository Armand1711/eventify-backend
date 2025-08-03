using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Threading.Tasks;
using System.Security.Claims;

namespace EventifyBackend.Controllers
{
    [Route("api/tasks/{taskId:int}/budgets")]
    [ApiController]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public BudgetsController(EventifyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgets(int taskId)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var budgets = await _context.Budgets
                .Where(b => b.TaskId == taskId && b.UserId == userId)
                .ToListAsync();
            return Ok(budgets);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget(int taskId, [FromBody] Budget budget)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var task = await _context.EventTasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                return NotFound("Task not found or unauthorized");

            budget.TaskId = taskId;
            budget.UserId = userId.Value;
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBudget), new { taskId, id = budget.Id }, budget);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBudget(int taskId, int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.TaskId == taskId && b.Id == id && b.UserId == userId);
            if (budget == null)
                return NotFound();
            return Ok(budget);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBudget(int taskId, int id, [FromBody] Budget budget)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var existingBudget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.TaskId == taskId && b.Id == id && b.UserId == userId);
            if (existingBudget == null)
                return NotFound();

            existingBudget.Amount = budget.Amount;
            existingBudget.Category = budget.Category;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBudget(int taskId, int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.TaskId == taskId && b.Id == id && b.UserId == userId);
            if (budget == null)
                return NotFound();

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Returns total budget and used budget for an event
        [HttpGet("~/api/events/{eventId}/budget-summary")]
        public async Task<IActionResult> GetEventBudgetSummary(int eventId)
        {
            // Get all tasks for the event
            var tasks = await _context.EventTasks
                .Where(t => t.EventId == eventId && !t.Archived)
                .ToListAsync();

            // Sum all budgets 
            decimal totalBudget = tasks
                .Where(t => !string.IsNullOrEmpty(t.Budget))
                .Sum(t => decimal.TryParse(t.Budget.Replace("R", "").Replace(",", "").Trim(), out var b) ? b : 0);

            // Sum budgets for completed tasks
            decimal usedBudget = tasks
                .Where(t => t.Completed && !string.IsNullOrEmpty(t.Budget))
                .Sum(t => decimal.TryParse(t.Budget.Replace("R", "").Replace(",", "").Trim(), out var b) ? b : 0);

            return Ok(new
            {
                totalBudget,
                usedBudget,
                tasks = tasks.Select(t => new {
                    t.Id,
                    t.Title,
                    t.Completed,
                    Budget = t.Budget
                })
            });
        }

        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }
}