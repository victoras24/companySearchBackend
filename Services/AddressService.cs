using System.Text.Json;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;

namespace CompanySearchBackend.Services;

public class AddressService : IAddressService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CompanyService> _logger;
    private const string AddressResourceId = "31d675a2-4335-40ba-b63c-d830d6b5c55d";

    public AddressService(HttpClient httpClient, ILogger<CompanyService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    public async Task<Address> GetDetailedAddressDataAsync(string addressSeqNo)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"resource_id={AddressResourceId}",
                $"filters[address_seq_no]={addressSeqNo}"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var records = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var addresses = JsonSerializer.Deserialize<List<Address>>(records.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return addresses?.FirstOrDefault();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error getting address for seqNo {addressSeqNo}: {e.Message}");
            throw;
        }
    }

}