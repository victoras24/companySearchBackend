using System.Text.Json;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CompanySearchBackend.Services;

public class AddressService : IAddressService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AddressService> _logger; 
    private readonly IMemoryCache _memoryCache;
    private readonly string _addressCachePrefix = "address_";

    private const string AddressResourceId = "31d675a2-4335-40ba-b63c-d830d6b5c55d";

    
    private readonly MemoryCacheEntryOptions _addressCacheOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
    };

    public AddressService(HttpClient httpClient, ILogger<AddressService> logger, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<Address?> GetDetailedAddressDataAsync(string addressSeqNo)
    {
        if (string.IsNullOrWhiteSpace(addressSeqNo))
        {
            _logger.LogWarning("Address sequence number is null or empty");
            return null;
        }

        var normalizedSeqNo = addressSeqNo.Trim().ToUpperInvariant();
        var cacheKey = $"{_addressCachePrefix}{normalizedSeqNo}";

        if (_memoryCache.TryGetValue(cacheKey, out Address? cachedAddress))
        {
            _logger.LogInformation("Retrieved address from cache for sequence number: {AddressSeqNo}", addressSeqNo);
            return cachedAddress;
        }

        try
        {
            _logger.LogInformation("Fetching address from API for sequence number: {AddressSeqNo}", addressSeqNo);

            var queryParams = new List<string>
            {
                $"resource_id={AddressResourceId}",
                $"filters[address_seq_no]={Uri.EscapeDataString(addressSeqNo)}"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode(); 
            
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var records = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var addresses = JsonSerializer.Deserialize<List<Address>>(records.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var address = addresses?.FirstOrDefault();

            _memoryCache.Set(cacheKey, address, _addressCacheOptions);
            
            if (address != null)
            {
                _logger.LogInformation("Cached address for sequence number: {AddressSeqNo}", addressSeqNo);
            }
            else
            {
                _logger.LogInformation("No address found for sequence number: {AddressSeqNo} - cached null result", addressSeqNo);
            }

            return address;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting address for sequence number: {AddressSeqNo}", addressSeqNo);
            throw;
        }
    }

    public async Task<Dictionary<string, Address?>> GetMultipleAddressesAsync(IEnumerable<string> addressSeqNos)
    {
        var result = new Dictionary<string, Address?>();
        var uncachedSeqNos = new List<string>();

        foreach (var seqNo in addressSeqNos.Where(s => !string.IsNullOrWhiteSpace(s)))
        {
            var normalizedSeqNo = seqNo.Trim().ToUpperInvariant();
            var cacheKey = $"{_addressCachePrefix}{normalizedSeqNo}";

            if (_memoryCache.TryGetValue(cacheKey, out Address? cachedAddress))
            {
                result[seqNo] = cachedAddress;
                _logger.LogDebug("Retrieved address from cache for sequence number: {AddressSeqNo}", seqNo);
            }
            else
            {
                uncachedSeqNos.Add(seqNo);
            }
        }

        foreach (var seqNo in uncachedSeqNos)
        {
            result[seqNo] = await GetDetailedAddressDataAsync(seqNo);
        }

        _logger.LogInformation("Retrieved {TotalCount} addresses ({CachedCount} from cache, {FetchedCount} from API)", 
            result.Count, result.Count - uncachedSeqNos.Count, uncachedSeqNos.Count);

        return result;
    }

    public void ClearCache()
    {
        if (_memoryCache is MemoryCache mc)
        {
            mc.Clear();
            _logger.LogInformation("Address cache cleared");
        }
    }

    public void ClearAddressCache(string addressSeqNo)
    {
        var normalizedSeqNo = addressSeqNo.Trim().ToUpperInvariant();
        var cacheKey = $"{_addressCachePrefix}{normalizedSeqNo}";
        _memoryCache.Remove(cacheKey);
        _logger.LogInformation("Cleared cache for address with sequence number: {AddressSeqNo}", addressSeqNo);
    }

    public async Task PreloadAddressCacheAsync(IEnumerable<string> addressSeqNos)
    {
        var tasks = addressSeqNos
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(seqNo => GetDetailedAddressDataAsync(seqNo))
            .ToArray();

        await Task.WhenAll(tasks);
        
        _logger.LogInformation("Preloaded {Count} addresses into cache", tasks.Length);
    }
}