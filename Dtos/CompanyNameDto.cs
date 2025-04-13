using System.ComponentModel.DataAnnotations;

namespace CompanySearchBackend.Dtos
{
    public class CompanyNameDto
    {
        [Key]
        public string RegistrationNo { get; set; }
        public string? Name { get; set; }
        public string? OrganisationStatus { get; set; }
    }
}