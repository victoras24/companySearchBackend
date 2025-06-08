using CompanySearchBackend.Models;

namespace CompanySearchBackend.Interfaces;

public interface ICompanyService
{
    Task<List<Company>> SearchCompaniesAsync(string searchTerm);
    Task<CompanyAndAddress> GetDetailedCompanyDataAsync(string addressSeqNo, string entryId, string registrationNo);
}