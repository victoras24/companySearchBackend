using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.Extensions.Caching.Memory;
using Postgrest;
using Postgrest.Responses;

namespace CompanySearchBackend.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public CompanyRepository(Supabase.Client supabaseClient, ILogger<CompanyRepository> logger, IMemoryCache memoryCache)
        {
            _supabaseClient = supabaseClient;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<List<Organisation>> GetCompanyAsync(string name)
        {
            try
            {
                name = name.Trim().ToLower();
                
                if (_memoryCache.TryGetValue("companies", out List<Organisation>? cachedCompanies))
                {
                    if (cachedCompanies != null)
                    {
                        var filteredCompanies = cachedCompanies
                            .Where(c => c.OrganisationName != null && c.OrganisationName
                                .ToLower()
                                .Contains(name))
                            .Take(5)  
                            .ToList();
                    
                        if (filteredCompanies.Any())
                        {
                            _logger.LogInformation($"Returning cached results for company search: {name}");
                            return filteredCompanies;
                        }
                    }
                }
                
                var response = await _supabaseClient
                    .From<Organisation>()
                    .Filter(x => x.OrganisationName, Constants.Operator.ILike, $"%{name}%")
                    .Limit(5)
                    .Get()
                    .ConfigureAwait(false);
                return response.Models.ToList();
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
            .Limit(5)
            .Get();
        
        return response.Models.ToList();
        }
        
        public async Task<List<Organisation>> GetInactiveOrganisation(string name)
        {
            var response = await _supabaseClient.From<Organisation>()
                .Filter(x => x.OrganisationName, Constants.Operator.ILike, $"%{name}%")
                .Where(x => x.OrganisationStatus != "Διαγραμμένη")
                .Limit(5)
                .Get();
        
            return response.Models.ToList();
        }
        
        public async Task<List<Officials>> GetOfficialsAsync(string name)
        {
            var response = await _supabaseClient.From<Officials>()
                .Filter(x => x.PersonOrOrganisationName, Constants.Operator.ILike, $"%{name}%")
                .Limit(5)
                .Get();
        
            return response.Models;
        }

        public async Task<List<Organisation>> GetAllCompanies(int page, int pageSize)
        {
            var actualPageSize = Math.Min(pageSize, 1000);
            var offset = page * actualPageSize;
            var limit = offset + actualPageSize - 1;
    
            _logger?.LogDebug($"Requesting Supabase range: {offset} to {limit} (page {page}, pageSize {actualPageSize})");
    
            var response = await _supabaseClient.From<Organisation>()
                .Range(offset, limit)
                .Order(organisation => organisation.Id, Constants.Ordering.Ascending)
                .Get();
    
            _logger?.LogDebug($"Supabase returned {response.Models?.Count ?? 0} records for range {offset}-{limit}");
    
            return response.Models?.ToList() ?? new List<Organisation>();
        }
    }
}