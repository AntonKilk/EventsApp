using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;
using EventWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.Controllers
{
    public class ParticipantsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IPersonService _personService;
        private readonly ICompanyService _companyService;
        public ParticipantsController(IEventService eventService, ICompanyService companyService, IPersonService personService)
        {
            _eventService = eventService;
            _companyService = companyService;
            _personService = personService;
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

                    if (form["PersonalCode"].ToString().Length != 11)
                    {
                        ModelState.AddModelError("PersonalCode", "Personal code must be 11 characters long");
                        return View("AddParticipant", new AddParticipantViewModel { EventId = eventId });
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

        public async Task<IActionResult> CompanyDetails(int id)
        {
            var company = await _companyService.GetCompanyAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        public async Task<IActionResult> PersonDetails(int id)
        {
            var person = await _personService.GetPersonAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, IFormCollection form, bool isCompany)
        {
            if (isCompany)
            {
                var company = await _companyService.GetCompanyAsync(id);
                if (company == null)
                {
                    return NotFound();
                }
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

                    company.Name = form["Name"];
                    company.CompanyRegistrationCode = form["CompanyRegistrationCode"];
                    company.NumberOfParticipants = int.Parse(form["NumberOfParticipants"]);
                    company.PaymentMethod = paymentMethod;
                    company.AdditionalInformation = form["AdditionalInformation"];

                    await _companyService.UpdateCompanyAsync(company);
                    return RedirectToAction("CompanyDetails", new { id, isCompany = true });
                }
                else
                {
                    ModelState.AddModelError("Name", "Name is required");
                    ModelState.AddModelError("CompanyRegistrationCode", "Company registration code is required");
                    ModelState.AddModelError("NumberOfParticipants", "Number of participants is required");
                    ModelState.AddModelError("PaymentMethod", "Payment method is required");
                    return RedirectToAction("CompanyDetails", new { id, isCompany = true });
                }
            }
            else
            {
                var person = await _personService.GetPersonAsync(id);
                if (person == null)
                {
                    return NotFound();
                }
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

                    if (form["PersonalCode"].ToString().Length != 11)
                    {
                        ModelState.AddModelError("PersonalCode", "Personal code must be 11 characters long");
                        return View("AddParticipant", new AddParticipantViewModel { EventId = id });
                    }

                    person.FirstName = form["FirstName"];
                    person.LastName = form["LastName"];
                    person.PersonalCode = form["PersonalCode"];
                    person.PaymentMethod = paymentMethod;
                    person.AdditionalInformation = form["AdditionalInformation"];

                    await _personService.UpdatePersonAsync(person);
                    return RedirectToAction("PersonDetails", new { id, isCompany = false });

                }
                else
                {
                    ModelState.AddModelError("FirstName", "First name is required");
                    ModelState.AddModelError("LastName", "Last name is required");
                    ModelState.AddModelError("PersonalCode", "Personal code is required");
                    ModelState.AddModelError("PaymentMethod", "Payment method is required");
                }
                return RedirectToAction("PersonDetails", new { id, isCompany = false });
            }
        }
    }
}
