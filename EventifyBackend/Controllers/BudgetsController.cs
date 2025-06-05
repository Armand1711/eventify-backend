using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EventifyBackend.Models;

namespace EventifyBackend.Controllers
{
    [Route("api/events/{eventId}/budgets")]
    [ApiController]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public BudgetsController(EventifyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget(int eventId, [FromBody] Budget budget)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            budget.EventId = eventId;
            budget.UserId = userId;
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return StatusCode(201, budget);
        }

        [HttpGet]
        public async Task<IActionResult> GetBudgets(int eventId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var budgets = await _context.Budgets.Where(b => b.EventId == eventId && b.UserId == userId).ToListAsync();
            return Ok(budgets);
        }

        [HttpGet("{budgetId}")]
        public async Task<IActionResult> GetBudget(int eventId, int budgetId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == budgetId && b.EventId == eventId && b.UserId == userId);
            if (budget == null) return NotFound(new { error = "Budget not found" });
            return Ok(budget);
        }

        [HttpPut("{budgetId}")]
        public async Task<IActionResult> UpdateBudget(int eventId, int budgetId, [FromBody] Budget updatedBudget)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == budgetId && b.EventId == eventId && b.UserId == userId);
            if (budget == null) return NotFound(new { error = "Budget not found" });

            budget.Category = updatedBudget.Category;
            budget.Amount = updatedBudget.Amount;
            await _context.SaveChangesAsync();
            return Ok(budget);
        }

        [HttpDelete("{budgetId}")]
        public async Task<IActionResult> DeleteBudget(int eventId, int budgetId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == budgetId && b.EventId == eventId && b.UserId == userId);
            if (budget == null) return NotFound(new { error = "Budget not found" });

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}