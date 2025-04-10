using CompanySearchBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySearchBackend.Controllers;
[ApiController]
[Route("api/officials/")]
public class OfficialController(IOfficialRepository officialRepository) : ControllerBase
{
    private readonly IOfficialRepository _officialRepository = officialRepository;

    [HttpGet("{name}")]

        public async Task<IActionResult> GetOfficialByName([FromRoute] string name)
        {
            var officialName = await _officialRepository.GetOfficialAsync(name);
            
            if (officialName  == null)
            {
                return NotFound();
            }
            
            return Ok(officialName);
        }
}