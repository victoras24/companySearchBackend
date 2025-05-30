using CompanySearchBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySearchBackend.Controllers
{
    [ApiController]
    [Route("api/company/")]

    public class CompanyController(ICompanyRepository companyRepository) : ControllerBase
    {
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {

            var companyName = await companyRepository.GetCompanyAsync(name);

            if (companyName  == null)
            {
                return NotFound();
            }

            return Ok(companyName);

        }

        [HttpGet("{registrationNo}/detailed")]
        public async Task<IActionResult> GetAddressAndOfficialsByRegNo([FromRoute] string registrationNo)
        {
            var detailedCompany = await companyRepository.GetAddressAndOfficials(registrationNo);
            
            if (detailedCompany == null)
            {
                return NotFound();
            }

            return Ok(detailedCompany);
        }

        [HttpGet("{name}/active")]

        public async Task<IActionResult> GetActiveOrganisations([FromRoute] string name)
        {
            var activeCompany = await companyRepository.GetActiveOrganisation(name);
            
            if (activeCompany == null)
            {
                return NotFound();
            }
            
            return Ok(activeCompany);
        }

        [HttpGet("{name}/inactive")]
        public async Task<IActionResult> GetInactiveOrganisations([FromRoute] string name)
        {
            var inactiveCompany = await companyRepository.GetInactiveOrganisation(name);
            
            if (inactiveCompany == null)
            {
                return NotFound();
            }
            
            return Ok(inactiveCompany);
        }
        
    }

}