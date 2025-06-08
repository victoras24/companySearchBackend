using System.Text.Json;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;

namespace CompanySearchBackend.Services;

public class CompanyService : ICompanyService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    private const string OrganisationResourceId = "b48bf3b6-51f2-4368-8eaa-63d61836aaa9";
    private const string AddressResourceId = "31d675a2-4335-40ba-b63c-d830d6b5c55d";

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

    public async Task<CompanyAndAddress> GetDetailedCompanyDataAsync(string addressSeqNo, string entryId, string registrationNo)
    {
        try
        {
            var queryParams = new List<string>
            {
                $"resource_id[pop]={OrganisationResourceId}",
                $"resource_id[size]={AddressResourceId}",
                $"filters[pop][ADDRESS_SEQ_NO]={addressSeqNo}",
                $"filters[pop][entry_id]={entryId}",
                $"filters[pop][registration_no]={registrationNo}",
                $"join[pop]=ADDRESS_SEQ_NO",
                $"join[size]=ADDRESS_SEQ_NO",
            };
            
            var url = $"https://www.data.gov.cy/api/action/datastore/search.json?{string.Join("&", queryParams)}";
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var recordsElement = doc.RootElement
                .GetProperty("result")
                .GetProperty("records");

            var companies = JsonSerializer.Deserialize<List<CompanyAndAddress>>(recordsElement.GetRawText(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return companies?.FirstOrDefault();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error searching organisations with addressSeqNo: {addressSeqNo}, entryId: {entryId}", addressSeqNo, entryId);
            throw;
        }
    }

}