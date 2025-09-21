using CompanySearchBackend.Models;

namespace CompanySearchBackend.Interfaces;

public interface ICompanyService
{
    Task<bool> CacheAllCompaniesAsync();
    Task<List<Organisation>> GetCachedCompaniesAsync();
}