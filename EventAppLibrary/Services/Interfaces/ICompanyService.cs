using EventAppLibrary.Models;

namespace EventsLibrary.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> GetCompanyAsync(int companyId);
        Task UpdateCompanyAsync(Company updatedCompany);
    }
}