using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace EventifyBackend.Controllers
{
    [Route("api/eventrequests")]
    [ApiController]
    public class EventRequestsController : ControllerBase
    {
        private readonly EventifyDbContext _context;
        private readonly ILogger<EventRequestsController> _logger;

        public EventRequestsController(EventifyDbContext context, ILogger<EventRequestsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostEventRequest([FromBody] EventRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // Ignore client-provided Id, createdAt, and updatedAt; set them server-side
            request.Id = 0; // Ensure new record
            request.CreatedAt = DateTime.UtcNow; // Override client value
            request.UpdatedAt = DateTime.UtcNow; // Override client value

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                request.Status = "Pending";
            }

            _context.EventRequests.Add(request);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEventRequest), new { id = request.Id }, request);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while saving EventRequest: {@EventRequest}", request);
                return StatusCode(500, new { error = "Database update error", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error while saving EventRequest: {@EventRequest}", request);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EventRequest>>> GetEventRequests()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var eventRequests = await _context.EventRequests
                .Where(er => er.UserId == userId || er.UserId == null) // Filter by authenticated user or unprocessed
                .ToListAsync();
            return Ok(eventRequests);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EventRequest>> GetEventRequest(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var eventRequest = await _context.EventRequests
                .FirstOrDefaultAsync(er => er.Id == id && (er.UserId == userId || er.UserId == null));
            if (eventRequest == null)
                return NotFound();

            return Ok(eventRequest);
        }

        [HttpPut("{id}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptEventRequest(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var request = await _context.EventRequests.FirstOrDefaultAsync(er => er.Id == id);
            if (request == null)
                return NotFound();

            request.Status = "Accepted";
            request.UserId = userId; // Assign the authenticated user
            request.UpdatedAt = DateTime.UtcNow;

            var newEvent = new Event
            {
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                UserId = userId.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Archived = false
            };
            _context.Events.Add(newEvent);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(request);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while accepting EventRequest {Id}", id);
                return StatusCode(500, new { error = "Database update error", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpDelete("{id}/deny")]
        [Authorize]
        public async Task<IActionResult> DenyEventRequest(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized("User not authenticated");

            var request = await _context.EventRequests.FirstOrDefaultAsync(er => er.Id == id);
            if (request == null)
                return NotFound();

            _context.EventRequests.Remove(request);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error while denying EventRequest {Id}", id);
                return StatusCode(500, new { error = "Database update error", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }
}