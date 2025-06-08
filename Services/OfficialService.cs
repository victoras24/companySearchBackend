using System.Text.Json;
using CompanySearchBackend.Dtos;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CompanySearchBackend.Services;

public class OfficialService(HttpClient httpClient, ILogger<OfficialService> logger, IMemoryCache memoryCache)
    : IOfficialService
{
    private readonly string _officialsByRegNoCachePrefix = "officialsByRegNo_";
    private readonly string _officialsSearchCachePrefix = "officialsSearch_";
    private readonly string _relatedCompaniesCachePrefix = "relatedCompanies_";

    private const string OfficialResourceId = "a1deb65d-102b-4e8e-9b9c-5b357d719477";

    
    private readonly MemoryCacheEntryOptions _officialsByRegNoCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)

    };

    private readonly MemoryCacheEntryOptions _officialsSearchCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    public async Task<List<Officials>> GetOfficialsByRegistrationNoAsync(string registrationNo)
    {
        if (string.IsNullOrWhiteSpace(registrationNo))
        {
            logger.LogWarning("Registration number is null or empty");
            return new List<Officials>();
        }

        var normalizedRegNo = registrationNo.Trim().ToUpperInvariant();
        var cacheKey = $"{_officialsByRegNoCachePrefix}{normalizedRegNo}";

        if (memoryCache.TryGetValue(cacheKey, out List<Officials>? cachedResults))
        {
            logger.LogInformation("Retrieved officials from cache for registration number: {RegistrationNo}", registrationNo);
            return cachedResults ?? new List<Officials>();
        }

        try
        {
            logger.LogInformation("Fetching officials from API for registration number: {RegistrationNo}", registrationNo);

            var queryParams = new List<string>
            {
                $"resource_id={OfficialResourceId}",
                $"filters[registration_no]={Uri.EscapeDataString(registrationNo)}"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await httpClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode(); 
            
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var officials = JsonSerializer.Deserialize<List<Officials>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Officials>();

            memoryCache.Set(cacheKey, officials, _officialsByRegNoCacheOptions);
            logger.LogInformation("Cached officials for registration number: {RegistrationNo}. Found {Count} officials", 
                registrationNo, officials.Count);

            return officials;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error searching officials with registration number: {RegistrationNo}", registrationNo);
            throw;
        }
    }

    public async Task<List<Officials>> GetOfficialsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new List<Officials>();
        }

        var normalizedSearchTerm = searchTerm.Trim().ToLowerInvariant();
        var cacheKey = $"{_officialsSearchCachePrefix}{normalizedSearchTerm}";

        if (memoryCache.TryGetValue(cacheKey, out List<Officials>? cachedResults))
        {
            logger.LogInformation("Retrieved officials search results from cache for term: {SearchTerm}", searchTerm);
            return cachedResults ?? new List<Officials>();
        }

        try
        {
            logger.LogInformation("Fetching officials search results from API for term: {SearchTerm}", searchTerm);

            var queryParams = new List<string>
            {
                $"resource_id={OfficialResourceId}",
                $"fields[]=person_or_organisation_name",
                $"fields[]=official_position",
                $"fields[]=entry_id",
                $"fields[]=registration_no",
                $"q={Uri.EscapeDataString(searchTerm)}"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await httpClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var officials = JsonSerializer.Deserialize<List<Officials>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Officials>();

            memoryCache.Set(cacheKey, officials, _officialsSearchCacheOptions);
            logger.LogInformation("Cached officials search results for term: {SearchTerm}. Found {Count} officials", 
                searchTerm, officials.Count);

            return officials;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error searching officials with search term: {SearchTerm}", searchTerm);
            throw;
        }
    }
    
    public async Task<List<RelatedCompanyDto>> GetRelatedCompanies(string companyName)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            return new List<RelatedCompanyDto>();
        }
        
        var cacheKey = $"{_relatedCompaniesCachePrefix}{companyName}";

        if (memoryCache.TryGetValue(cacheKey, out List<RelatedCompanyDto>? cachedResults))
        {
            logger.LogInformation("Retrieved related results from cache for term: {Related}", companyName);
            return cachedResults ?? new List<RelatedCompanyDto>();
        }

        try
        {
            logger.LogInformation("Fetching related results from API for term: {Related}", companyName);

            var queryParams = new List<string>
            {
                $"resource_id={OfficialResourceId}",
                $"filters[person_or_organisation_name]={companyName}",
                "limit=0"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await httpClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var related = JsonSerializer.Deserialize<List<RelatedCompanyDto>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<RelatedCompanyDto>();

            memoryCache.Set(cacheKey, related, _officialsSearchCacheOptions);
            logger.LogInformation("Cached officials search results for term: {Related}. Found {Count} related companies", 
                companyName, related.Count);

            return related;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error searching officials with search term: {Related}", companyName);
            throw;
        }
    }
    
    

    public void ClearCache()
    {
        if (memoryCache is MemoryCache mc)
        {
            mc.Clear();
            logger.LogInformation("Officials cache cleared");
        }
    }

    public void ClearOfficialsByRegistrationNoCache(string registrationNo)
    {
        var normalizedRegNo = registrationNo.Trim().ToUpperInvariant();
        var cacheKey = $"{_officialsByRegNoCachePrefix}{normalizedRegNo}";
        memoryCache.Remove(cacheKey);
        logger.LogInformation("Cleared cache for officials with registration number: {RegistrationNo}", registrationNo);
    }

    public void ClearOfficialsSearchCache(string searchTerm)
    {
        var normalizedSearchTerm = searchTerm.Trim().ToLowerInvariant();
        var cacheKey = $"{_officialsSearchCachePrefix}{normalizedSearchTerm}";
        memoryCache.Remove(cacheKey);
        logger.LogInformation("Cleared cache for officials search term: {SearchTerm}", searchTerm);
    }
}