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
            try
            {
                var events = await _eventService.GetAllEventsAsync();
                return View(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all events.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            try
            {
                await _eventService.DeleteEventAsync(eventId);
            }
            catch (KeyNotFoundException)
            {
                return RedirectToAction(nameof(Error));
            }
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
