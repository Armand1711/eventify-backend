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
    public class BudgetsControllerTests
    {
        private readonly Mock<EventifyDbContext> _mockContext;
        private readonly BudgetsController _controller;

        public BudgetsControllerTests()
        {
            _mockContext = new Mock<EventifyDbContext>();
            _controller = new BudgetsController(_mockContext.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
            { new Claim(ClaimTypes.NameIdentifier, "1") }, "mock"));
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        [Fact]
        public async Task CreateBudget_ValidRequest_ReturnsCreated()
        {
            var taskId = 1;
            var task = new EventTasks { Id = taskId, UserId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Archived = false };
            var budget = new Budget { Amount = 100, Category = "Travel" };
            _mockContext.Setup(c => c.Budgets.Add(It.IsAny<Budget>())).Callback<Budget>(b => b.Id = 1);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _controller.CreateBudget(taskId, budget);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetBudget", createdResult.ActionName);
        }
    }
}