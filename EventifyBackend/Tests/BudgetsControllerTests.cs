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
    public class BudgetsControllerTests
    {
        private BudgetsController GetControllerWithUser(EventifyDbContext context, int userId = 1)
        {
            var controller = new BudgetsController(context);
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
        public async Task CreateBudget_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EventifyDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            using var db = new EventifyDbContext(options);

            // Add a task to associate the budget with
            db.EventTasks.Add(new EventTasks { Id = 1, UserId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Archived = false });
            db.SaveChanges();

            var controller = GetControllerWithUser(db, 1);
            var budget = new Budget { Amount = 100, Category = "Travel" };

            // Act
            var result = await controller.CreateBudget(1, budget);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedBudget = Assert.IsType<Budget>(createdResult.Value);
            Assert.Equal("GetBudget", createdResult.ActionName);
            Assert.Equal(1, returnedBudget.TaskId);
            Assert.Equal(1, returnedBudget.UserId);
        }
    }
}