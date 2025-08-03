using Xunit;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Controllers;
using EventifyBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace EventifyBackend.Tests
{
    public class EventsControllerTests
    {
        private EventsController GetControllerWithUser(EventifyDbContext context, int userId = 1)
        {
            var controller = new EventsController(context);
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
        public async Task CreateEvent_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EventifyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var db = new EventifyDbContext(options);

            var controller = GetControllerWithUser(db, 1);

            var request = new EventsController.EventDto { Title = "Test", Date = DateTime.UtcNow };

            // Act
            var result = await controller.CreateEvent(request);

            // Assert
            var actionResult = Assert.IsType<ActionResult<EventsController.EventDto>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("GetEvent", createdResult.ActionName);
        }
    }
}