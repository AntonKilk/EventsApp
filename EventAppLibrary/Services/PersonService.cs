using EventAppLibrary.DataAccess;
using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;

namespace EventAppLibrary.Services
{
    public class PersonService : IPersonService
    {
        private readonly EventContext _context;

        public PersonService(EventContext context)
        {
            _context = context;
        }

        public async Task<Person> GetPersonAsync(int personId)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                throw new ArgumentException("Person not found.");
            }
            return person;
        }

        public async Task UpdatePersonAsync(Person updatedPerson)
        {
            var person = await _context.Persons.FindAsync(updatedPerson.Id);
            if (person == null)
            {
                throw new ArgumentException("Person not found.");
            }

            _context.Entry(person).CurrentValues.SetValues(updatedPerson);
            await _context.SaveChangesAsync();
        }
    }
}
