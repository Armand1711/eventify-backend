using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Controllers;
using EventifyBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventifyBackend.Tests
{
    public class EventsControllerTests
    {
        private readonly Mock<EventifyDbContext> _mockContext;
        private readonly EventsController _controller;

        public EventsControllerTests()
        {
            _mockContext = new Mock<EventifyDbContext>();
            _controller = new EventsController(_mockContext.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
            { new Claim(ClaimTypes.NameIdentifier, "1") }, "mock"));
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        [Fact]
        public async Task CreateEvent_ValidRequest_ReturnsCreated()
        {
            var request = new EventsController.EventDto { Title = "Test" };
            var newEvent = new Event { Id = 1, Title = "Test", UserId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Archived = false };
            _mockContext.Setup(c => c.Events.Add(It.IsAny<Event>())).Callback<Event>(e => e.Id = 1);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _controller.CreateEvent(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetEvent", createdResult.ActionName);
        }
    }
}