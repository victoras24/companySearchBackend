using CompanySearchBackend.Dtos;
using CompanySearchBackend.Models;
using Postgrest.Responses;

namespace CompanySearchBackend.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<Organisation>> GetCompanyAsync(string name);

        Task<Task<ModeledResponse<Organisation>>> GetCompanyDetailed(string registrationNo);
        
        Task<ModeledResponse<Address>> GetCompanyAddress(int registrationNo);
        
        Task<List<Officials>> GetCompanyKeyPeople(string registrationNo);
        Task<List<Officials>> GetCompanyRelated(string organisationName);
        
        Task<List<Organisation>> GetActiveOrganisation(string name);
        
        Task<List<Organisation>> GetInactiveOrganisation(string name);
    }
}