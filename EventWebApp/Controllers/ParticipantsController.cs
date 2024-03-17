using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;
using EventWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.Controllers
{
    public class ParticipantsController : Controller
    {
        private readonly IEventService _eventService;
        public ParticipantsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index(int eventId)
        {
            var eventWithParticipants = await _eventService.GetEventWithParticipantsAsync(eventId);
            if (eventWithParticipants == null)
            {
                return NotFound();
            }
            return View(eventWithParticipants);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteParticipant(int eventId, int participantId, bool isCompany)
        {
            await _eventService.DeleteParticipantFromEventAsync(eventId, participantId, isCompany);
            return RedirectToAction("Index", new { eventId });
        }

        public IActionResult AddParticipant(int eventId)
        {
            var model = new AddParticipantViewModel
            {
                EventId = eventId
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddParticipantToEvent(int eventId, string participantType, IFormCollection form)
        {
            if (participantType == "Person")
            {
                if (!string.IsNullOrWhiteSpace(form["FirstName"]) &&
                    !string.IsNullOrWhiteSpace(form["LastName"]) &&
                    !string.IsNullOrWhiteSpace(form["PersonalCode"]) &&
                    !string.IsNullOrWhiteSpace(form["PaymentMethod"]))
                {
                    PaymentMethod paymentMethod;
                    bool isPaymentMethodValid = Enum.TryParse(form["PaymentMethod"], true, out paymentMethod);

                    if (!isPaymentMethodValid)
                    {
                        paymentMethod = PaymentMethod.BankTransfer;
                    }

                    var personDetails = new Person
                    {
                        FirstName = form["FirstName"],
                        LastName = form["LastName"],
                        PersonalCode = form["PersonalCode"],
                        PaymentMethod = paymentMethod,
                        AdditionalInformation = form["AdditionalInformation"]
                    };
                    await _eventService.AddPersonToEventAsync(eventId, personDetails);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("FirstName", "First name is required");
                    ModelState.AddModelError("LastName", "Last name is required");
                    ModelState.AddModelError("PersonalCode", "Personal code is required");
                    ModelState.AddModelError("PaymentMethod", "Payment method is required");
                }
            }

            if (participantType == "Company")
            {
                if (!string.IsNullOrWhiteSpace(form["Name"]) &&
                                       !string.IsNullOrWhiteSpace(form["CompanyRegistrationCode"]) &&
                                                          !string.IsNullOrWhiteSpace(form["NumberOfParticipants"]) &&
                                                                             !string.IsNullOrWhiteSpace(form["PaymentMethod"]))
                {
                    PaymentMethod paymentMethod;
                    bool isPaymentMethodValid = Enum.TryParse(form["PaymentMethod"], true, out paymentMethod);

                    if (!isPaymentMethodValid)
                    {
                        paymentMethod = PaymentMethod.BankTransfer;
                    }

                    var companyDetails = new Company
                    {
                        Name = form["Name"],
                        CompanyRegistrationCode = form["CompanyRegistrationCode"],
                        NumberOfParticipants = int.Parse(form["NumberOfParticipants"]),
                        PaymentMethod = paymentMethod,
                        AdditionalInformation = form["AdditionalInformation"]
                    };
                    await _eventService.AddCompanyToEventAsync(eventId, companyDetails);
                    return RedirectToAction("Index", "Home");

                }
            }
            return BadRequest();
        }
    }
}
