using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Models;

namespace EventifyBackend.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventifyDbContext _context;

        public EventsController(EventifyDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null)
                return NotFound();
            return evt;
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] Event evt)
        {
            evt.CreatedAt = DateTime.UtcNow;
            evt.UpdatedAt = DateTime.UtcNow;
            _context.Events.Add(evt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, evt);
        }
    }
}