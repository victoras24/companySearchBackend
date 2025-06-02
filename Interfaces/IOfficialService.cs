using CompanySearchBackend.Dtos;
using CompanySearchBackend.Models;

namespace CompanySearchBackend.Interfaces;

public interface IOfficialService
{
    Task<List<Officials>> GetOfficialsAsync(string registrationNo);
}