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
    [Route("api/[controller]")]
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

        // POST: api/eventrequests
        [HttpPost]
        // No [Authorize] here so users can create event requests without logging in
        public async Task<IActionResult> PostEventRequest([FromBody] EventifyBackend.Models.EventRequest eventRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // Set server-side values
            eventRequest.Id = 0; // Ensure treated as new entity
            eventRequest.CreatedAt = DateTime.UtcNow;
            eventRequest.UpdatedAt = DateTime.UtcNow;

            if (string.IsNullOrWhiteSpace(eventRequest.Status))
                eventRequest.Status = "Pending";

            _context.EventRequests.Add(eventRequest);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEventRequest), new { id = eventRequest.Id }, eventRequest);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while saving EventRequest: {@EventRequest}", eventRequest);
                return StatusCode(500, new { error = "Database update error", details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error while saving EventRequest: {@EventRequest}", eventRequest);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        // GET: api/eventrequests
        [HttpGet]
        [Authorize(Roles = "Admin")] // Restrict to admins only
        public async Task<ActionResult<IEnumerable<EventifyBackend.Models.EventRequest>>> GetEventRequests()
        {
            var eventRequests = await _context.EventRequests.ToListAsync();
            return Ok(eventRequests);
        }

        // GET: api/eventrequests/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")] // Restrict to admins only
        public async Task<ActionResult<EventifyBackend.Models.EventRequest>> GetEventRequest(int id)
        {
            var eventRequest = await _context.EventRequests.FirstOrDefaultAsync(er => er.Id == id);
            if (eventRequest == null)
                return NotFound(new { error = "Event request not found" });

            return Ok(eventRequest);
        }

        // PUT: api/eventrequests/{id}/accept
        [HttpPut("{id}/accept")]
        [Authorize(Roles = "Admin")] // Restrict to admins only
        public async Task<IActionResult> AcceptEventRequest(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var request = await _context.EventRequests.FirstOrDefaultAsync(er => er.Id == id);
            if (request == null)
                return NotFound(new { error = "Event request not found" });

            request.Status = "Accepted";
            request.UserId = userId; // Assign the processing admin's userId
            request.UpdatedAt = DateTime.UtcNow;

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

        // DELETE: api/eventrequests/{id}/deny
        [HttpDelete("{id}/deny")]
        [Authorize(Roles = "Admin")] // Restrict to admins only
        public async Task<IActionResult> DenyEventRequest(int id)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return Unauthorized(new { error = "User not authenticated" });

            var request = await _context.EventRequests.FirstOrDefaultAsync(er => er.Id == id);
            if (request == null)
                return NotFound(new { error = "Event request not found" });

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

        // Helper to extract UserId from JWT token claims
        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }
}