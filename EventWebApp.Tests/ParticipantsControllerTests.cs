using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;
using EventWebApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventWebApp.Tests
{
    public class ParticipantsControllerTests
    {
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly Mock<IPersonService> _personServiceMock;
        private readonly Mock<ICompanyService> _companyServiceMock;
        private readonly ParticipantsController _controller;

        public ParticipantsControllerTests()
        {
            _eventServiceMock = new Mock<IEventService>();
            _personServiceMock = new Mock<IPersonService>();
            _companyServiceMock = new Mock<ICompanyService>();
            _controller = new ParticipantsController(_eventServiceMock.Object, _companyServiceMock.Object, _personServiceMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
            };
        }

        [Fact]
        public async Task Index_EventExists_ReturnsViewWithModel()
        {
            // Arrange
            var eventId = 1;
            _eventServiceMock.Setup(service => service.GetEventWithParticipantsAsync(eventId))
                .ReturnsAsync(new Event { DateAndTime = DateTime.Now.AddDays(1), Name = "Tech Conference", EventPlace = "Conference Center" });

            // Act
            var result = await _controller.Index(eventId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Event>(viewResult.Model);
        }

        [Fact]
        public async Task Index_EventDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var eventId = 1;
            _eventServiceMock.Setup(service => service.GetEventWithParticipantsAsync(eventId))
                .ReturnsAsync(value: null);

            // Act
            var result = await _controller.Index(eventId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddParticipantToEvent_ValidPerson_SubmitsSuccessfully()
        {
            // Arrange
            var eventId = 1;
            var participantType = "Person";
            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                {"FirstName", "Test"},
                {"LastName", "User"},
                {"PersonalCode", "123456-7890"},
                {"PaymentMethod", "BankTransfer"}
            });

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Request.Form = formCollection;

            _eventServiceMock.Setup(service => service.AddPersonToEventAsync(eventId, It.IsAny<Person>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddParticipantToEvent(eventId, participantType, formCollection);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }

        [Fact]
        public async Task DeleteParticipant_CallsServiceAndRedirects()
        {
            // Arrange
            int eventId = 1;
            int participantId = 2;
            bool isCompany = true;

            _eventServiceMock.Setup(s => s.DeleteParticipantFromEventAsync(eventId, participantId, isCompany))
                .Returns(Task.CompletedTask)
                .Verifiable("The service method DeleteParticipantFromEventAsync was not called with expected parameters.");

            // Act
            var result = await _controller.DeleteParticipant(eventId, participantId, isCompany);

            // Assert
            _eventServiceMock.Verify();

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(eventId, redirectToActionResult?.RouteValues?["eventId"]);
        }

        [Fact]
        public async Task Edit_CompanyExistsAndValidData_RedirectsToCompanyDetails()
        {
            // Arrange
            int companyId = 1;
            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                {"Name", "Test Company"},
                {"CompanyRegistrationCode", "123456"},
                {"NumberOfParticipants", "10"},
                {"PaymentMethod", "BankTransfer"}
            });

            var company = new Company { Id = companyId, Name = "Good Company", CompanyRegistrationCode = "654321", NumberOfParticipants = 5, PaymentMethod = PaymentMethod.Cash };
            _companyServiceMock.Setup(x => x.GetCompanyAsync(companyId)).ReturnsAsync(company);

            _controller.ControllerContext.HttpContext.Request.Form = formCollection;

            // Act
            var result = await _controller.Edit(companyId, formCollection, isCompany: true);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CompanyDetails", redirectToActionResult.ActionName);
            Assert.True((bool)redirectToActionResult.RouteValues["isCompany"]);
            _companyServiceMock.Verify(x => x.UpdateCompanyAsync(It.IsAny<Company>()), Times.Once);
        }

        [Fact]
        public async Task Edit_CompanyDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            int companyId = 1;
            _companyServiceMock.Setup(x => x.GetCompanyAsync(companyId)).ReturnsAsync((Company)null);

            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = await _controller.Edit(companyId, formCollection, isCompany: true);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_CompanyExistsButInvalidData_RedirectsToCompanyDetailsWithModelStateErrors()
        {
            // Arrange
            int companyId = 1;
            var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());

            var company = new Company { Id = companyId, Name = "Test Company", CompanyRegistrationCode = "123456", NumberOfParticipants = 10, PaymentMethod = PaymentMethod.BankTransfer };
            _companyServiceMock.Setup(x => x.GetCompanyAsync(companyId)).ReturnsAsync(company);


            _controller.ControllerContext.HttpContext.Request.Form = formCollection;

            // Act
            var result = await _controller.Edit(companyId, formCollection, isCompany: true);

            // Assert
            Assert.Equal(4, _controller.ModelState.ErrorCount);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CompanyDetails", redirectToActionResult.ActionName);
            _companyServiceMock.Verify(x => x.UpdateCompanyAsync(It.IsAny<Company>()), Times.Never);
        }
    }
}
