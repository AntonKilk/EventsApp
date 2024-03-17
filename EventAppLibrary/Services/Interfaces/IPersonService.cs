using EventAppLibrary.Models;

namespace EventsLibrary.Services.Interfaces
{
    public interface IPersonService
    {
        Task<Person> GetPersonAsync(int personId);
        Task UpdatePersonAsync(Person updatedPerson);
    }
}