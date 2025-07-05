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
        private readonly ILogger _logger;

        public CompanyRepository(Supabase.Client supabaseClient, ILogger<CompanyRepository> logger)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
        }

        public async Task<List<Organisation>> GetCompanyAsync(string name)
        {
            try
            {
                var response = await _supabaseClient
                    .From<Organisation>()
                    .Filter(x => x.OrganisationName, Constants.Operator.ILike, $"%{name}%")
                    .Limit(5)
                    .Get()
                    .ConfigureAwait(false);
                return response.Models.ToList();
            }
            catch (Postgrest.Exceptions.PostgrestException ex) when (ex.Message.Contains("57014"))
            {
                _logger.LogWarning($"Query timeout for company search: {name}");
        
                return new List<Organisation>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error searching for company: {name}");
                throw; 
            }
            

        }

        public async Task<Task<ModeledResponse<Organisation>>> GetCompanyDetailed(string registrationNo)
        {
            var company = _supabaseClient.From<Organisation>()
                .Filter(x => x.RegistrationNo, Constants.Operator.Equals, registrationNo)
                .Get();
            
            return company;
        }

        public async Task<ModeledResponse<Address>> GetCompanyAddress(int addressSeqNo)
        {
            var companyAddress = await _supabaseClient.From<Address>()
                .Filter(x => x.AddressSeqNo, Constants.Operator.Equals, addressSeqNo)
                .Get();
            
            return companyAddress;
        }

        public async Task<List<Officials>> GetCompanyKeyPeople(string registrationNo)
        {
            var companyKeyPeople = _supabaseClient.From<Officials>()
                .Filter(x => x.RegistrationNo, Constants.Operator.Equals, registrationNo)
                .Get()
                .Result
                .Models;
            
            return companyKeyPeople;
        }

        public Task<List<Officials>> GetCompanyRelated(string organizationName)
        {
            var related = _supabaseClient.From<Officials>()
                .Filter(x => x.PersonOrOrganisationName, Constants.Operator.Equals, organizationName)
                .Get()
                .Result
                .Models;
            
            return Task.FromResult(related);
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