using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventifyBackend.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ArchivesController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public ArchivesController(EventifyDbContext context)
        {
            _context = context;
        }

        [HttpPost("events/{eventId}/archive")]
        public async Task<IActionResult> ArchiveEvent(int eventId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { error = "User not authenticated" });

            var evt = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.UserId == userId);
            if (evt == null)
                return NotFound(new { error = "Event not found or unauthorized" });

            // Mark the event as archived
            evt.Archived = true;
            evt.UpdatedAt = DateTime.UtcNow; // Ensure UTC

            // Create archive record
            var archive = new Archive
            {
                EventId = evt.Id,
                Title = evt.Title,
                Description = evt.Description,
                Date = evt.Date, // Preserve the original event date
                UserId = userId,
                CreatedAt = DateTime.UtcNow, // Ensure UTC
                UpdatedAt = DateTime.UtcNow // Ensure UTC
            };

            _context.Archives.Add(archive);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetArchive), new { archiveId = archive.Id }, archive);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Database update error", details = ex.InnerException?.Message ?? ex.Message });
            }
        }

        [HttpGet("archives")]
        public async Task<IActionResult> GetArchives()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { error = "User not authenticated" });

            var archives = await _context.Archives
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return Ok(archives);
        }

        [HttpGet("archives/{archiveId}")]
        public async Task<IActionResult> GetArchive(int archiveId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { error = "User not authenticated" });

            var archive = await _context.Archives
                .FirstOrDefaultAsync(a => a.Id == archiveId && a.UserId == userId);

            if (archive == null)
                return NotFound(new { error = "Archive not found or unauthorized" });

            return Ok(archive);
        }

        [HttpDelete("archives/{archiveId}")]
        public async Task<IActionResult> DeleteArchive(int archiveId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { error = "User not authenticated" });

            var archive = await _context.Archives
                .FirstOrDefaultAsync(a => a.Id == archiveId && a.UserId == userId);

            if (archive == null)
                return NotFound(new { error = "Archive not found or unauthorized" });

            _context.Archives.Remove(archive);
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Database update error", details = ex.InnerException?.Message ?? ex.Message });
            }
        }
    }
}