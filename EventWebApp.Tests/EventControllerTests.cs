using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;
using EventWebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventWebApp.Tests
{
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly EventController _controller;

        public EventControllerTests()
        {
            _eventServiceMock = new Mock<IEventService>();
            _controller = new EventController(_eventServiceMock.Object);
        }

        [Fact]
        public async Task AddEvent_ValidModelFutureDate_RedirectsToHomeIndex()
        {
            // Arrange
            var mockEvent = new Event { DateAndTime = DateTime.Now.AddDays(1), Name = "Tech Conference", EventPlace = "Conference Center" }; // Future date
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.AddEvent(mockEvent);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            _eventServiceMock.Verify(s => s.CreateEventAsync(It.IsAny<Event>()), Times.Once);
        }

        [Fact]
        public async Task AddEvent_ValidModelPastDate_ReturnsViewWithModel()
        {
            // Arrange
            var mockEvent = new Event { DateAndTime = DateTime.Now.AddDays(-1), Name = "Tech Conference", EventPlace = "Conference Center" };
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.AddEvent(mockEvent);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Event>(viewResult.Model);
            Assert.Equal(mockEvent.DateAndTime, model.DateAndTime);
            _eventServiceMock.Verify(s => s.CreateEventAsync(It.IsAny<Event>()), Times.Never);
        }

        [Fact]
        public async Task AddEvent_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            var mockEvent = new Event { DateAndTime = DateTime.Now.AddDays(1), Name = "Tech Conference", EventPlace = "Conference Center" };
            _controller.ModelState.AddModelError("TestError", "Test error message");

            // Act
            var result = await _controller.AddEvent(mockEvent);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Event>(viewResult.Model);
            Assert.Equal(mockEvent, model);
            _eventServiceMock.Verify(s => s.CreateEventAsync(It.IsAny<Event>()), Times.Never);
        }
    }
}
