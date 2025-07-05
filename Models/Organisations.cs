using System.ComponentModel.DataAnnotations;
using Postgrest.Models;
using Postgrest.Attributes;

namespace CompanySearchBackend.Models;

[Table("organisations")]
public class Organisation : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("organisation_name")]
    public string? OrganisationName { get; set; }

    [Column("registration_no")]
    public string RegistrationNo { get; set; }

    [Column("registration_date")]
    public string? RegistrationDate { get; set; }

    [Column("organisation_status")]
    public string? OrganisationStatus { get; set; }

    [Column("address_seq_no")]
    public int? AddressSeqNo { get; set; }
}