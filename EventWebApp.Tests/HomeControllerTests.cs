using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;
using EventWebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventWebApp.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            var httpContext = new DefaultHttpContext();
            _loggerMock = new Mock<ILogger<HomeController>>();
            _eventServiceMock = new Mock<IEventService>();
            _controller = new HomeController(_loggerMock.Object, _eventServiceMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithAListOfEvents()
        {
            // Arrange
            _eventServiceMock.Setup(service => service.GetAllEventsAsync()).ReturnsAsync(new List<Event>());


            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Event>>(viewResult.ViewData.Model);
            _eventServiceMock.Verify(service => service.GetAllEventsAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteEvent_RedirectsToIndex()
        {
            // Arrange
            var eventId = 1;
            _eventServiceMock.Setup(service => service.DeleteEventAsync(eventId))
        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteEvent(eventId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(HomeController.Index), redirectToActionResult.ActionName);

            Assert.Equal(nameof(HomeController.Index), redirectToActionResult.ActionName);
            _eventServiceMock.Verify(service => service.DeleteEventAsync(eventId), Times.Once, "DeleteEventAsync was not called as expected.");
        }

        [Fact]
        public async Task Index_ReturnsErrorView_WhenServiceThrowsException()
        {
            // Arrange
            _eventServiceMock.Setup(service => service.GetAllEventsAsync())
                .ThrowsAsync(new Exception("Database context is null"));

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }

        [Fact]
        public async Task DeleteEvent_ReturnsErrorView_WhenServiceThrowsException()
        {
            // Arrange
            var eventId = 1;
            _eventServiceMock.Setup(service => service.DeleteEventAsync(eventId))
                .ThrowsAsync(new KeyNotFoundException("Event not found."));

            // Act
            var result = await _controller.DeleteEvent(eventId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Error", redirectToActionResult.ActionName);
        }
    }
}
