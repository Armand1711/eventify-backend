using Xunit;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Controllers;
using EventifyBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System;
using Moq;

namespace EventifyBackend.Tests
{
    public class EventRequestsControllerTests
    {
        private EventRequestsController GetControllerWithUser(EventifyDbContext context, ILogger<EventRequestsController> logger = null, int userId = 1)
        {
            var controller = new EventRequestsController(context, logger ?? new Mock<ILogger<EventRequestsController>>().Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            return controller;
        }

        [Fact]
        public async Task PostEventRequest_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EventifyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var db = new EventifyDbContext(options);

            var logger = new Mock<ILogger<EventRequestsController>>().Object;
            var controller = GetControllerWithUser(db, logger);

            var request = new EventRequest { Title = "Test Event", Description = "Test", Date = DateTime.UtcNow };

            // Act
            var result = await controller.PostEventRequest(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetEventRequest", createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues["id"]);
        }
    }
}