using CompanySearchBackend.Dtos;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Mappers;
using CompanySearchBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgrest;
using Postgrest.Responses;

namespace CompanySearchBackend.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly Supabase.Client _supabaseClient;
        
        public CompanyRepository(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<Organisation>> GetCompanyAsync(string name)
        {
            var response = await _supabaseClient
                .From<Organisation>()
                .Filter(x => x.OrganisationName, Constants.Operator.ILike, $"%{name}%")
                .Limit(10)
                .Get();

            return response.Models.ToList();
        }

        public async Task<Organisation?> GetAddressAndOfficials(string registrationNo)
        {
            var response = await _supabaseClient.From<Organisation>()
                .Filter(x => x.RegistrationNo, Constants.Operator.Equals, registrationNo)
                .Get();
            return response.Model;
        }

        public async Task<List<Organisation>> GetActiveOrganisation(string name)
        {
        var response = await _supabaseClient.From<Organisation>()
            .Filter(x => x.OrganisationName, Constants.Operator.ILike, $"%{name}%")
            .Filter(x => x.OrganisationStatus, Constants.Operator.Equals, "Εγγεγραμμένη")
            .Limit(10)
            .Get();
        
        return response.Models.ToList();
        }
        
        public async Task<List<Organisation>> GetInactiveOrganisation(string name)
        {
            var response = await _supabaseClient.From<Organisation>()
                .Filter(x => x.OrganisationName, Constants.Operator.ILike, $"%{name}%")
                .Filter(x => x.OrganisationStatus, Constants.Operator.Equals, "Διαγραμμένη")
                .Limit(10)
                .Get();
        
            return response.Models.ToList();
        }
    }
}