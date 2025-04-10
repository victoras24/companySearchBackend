using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CompanySearchBackend.Dtos;

public class OrganisationOfficialDto
{
    [Key]
    public string RegistrationNo { get; set; }
    public string? OrganisationName { get; set; }
    public string? Officials { get; set; }
}