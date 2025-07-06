using CompanySearchBackend.Dtos;
using CompanySearchBackend.Models;

namespace CompanySearchBackend.Interfaces;

public interface IOfficialService
{
    Task<List<Officials>> GetOfficialsByRegistrationNoAsync(string registrationNo);
    Task<List<RelatedCompanyDto>> GetRelatedCompanies(string companyName);
}