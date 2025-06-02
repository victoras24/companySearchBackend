using System.Text.Json;
using CompanySearchBackend.Interfaces;

namespace CompanySearchBackend.Services;

public class CompanyService : ICompanyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    private const string OrganisationResourceId = "b48bf3b6-51f2-4368-8eaa-63d61836aaa9";


    public CompanyService(HttpClient httpClient, ILogger<CompanyService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }


    public async Task<List<Company>> SearchCompaniesAsync(string searchTerm)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"resource_id={OrganisationResourceId}",
                $"q={searchTerm}"
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
            });

            return companies ?? new List<Company>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching organisations with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    public async Task<Company> GetDetailedCompanyDataAsync(string registrationNo, string entryId)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"resource_id={OrganisationResourceId}",
                $"filters[entry_id]={entryId}",
                $"filters[registration_no]={registrationNo}"
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
            });

            return companies?.FirstOrDefault();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching organisations with registrationNo: {registrationNo}, entryId: {entryId}", registrationNo, entryId);
            throw;
        }
    }

}