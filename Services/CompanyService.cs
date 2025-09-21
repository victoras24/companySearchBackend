using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CompanySearchBackend.Services;

public class CompanyService : ICompanyService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ICompanyRepository _companyRepository;
    private readonly ILogger<CompanyService> _logger;

    public CompanyService(IMemoryCache memoryCache, ICompanyRepository companyRepository, ILogger<CompanyService> logger)
    {
        _memoryCache = memoryCache;
        _companyRepository = companyRepository;
        _logger = logger;
    }
    public async Task<bool> CacheAllCompaniesAsync()
{
    var pageSize = 1000; 
    var page = 0;
    var hasMoreRecords = true;
    var companiesToCache = new HashSet<Organisation>(); 
    var totalProcessed = 0;

    try
    {
        while (hasMoreRecords)
        {
            var rangeStart = page * pageSize;
            var rangeEnd = (page + 1) * pageSize - 1;
            _logger.LogInformation($"Fetching page {page} with range {rangeStart}-{rangeEnd} (expecting up to {pageSize} records)");
            
            var res = await _companyRepository.GetAllCompanies(page, pageSize);

            if (res == null || res.Count == 0)
            {
                _logger.LogInformation("No more records found, stopping pagination");
                hasMoreRecords = false;
            }
            else
            {
                var duplicatesInBatch = 0;
                var initialCount = companiesToCache.Count;
                
                
                var sampleIds = string.Join(", ", res.Take(3).Select(c => $"ID:{c.Id}"));
                _logger.LogInformation($"Page {page}: Sample IDs from batch: {sampleIds}...");
                
                foreach (var company in res)
                {
                    if (!companiesToCache.Add(company))
                    {
                        duplicatesInBatch++;
                        _logger.LogWarning($"Duplicate company found: ID:{company.Id} - {company.OrganisationName}");
                    }
                }
                
                var actuallyAdded = companiesToCache.Count - initialCount;
                totalProcessed += res.Count;
                
                _logger.LogInformation($"Page {page}: Found {res.Count} companies, added {actuallyAdded} new ones, {duplicatesInBatch} duplicates");
                
                
                if (res.Count < pageSize)
                {
                    _logger.LogInformation($"Last page detected (got {res.Count} < {pageSize})");
                    hasMoreRecords = false;
                }
            }
            
            page++;
        }
        
        var companiesList = companiesToCache.ToList();
        _memoryCache.Set("companies", companiesList, TimeSpan.FromDays(31));
        
        _logger.LogInformation($"Successfully cached {companiesList.Count} unique companies out of {totalProcessed} total records processed");
        
        return true;
    }
    catch (Exception e)
    {
        _logger.LogError(e, "Error occurred while caching companies");
        throw;
    }
}

    public Task<List<Organisation>> GetCachedCompaniesAsync()
    {
        var cachedCompanies = _memoryCache.TryGetValue("companies", out List<Organisation>? cachedOrganisations);
        return cachedCompanies ? Task.FromResult(cachedOrganisations)! : Task.FromResult(new List<Organisation>());
    }
    
    
}