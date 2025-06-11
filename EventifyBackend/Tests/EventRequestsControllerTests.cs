using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Controllers;
using EventifyBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventifyBackend.Tests
{
    public class EventRequestsControllerTests
    {
        private readonly Mock<EventifyDbContext> _mockContext;
        private readonly Mock<ILogger<EventRequestsController>> _mockLogger;
        private readonly EventRequestsController _controller;

        public EventRequestsControllerTests()
        {
            _mockContext = new Mock<EventifyDbContext>();
            _mockLogger = new Mock<ILogger<EventRequestsController>>();
            _controller = new EventRequestsController(_mockContext.Object, _mockLogger.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
            { new Claim(ClaimTypes.NameIdentifier, "1") }, "mock"));
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        [Fact]
        public async Task PostEventRequest_ValidRequest_ReturnsCreated()
        {
            var request = new EventRequest { Title = "Test Event", Description = "Test", Date = DateTime.UtcNow };
            _mockContext.Setup(c => c.EventRequests.Add(It.IsAny<EventRequest>())).Callback<EventRequest>(r => r.Id = 1);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _controller.PostEventRequest(request);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetEventRequest", createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues["id"]);
        }
    }
}