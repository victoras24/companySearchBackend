using System.Text.Json;
using CompanySearchBackend.Dtos;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CompanySearchBackend.Services;

public class OfficialService : IOfficialService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OfficialService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly string _officialsByRegNoCachePrefix = "officialsByRegNo_";
    private readonly string _officialsSearchCachePrefix = "officialsSearch_";

    private const string OfficialResourceId = "a1deb65d-102b-4e8e-9b9c-5b357d719477";

    
    private readonly MemoryCacheEntryOptions _officialsByRegNoCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)

    };

    private readonly MemoryCacheEntryOptions _officialsSearchCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    public OfficialService(HttpClient httpClient, ILogger<OfficialService> logger, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<List<Officials>> GetOfficialsByRegistrationNoAsync(string registrationNo)
    {
        if (string.IsNullOrWhiteSpace(registrationNo))
        {
            _logger.LogWarning("Registration number is null or empty");
            return new List<Officials>();
        }

        var normalizedRegNo = registrationNo.Trim().ToUpperInvariant();
        var cacheKey = $"{_officialsByRegNoCachePrefix}{normalizedRegNo}";

        if (_memoryCache.TryGetValue(cacheKey, out List<Officials>? cachedResults))
        {
            _logger.LogInformation("Retrieved officials from cache for registration number: {RegistrationNo}", registrationNo);
            return cachedResults ?? new List<Officials>();
        }

        try
        {
            _logger.LogInformation("Fetching officials from API for registration number: {RegistrationNo}", registrationNo);

            var queryParams = new List<string>
            {
                $"resource_id={OfficialResourceId}",
                $"filters[registration_no]={Uri.EscapeDataString(registrationNo)}"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            
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

            _memoryCache.Set(cacheKey, officials, _officialsByRegNoCacheOptions);
            _logger.LogInformation("Cached officials for registration number: {RegistrationNo}. Found {Count} officials", 
                registrationNo, officials.Count);

            return officials;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching officials with registration number: {RegistrationNo}", registrationNo);
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

        if (_memoryCache.TryGetValue(cacheKey, out List<Officials>? cachedResults))
        {
            _logger.LogInformation("Retrieved officials search results from cache for term: {SearchTerm}", searchTerm);
            return cachedResults ?? new List<Officials>();
        }

        try
        {
            _logger.LogInformation("Fetching officials search results from API for term: {SearchTerm}", searchTerm);

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
            var response = await _httpClient.GetAsync(url);
            
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

            _memoryCache.Set(cacheKey, officials, _officialsSearchCacheOptions);
            _logger.LogInformation("Cached officials search results for term: {SearchTerm}. Found {Count} officials", 
                searchTerm, officials.Count);

            return officials;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching officials with search term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public void ClearCache()
    {
        if (_memoryCache is MemoryCache mc)
        {
            mc.Clear();
            _logger.LogInformation("Officials cache cleared");
        }
    }

    public void ClearOfficialsByRegistrationNoCache(string registrationNo)
    {
        var normalizedRegNo = registrationNo.Trim().ToUpperInvariant();
        var cacheKey = $"{_officialsByRegNoCachePrefix}{normalizedRegNo}";
        _memoryCache.Remove(cacheKey);
        _logger.LogInformation("Cleared cache for officials with registration number: {RegistrationNo}", registrationNo);
    }

    public void ClearOfficialsSearchCache(string searchTerm)
    {
        var normalizedSearchTerm = searchTerm.Trim().ToLowerInvariant();
        var cacheKey = $"{_officialsSearchCachePrefix}{normalizedSearchTerm}";
        _memoryCache.Remove(cacheKey);
        _logger.LogInformation("Cleared cache for officials search term: {SearchTerm}", searchTerm);
    }
}