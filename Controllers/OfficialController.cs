using CompanySearchBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySearchBackend.Controllers;
[ApiController]
[Route("api/officials/")]
public class OfficialController(IOfficialRepository officialRepository) : ControllerBase
{
    private readonly IOfficialRepository _officialRepository = officialRepository;

    [HttpGet("{name}")]
    public async Task<IActionResult> GetOfficial([FromRoute] string name)
    {
        var official = await _officialRepository.GetOfficials(name);

        if (official is null)
        {
            return NotFound();
        }
        
        return Ok(official);
    }

    [HttpGet("{registrationNo}/detailed")]

    public async Task<IActionResult> GetOfficialsByRegistrationNo([FromRoute] string registrationNo)
    {
        var official = await _officialRepository.GetOrganisationOfficials(registrationNo);

        if (official == null)
        {
            return NotFound();
        }
        
        return Ok(official);
    }
}