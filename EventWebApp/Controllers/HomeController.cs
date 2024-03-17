using EventsLibrary.Services.Interfaces;
using EventWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EventWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventService;
        public HomeController(ILogger<HomeController> logger, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _eventService.GetAllEventsAsync();
            return View(events);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            await _eventService.DeleteEventAsync(eventId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
