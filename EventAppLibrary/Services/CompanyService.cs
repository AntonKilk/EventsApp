using EventAppLibrary.DataAccess;
using EventAppLibrary.Models;
using EventsLibrary.Services.Interfaces;

namespace EventAppLibrary.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly EventContext _context;

        public CompanyService(EventContext context)
        {
            _context = context;
        }

        public async Task CreateCompanyAsync(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCompanyAsync(Company updatedCompany)
        {
            var company = await _context.Companies.FindAsync(updatedCompany.Id);
            if (company == null)
            {
                throw new ArgumentException("Company not found.");
            }

            _context.Entry(company).CurrentValues.SetValues(updatedCompany);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
        }
    }
}
