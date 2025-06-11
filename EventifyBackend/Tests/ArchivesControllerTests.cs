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
    public class ArchivesControllerTests
    {
        private readonly Mock<EventifyDbContext> _mockContext;
        private readonly ArchivesController _controller;

        public ArchivesControllerTests()
        {
            _mockContext = new Mock<EventifyDbContext>();
            _controller = new ArchivesController(_mockContext.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
            { new Claim(ClaimTypes.NameIdentifier, "1") }, "mock"));
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        [Fact]
        public async Task ArchiveEvent_ValidEvent_ReturnsCreated()
        {
            var eventItem = new Event { Id = 1, Title = "Test", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Archived = false };
            _mockContext.Setup(c => c.Archives.Add(It.IsAny<Archive>())).Callback<Archive>(a => 
            { a.Id = 1; a.CreatedAt = DateTime.UtcNow; a.UpdatedAt = DateTime.UtcNow; });
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _controller.ArchiveEvent(1);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetArchive", createdResult.ActionName);
        }
    }
}