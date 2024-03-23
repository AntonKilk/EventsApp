using EventAppLibrary.DataAccess;
using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventAppLibrary.Services
{

    public class EventService : IEventService
    {
        private readonly EventContext _context;

        public EventService(EventContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            //handle service error
            if (_context == null)
            {
                throw new InvalidOperationException("Database context is null");
            }
            return await _context.Events
                .Include(e => e.RegisteredPersons)
                .Include(e => e.RegisteredCompanies)
                .ToListAsync();
        }

        public async Task<Event> GetEventWithParticipantsAsync(int eventId)
        {
            var singleEvent = await _context.Events
                .Include(e => e.RegisteredPersons)
                .Include(e => e.RegisteredCompanies)
                .FirstOrDefaultAsync(e => e.Id == eventId);
            if (singleEvent == null)
            {
                throw new ArgumentException("Event not found.");
            }
            return singleEvent;
        }

        public async Task DeleteEventAsync(int eventId)
        {
            var eventToDelete = await _context.Events
                .Include(e => e.RegisteredPersons)
                .Include(e => e.RegisteredCompanies)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventToDelete != null)
            {
                if (eventToDelete.DateAndTime < DateTime.Now)
                {
                    throw new InvalidOperationException("Past events cannot be deleted");
                }
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateEventAsync(Event newEvent)
        {
            if (newEvent.DateAndTime <= DateTime.Now)
            {
                throw new ArgumentException("Event date must be in the future.");
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
        }

        public async Task AddCompanyToEventAsync(int eventId, Company company)
        {
            var eventToUpdate = await _context.Events
                .Include(e => e.RegisteredPersons)
                .Include(e => e.RegisteredCompanies)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventToUpdate == null) return;

            _context.Companies.Add(company);
            eventToUpdate.RegisteredCompanies.Add(company);

            await _context.SaveChangesAsync();
        }

        public async Task AddPersonToEventAsync(int eventId, Person person)
        {
            var eventToUpdate = await _context.Events
                .Include(e => e.RegisteredPersons)
                .Include(e => e.RegisteredCompanies)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventToUpdate == null) return;

            _context.Persons.Add(person);
            eventToUpdate.RegisteredPersons.Add(person);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteParticipantFromEventAsync(int eventId, int participantId, bool isCompany)
        {
            var eventToUpdate = await _context.Events
                .Include(e => e.RegisteredPersons)
                .Include(e => e.RegisteredCompanies)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventToUpdate == null)
            {
                throw new ArgumentException("Event not found");
            }

            if (isCompany)
            {
                var companyToRemove = eventToUpdate.RegisteredCompanies.FirstOrDefault(c => c.Id == participantId);
                if (companyToRemove != null)
                {
                    eventToUpdate.RegisteredCompanies.Remove(companyToRemove);
                }
            }
            else
            {
                var personToRemove = eventToUpdate.RegisteredPersons.FirstOrDefault(p => p.Id == participantId);
                if (personToRemove != null)
                {
                    eventToUpdate.RegisteredPersons.Remove(personToRemove);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
