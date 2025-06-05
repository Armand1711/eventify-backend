using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using System.Security.Claims;
  using EventifyBackend.Models;

  namespace EventifyBackend.Controllers
  {
      [Route("api/events")]
      [ApiController]
      [Authorize]
      public class EventsController : ControllerBase
      {
          private readonly EventifyDbContext _context;

          public EventsController(EventifyDbContext context)
          {
              _context = context;
          }

          [HttpPost]
          public async Task<IActionResult> CreateEvent([FromBody] Event evt)
          {
              var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { error = "User ID not found in token" });
              evt.UserId = int.Parse(userIdClaim);
              _context.Events.Add(evt);
              await _context.SaveChangesAsync();
              return StatusCode(201, evt);
          }

          [HttpGet]
          public async Task<IActionResult> GetEvents()
          {
              var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { error = "User ID not found in token" });
              var userId = int.Parse(userIdClaim);
              var events = await _context.Events.Where(e => e.UserId == userId).ToListAsync();
              return Ok(events);
          }

          [HttpGet("{id}")]
          public async Task<IActionResult> GetEvent(int id)
          {
              var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { error = "User ID not found in token" });
              var userId = int.Parse(userIdClaim);
              var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
              if (evt == null) return NotFound(new { error = "Event not found" });
              return Ok(evt);
          }

          [HttpPut("{id}")]
          public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
          {
              var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { error = "User ID not found in token" });
              var userId = int.Parse(userIdClaim);
              var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
              if (evt == null) return NotFound(new { error = "Event not found" });

              evt.Title = updatedEvent.Title;
              evt.Description = updatedEvent.Description;
              evt.Date = updatedEvent.Date;
              await _context.SaveChangesAsync();
              return Ok(evt);
          }

          [HttpDelete("{id}")]
          public async Task<IActionResult> DeleteEvent(int id)
          {
              var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { error = "User ID not found in token" });
              var userId = int.Parse(userIdClaim);
              var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
              if (evt == null) return NotFound(new { error = "Event not found" });

              _context.Events.Remove(evt);
              await _context.SaveChangesAsync();
              return NoContent();
          }
      }
  }