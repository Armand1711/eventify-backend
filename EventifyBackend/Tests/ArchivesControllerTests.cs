using Xunit;
using Microsoft.EntityFrameworkCore;
using EventifyBackend.Controllers;
using EventifyBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace EventifyBackend.Tests
{
    public class ArchivesControllerTests
    {
        private ArchivesController GetControllerWithUser(EventifyDbContext context, int userId = 1)
        {
            var controller = new ArchivesController(context);
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
        public async Task ArchiveEvent_ValidEvent_ReturnsCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EventifyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var db = new EventifyDbContext(options);

            db.Events.Add(new Event { Id = 1, Title = "Test", Date = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Archived = false });
            db.SaveChanges();

            var controller = GetControllerWithUser(db);

            // Act
            var result = await controller.ArchiveEvent(1);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetArchive", createdResult.ActionName);
        }
    }
}