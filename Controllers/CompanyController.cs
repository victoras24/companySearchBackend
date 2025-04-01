using CompanySearchBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySearchBackend.Controllers
{
    [ApiController]
    [Route("api/company/")]

    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {

            var companyName = await _companyRepository.GetCompanyAsync(name);

            if (companyName  == null)
            {
                return NotFound();
            }

            return Ok(companyName);

        }

        [HttpGet("{registrationNo}/detailed")]
        public async Task<IActionResult> GetAddressAndOfficialsByRegNo([FromRoute] int registrationNo)
        {
            var detailedCompany = await _companyRepository.GetAddressAndOfficials(registrationNo);

            if (detailedCompany == null)
            {
                return NotFound();
            }

            return Ok(detailedCompany);
        }
    }

}