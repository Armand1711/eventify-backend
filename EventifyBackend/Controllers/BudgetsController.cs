using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Threading.Tasks;

namespace EventifyBackend.Controllers;

[Route("api/tasks/{taskId}/budgets")]
[ApiController]
public class BudgetsController : ControllerBase
{
    private readonly EventifyDbContext _context;

    public BudgetsController(EventifyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgets(string taskId)
    {
        if (!int.TryParse(taskId, out int parsedTaskId))
            return BadRequest("Invalid task ID.");

        var budgets = await _context.Budgets
            .Where(b => b.TaskId == parsedTaskId)
            .ToListAsync();
        return Ok(budgets);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBudget(string taskId, [FromBody] Budget budget)
    {
        if (!int.TryParse(taskId, out int parsedTaskId))
            return BadRequest("Invalid task ID.");

        budget.TaskId = parsedTaskId;
        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(CreateBudget), new { id = budget.Id }, budget);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudget(string taskId, string id)
    {
        if (!int.TryParse(taskId, out int parsedTaskId) || !int.TryParse(id, out int budgetId))
            return BadRequest("Invalid task ID or budget ID.");

        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.TaskId == parsedTaskId && b.Id == budgetId);
        if (budget == null)
            return NotFound();
        return Ok(budget);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(string taskId, string id, [FromBody] Budget budget)
    {
        if (!int.TryParse(taskId, out int parsedTaskId) || !int.TryParse(id, out int budgetId))
            return BadRequest("Invalid task ID or budget ID.");

        var existingBudget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.TaskId == parsedTaskId && b.Id == budgetId);
        if (existingBudget == null)
            return NotFound();

        existingBudget.Amount = budget.Amount;
        existingBudget.Category = budget.Category;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(string taskId, string id)
    {
        if (!int.TryParse(taskId, out int parsedTaskId) || !int.TryParse(id, out int budgetId))
            return BadRequest("Invalid task ID or budget ID.");

        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.TaskId == parsedTaskId && b.Id == budgetId);
        if (budget == null)
            return NotFound();

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}