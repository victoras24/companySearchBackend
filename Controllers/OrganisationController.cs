using CompanySearchBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySearchBackend.Controllers;

[ApiController]
[Route("api/organisation")]
public class OrganisationController(ILogger<OrganisationController> logger, ICompanyService companyService, IAddressService addressService, IOfficialService officialService)
    : ControllerBase
{

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

    [HttpGet("{registrationNo}/{entryId}/detailed")]
    public async Task<IActionResult> GetDetailedOrganisationData([FromRoute] string registrationNo, [FromRoute] string entryId)
    {
        try
        {
            var result = await companyService.GetDetailedCompanyDataAsync(registrationNo, entryId);
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
    
}