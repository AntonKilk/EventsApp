using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventWebApp.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEvent(Event model)
        {
            if (ModelState.IsValid && model.DateAndTime > DateTime.Now)
            {
                await _eventService.CreateEventAsync(model);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
