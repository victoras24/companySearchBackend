using CompanySearchBackend.Models;

namespace CompanySearchBackend.Interfaces;

public interface ICompanyService
{
    Task<List<Company>> SearchCompaniesAsync(string searchTerm);
    Task<Company> GetDetailedCompanyDataAsync( string registrationNo, string entryId);
}