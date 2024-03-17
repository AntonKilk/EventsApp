using EventAppLibrary.Models;

namespace EventsLibrary.Services.Interfaces
{
    public interface IEventService
    {
        Task<Event> GetEventWithParticipantsAsync(int eventId);
        Task CreateEventAsync(Event newEvent);
        Task DeleteEventAsync(int eventId);
        Task<List<Event>> GetAllEventsAsync();
        Task DeleteParticipantFromEventAsync(int eventId, int participantId, bool isCompany);

        Task AddCompanyToEventAsync(int eventId, Company company);
        Task AddPersonToEventAsync(int eventId, Person person);
    }
}