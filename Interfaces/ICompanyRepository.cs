using CompanySearchBackend.Dtos;
using CompanySearchBackend.Models;
using Postgrest.Responses;

namespace CompanySearchBackend.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Organisation>> GetCompanyAsync(string name);

        Task<OrganisationWithOfficialsAndAddress?> GetAddressAndOfficials(string registrationNo);
        
        Task<List<Organisation>> GetActiveOrganisation(string name);
        
        Task<List<Organisation>> GetInactiveOrganisation(string name);
    }
}