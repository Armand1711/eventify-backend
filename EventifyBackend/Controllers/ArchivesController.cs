using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EventifyBackend.Models;

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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId && e.UserId == userId);
            if (evt == null) return NotFound(new { error = "Event not found" });

            var archive = new Archive
            {
                EventId = evt.Id,
                Title = evt.Title,
                Description = evt.Description,
                Date = evt.Date,
                UserId = userId
            };
            _context.Archives.Add(archive);
            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
            return StatusCode(201, archive);
        }

        [HttpGet("archives")]
        public async Task<IActionResult> GetArchives()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var archives = await _context.Archives.Where(a => a.UserId == userId).ToListAsync();
            return Ok(archives);
        }

        [HttpGet("archives/{archiveId}")]
        public async Task<IActionResult> GetArchive(int archiveId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var archive = await _context.Archives.FirstOrDefaultAsync(a => a.Id == archiveId && a.UserId == userId);
            if (archive == null) return NotFound(new { error = "Archive not found" });
            return Ok(archive);
        }

        [HttpDelete("archives/{archiveId}")]
        public async Task<IActionResult> DeleteArchive(int archiveId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var archive = await _context.Archives.FirstOrDefaultAsync(a => a.Id == archiveId && a.UserId == userId);
            if (archive == null) return NotFound(new { error = "Archive not found" });

            _context.Archives.Remove(archive);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}