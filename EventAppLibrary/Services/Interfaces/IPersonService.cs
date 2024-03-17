using EventAppLibrary.Models;

namespace EventsLibrary.Services.Interfaces
{
    public interface IPersonService
    {
        Task CreatePersonAsync(Person person);
        Task DeletePersonAsync(int personId);
        Task UpdatePersonAsync(Person updatedPerson);
    }
}