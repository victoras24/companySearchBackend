using System.Text.Json;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CompanySearchBackend.Services;

public class CompanyService : ICompanyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CompanyService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly string _searchCacheKeyPrefix = "companySearch_";
    private readonly string _detailsCacheKeyPrefix = "companyDetails_";

    private const string OrganisationResourceId = "b48bf3b6-51f2-4368-8eaa-63d61836aaa9";

    private readonly MemoryCacheEntryOptions _searchCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    private readonly MemoryCacheEntryOptions _detailsCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    public CompanyService(HttpClient httpClient, ILogger<CompanyService> logger, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<List<Company>> SearchCompaniesAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new List<Company>();
        }

        var normalizedSearchTerm = searchTerm.Trim().ToLowerInvariant();
        var cacheKey = $"{_searchCacheKeyPrefix}{normalizedSearchTerm}";

        if (_memoryCache.TryGetValue(cacheKey, out List<Company>? cachedResults))
        {
            _logger.LogInformation("Retrieved search results from cache for term: {SearchTerm}", searchTerm);
            return cachedResults ?? new List<Company>();
        }

        try
        {
            _logger.LogInformation("Fetching search results from API for term: {SearchTerm}", searchTerm);
            
            var queryParams = new List<string>
            {
                $"resource_id={OrganisationResourceId}",
                $"q={Uri.EscapeDataString(searchTerm)}",
            };
        
            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            
            
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var companies = JsonSerializer.Deserialize<List<Company>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Company>();

            _memoryCache.Set(cacheKey, companies, _searchCacheOptions);
            _logger.LogInformation("Cached search results for term: {SearchTerm}. Found {Count} companies", searchTerm, companies.Count);

            return companies;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching organisations with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<Company?> GetDetailedCompanyDataAsync( string registrationNo)
    {
        if ( string.IsNullOrWhiteSpace(registrationNo))
        {
            _logger.LogWarning("Invalid parameters provided for GetDetailedCompanyDataAsync");
            return null;
        }

        var cacheKey = $"{_detailsCacheKeyPrefix}{registrationNo}";

        if (_memoryCache.TryGetValue(cacheKey, out Company? cachedResult))
        {
            _logger.LogInformation("Retrieved company details from cache for registration: {RegistrationNo}", registrationNo);
            return cachedResult;
        }

        try
        {
            _logger.LogInformation("Fetching company details from API for registration: {RegistrationNo}", registrationNo);
            
            var queryParams = new List<string>
            {
                $"resource_id={OrganisationResourceId}",
                $"filters[registration_no]={registrationNo}"
            };
            
            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var companies = JsonSerializer.Deserialize<List<Company>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var company = companies?.FirstOrDefault();
            
            _memoryCache.Set(cacheKey, company, _detailsCacheOptions);
            
            if (company != null)
            {
                _logger.LogInformation("Cached company details for registration: {RegistrationNo}", registrationNo);
            }
            else
            {
                _logger.LogInformation("No company found for registration: {RegistrationNo} - cached null result", registrationNo);
            }

            return company;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting detailed company data for registrationNo: {RegistrationNo}", registrationNo);
            throw;
        }
    }

    // Optional: Method to clear cache if needed
    public void ClearCache()
    {
        if (_memoryCache is MemoryCache mc)
        {
            mc.Clear();
            _logger.LogInformation("Cache cleared");
        }
    }

    // Optional: Method to clear specific search cache
    public void ClearSearchCache(string searchTerm)
    {
        var normalizedSearchTerm = searchTerm.Trim().ToLowerInvariant();
        var cacheKey = $"{_searchCacheKeyPrefix}{normalizedSearchTerm}";
        _memoryCache.Remove(cacheKey);
        _logger.LogInformation("Cleared cache for search term: {SearchTerm}", searchTerm);
    }

    // Optional: Method to clear specific company details cache
    public void ClearCompanyDetailsCache(string addressSeqNo, string entryId, string registrationNo)
    {
        var cacheKey = $"{_detailsCacheKeyPrefix}{addressSeqNo}_{entryId}_{registrationNo}";
        _memoryCache.Remove(cacheKey);
        _logger.LogInformation("Cleared cache for company with registration: {RegistrationNo}", registrationNo);
    }
}