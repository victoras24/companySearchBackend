using CompanySearchBackend.Dtos;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.EntityFrameworkCore;
using Postgrest;

namespace CompanySearchBackend.Repository;

public class OfficialRepository(Supabase.Client supabaseDb) : IOfficialRepository
{
    private readonly Supabase.Client _supabaseClient = supabaseDb;
    

    public async Task<List<OrganisationOfficial>> GetOrganisationOfficials(string registrationNo)
    {
        var response = await _supabaseClient.From<OrganisationOfficial>()
            .Filter(x => x.RegistrationNo, Constants.Operator.Equals, registrationNo)
            .Get();

        return response.Models;
    }

    public async Task<List<OrganisationOfficial>> GetOfficials(string name)
    {
        var response = await _supabaseClient.From<OrganisationOfficial>()
            .Filter(x => x.PersonOrOrganisationName, Constants.Operator.ILike, $"%{name}%")
            .Limit(10)
            .Get();
        
        return response.Models;
    }
}