using CompanySearchBackend.Dtos;
using Postgrest.Models;
using Postgrest.Attributes;

namespace CompanySearchBackend.Models;
[Table("organisationwithofficials")]
public class OrganisationWithOfficialsAndAddress : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? OrganisationName { get; set; }

    [Column("registrationno")]
    public string RegistrationNo { get; set; }

    [Column("organisationtypecode")]
    public string? OrganisationTypeCode { get; set; }

    [Column("organisationtype")]
    public string? OrganisationType { get; set; }

    [Column("namestatus")]
    public string? NameStatus { get; set; }

    [Column("registrationdate")]
    public string? RegistrationDate { get; set; }

    [Column("organisationstatus")]
    public string? OrganisationStatus { get; set; }

    [Column("organisationstatusdate")]
    public string? OrganisationStatusDate { get; set; }

    [Column("ADDRESS_SEQ_NO")]
    public int? AddressSeqNo { get; set; }
    
    [Column("STREET")]
    public string? Street { get; set; }
    
    [Column("BUILDING")]
    public string? Building { get; set; }
    
    [Column("TERRITORY")]
    public string? Territory { get; set; }
    
    [Column("OFFICIALS")]
    public List<OfficialsDto> Officials { get; set; }
}