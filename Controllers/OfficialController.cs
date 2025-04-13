using CompanySearchBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySearchBackend.Controllers;
[ApiController]
[Route("api/officials/")]
public class OfficialController(IOfficialRepository officialRepository) : ControllerBase
{
    private readonly IOfficialRepository _officialRepository = officialRepository;

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