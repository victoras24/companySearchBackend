using System.Text.Json;
using CompanySearchBackend.Dtos;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;

namespace CompanySearchBackend.Services;

public class OfficialService : IOfficialService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OfficialService> _logger;
    private const string OfficialResourceId = "a1deb65d-102b-4e8e-9b9c-5b357d719477";

    public OfficialService(HttpClient httpClient, ILogger<OfficialService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    public async Task<List<Officials>> GetOfficialsByRegistrationNoAsync(string registrationNo)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"resource_id={OfficialResourceId}",
                $"filters[registration_no]={registrationNo}"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var officials = JsonSerializer.Deserialize<List<Officials>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return officials ?? new List<Officials>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching organisations with registrationNo: {registrationNo}, registrationNo");
            throw;
        }
    }

    public async Task<List<Officials>> GetOfficialsAsync(string searchTerm)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"resource_id={OfficialResourceId}",
                $"fields[]=person_or_organisation_name",
                $"&fields[]=official_position",
                $"&fields[]=entry_id",
                $"&fields[]=registration_no",
                $"q={searchTerm}"
            };

            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var officials = JsonSerializer.Deserialize<List<Officials>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return officials ?? new List<Officials>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching organisations with registrationNo: {registrationNo}, registrationNo");
            throw;
        }
    }
}