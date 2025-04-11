using CompanySearchBackend.Dtos;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanySearchBackend.Repository;

public class OfficialRepository(CompanyDbContext companyDb) : IOfficialRepository
{
    private readonly CompanyDbContext _companyDb = companyDb;

    public async Task<List<OrganisationOfficialDto>> GetOfficialAsync(string name)
    {
        var query = $@"SELECT TOP 10 REGISTRATION_NO as RegistrationNo, ORGANISATION_NAME as OrganisationName,
        STRING_AGG(Person_Or_Organisation_Name + ' is ' + OFFICIAL_POSITION + ' at ' + ORGANISATION_NAME, ', ') AS Officials
    FROM organisation_officials
    WHERE Person_Or_Organisation_Name LIKE N'%{name}%'
    GROUP BY REGISTRATION_NO, ORGANISATION_NAME
";

        return await _companyDb.OrganisationOfficials.FromSqlRaw(query).ToListAsync();

    }
}