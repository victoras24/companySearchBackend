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

            if (companyName  is null)
            {
                return NotFound();
            }

            return Ok(companyName);

        }

        [HttpGet("{registrationNo}/detailed")]
        public async Task<IActionResult> GetDetailedCompanyData([FromRoute] string registrationNo)
        {
            var detailedCompany = await companyRepository.GetCompanyDetailed(registrationNo);
            
            if (detailedCompany is null)
            {
                return NotFound();
            }

            return Ok(detailedCompany.Result.Models);
        }

        [HttpGet("{addressSeqNo}/address")]
        public async Task<IActionResult> GetCompanyAddress(int addressSeqNo)
        {
            var companyAddress = await companyRepository.GetCompanyAddress(addressSeqNo);
            if (companyAddress is null)
            {
                return NotFound();
            }
            return Ok(companyAddress.Model);
        }

        [HttpGet("{registrationNo}/key-people")]
        public async Task<IActionResult> GetCompanyKeyPeople(string registrationNo)
        {
            var companyKeyPeople = await companyRepository.GetCompanyKeyPeople(registrationNo);

            if (companyKeyPeople is null)
            {
                return NotFound();
            }
            
            return Ok(companyKeyPeople);
        }

        [HttpGet("{companyName}/related")]
        public async Task<IActionResult> GetCompanyRelatedCompany(string companyName)
        {
            var related = await companyRepository.GetCompanyRelated(companyName);

            if (related is null)
            {
                return NotFound();
            }
            
            return Ok(related);
        }
        
        [HttpGet("{name}/active")]

        public async Task<IActionResult> GetActiveOrganisations([FromRoute] string name)
        {
            var activeCompany = await companyRepository.GetActiveOrganisation(name);
            
            if (activeCompany is null)
            {
                return NotFound();
            }
            
            return Ok(activeCompany);
        }

        [HttpGet("{name}/inactive")]
        public async Task<IActionResult> GetInactiveOrganisations([FromRoute] string name)
        {
            var inactiveCompany = await companyRepository.GetInactiveOrganisation(name);
            
            if (inactiveCompany is null)
            {
                return NotFound();
            }
            
            return Ok(inactiveCompany);
        }
        
        [HttpGet("officials/{searchTem}")]
        public async Task<IActionResult> GetOfficials([FromRoute] string searchTem)
        {
            var officials = await companyRepository.GetOfficialsAsync(searchTem);
            
            if (officials is null)
            {
                return NotFound();
            }
            
            return Ok(officials);
        }
        
    }

}