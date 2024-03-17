using EventAppLibrary.Models;

namespace EventsLibrary.Services.Interfaces
{
    public interface ICompanyService
    {
        Task CreateCompanyAsync(Company company);
        Task DeleteCompanyAsync(int companyId);
        Task UpdateCompanyAsync(Company updatedCompany);
    }
}