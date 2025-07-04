using CompanySearchBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CompanySearchBackend.Controllers;

[ApiController]
[Route("api/organisation")]

public class OrganisationController(ILogger<OrganisationController> logger, ICompanyService companyService, IAddressService addressService, IOfficialService officialService, IMemoryCache memoryCache)
    : ControllerBase
{
    private readonly string _searchCacheKeyPrefix = "companySearch_";
    private readonly string _detailsCacheKeyPrefix = "companyDetails_";
    private readonly ILogger _logger = logger;

    [HttpGet("{searchTerm}")]
    public async Task<ActionResult<List<Company>>> SearchOrganisations(string searchTerm)
    {
        
        try
        {
            var result = await companyService.SearchCompaniesAsync(searchTerm);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("{registrationNo}/detailed")]
    public async Task<IActionResult> GetDetailedOrganisationData(  [FromRoute] string registrationNo)
    {
        try
        {
            var result = await companyService.GetDetailedCompanyDataAsync(registrationNo);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("{addressSeqNo}/address")]
    public async Task<IActionResult> GetOrganisationAddress([FromRoute] string addressSeqNo)
    {
        try
        {
            var result = await addressService.GetDetailedAddressDataAsync(addressSeqNo);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("{registrationNo}/officials")]
    public async Task<IActionResult> GetDetailedOfficialsDataByRegistrationNo([FromRoute] string registrationNo)
    {
        try
        {
            var result = await officialService.GetOfficialsByRegistrationNoAsync(registrationNo);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpGet("officials/{searchTerm}")]
    public async Task<IActionResult> GetDetailedOfficialsDataBySearchTerm([FromRoute] string searchTerm)
    {
        try
        {
            var result = await officialService.GetOfficialsAsync(searchTerm);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet("related/{companyName}")]
    public async Task<IActionResult> GetRelatedOrganisationData([FromRoute] string companyName)
    {
        try
        {
            var result = await officialService.GetRelatedCompanies(companyName);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpDelete("clear")]
    public IActionResult ClearCache()
    {
        if (memoryCache is MemoryCache mc)
        {
            mc.Clear();
            _logger.LogInformation("Cleared cache");

        }
        
        return Ok(new { message = "Cache cleared successfully" });
    }

    [HttpDelete("search/{searchTerm}")]
    public IActionResult ClearSearchCache(string searchTerm)
    {
        var normalizedSearchTerm = searchTerm.Trim().ToLowerInvariant();
        var cacheKey = $"{_searchCacheKeyPrefix}{normalizedSearchTerm}";
        memoryCache.Remove(cacheKey);
        _logger.LogInformation("Cleared cache for search term: {SearchTerm}", searchTerm);

        return Ok(new { message = $"Search cache cleared for: {searchTerm}" });
    }

    [HttpDelete("company/{registrationNo}")]
    public IActionResult ClearCompanyCache(string registrationNo)
    {
        var cacheKey = $"{_detailsCacheKeyPrefix}{registrationNo}";
        memoryCache.Remove(cacheKey);
        _logger.LogInformation("Cleared cache for company with registration: {RegistrationNo}", registrationNo);
        return Ok(new { message = $"Company cache cleared for: {registrationNo}" });
    }
}