using CompanySearchBackend.Dtos;
using CompanySearchBackend.Models;

namespace CompanySearchBackend.Interfaces
{
    public interface ICompanyRepository
    {
        Task<List<CompanyNameDto>> GetCompanyAsync(string name);

        Task<AddressAndOfficialsDto> GetAddressAndOfficials(int registrationNo);
        
        Task<List<CompanyNameDto>> GetActiveOrganisation(string name);
        
        Task<List<CompanyNameDto>> GetInactiveOrganisation(string name);
    }
}