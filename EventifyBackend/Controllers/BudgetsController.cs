using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Threading.Tasks;

namespace EventifyBackend.Controllers;

[Route("api/events/{eventId}/budgets")]
[ApiController]
public class BudgetsController : ControllerBase
{
    private readonly EventifyDbContext _context;

    public BudgetsController(EventifyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgets(string eventId)
    {
        if (!int.TryParse(eventId, out int parsedEventId))
            return BadRequest("Invalid event ID.");

        var budgets = await _context.Budgets
            .Where(b => b.EventId == parsedEventId)
            .ToListAsync();
        return Ok(budgets);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBudget(string eventId, [FromBody] Budget budget)
    {
        if (!int.TryParse(eventId, out int parsedEventId))
            return BadRequest("Invalid event ID.");

        budget.EventId = parsedEventId;
        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(CreateBudget), new { id = budget.Id }, budget);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudget(string eventId, string id)
    {
        if (!int.TryParse(eventId, out int parsedEventId) || !int.TryParse(id, out int budgetId))
            return BadRequest("Invalid event ID or budget ID.");

        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.EventId == parsedEventId && b.Id == budgetId);
        if (budget == null)
            return NotFound();
        return Ok(budget);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(string eventId, string id, [FromBody] Budget budget)
    {
        if (!int.TryParse(eventId, out int parsedEventId) || !int.TryParse(id, out int budgetId))
            return BadRequest("Invalid event ID or budget ID.");

        var existingBudget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.EventId == parsedEventId && b.Id == budgetId);
        if (existingBudget == null)
            return NotFound();

        existingBudget.Amount = budget.Amount;
        existingBudget.Category = budget.Category;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(string eventId, string id)
    {
        if (!int.TryParse(eventId, out int parsedEventId) || !int.TryParse(id, out int budgetId))
            return BadRequest("Invalid event ID or budget ID.");

        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.EventId == parsedEventId && b.Id == budgetId);
        if (budget == null)
            return NotFound();

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}