using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanySearchBackend.Controllers;

[ApiController]
[Route("api/cache/")]

public class CacheController(ICompanyService companyService) :  ControllerBase
{
        [HttpPost("create")]
        public async Task<bool> CreateCacheCompany()
        {
                var isCached = await companyService.CacheAllCompaniesAsync();
                return isCached;
        }

        [HttpGet("get")]
        public async Task<List<Organisation>> GetCachedCompaniesAsync()
        {
                var cachedCompanies = await companyService.GetCachedCompaniesAsync();
                return cachedCompanies;
        }
}