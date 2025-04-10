using CompanySearchBackend.Dtos;
using CompanySearchBackend.Models;

namespace CompanySearchBackend.Interfaces;

public interface IOfficialRepository
{
    Task<List<OrganisationOfficialDto>> GetOfficialAsync(string name);
}