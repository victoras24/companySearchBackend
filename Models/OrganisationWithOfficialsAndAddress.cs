using CompanySearchBackend.Dtos;
using Postgrest.Models;
using Postgrest.Attributes;

namespace CompanySearchBackend.Models;
[Table("organisationwithregisteredoffice")]
public class OrganisationWithOfficialsAndAddress : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("name")]
    public string? OrganisationName { get; set; }

    [Column("REGISTRATION_NO")]
    public string RegistrationNo { get; set; }

    [Column("ORGANISATION_TYPE_CODE")]
    public string? OrganisationTypeCode { get; set; }

    [Column("ORGANISATION_TYPE")]
    public string? OrganisationType { get; set; }

    [Column("NAME_STATUS")]
    public string? NameStatus { get; set; }

    [Column("REGISTRATION_DATE")]
    public string? RegistrationDate { get; set; }

    [Column("ORGANISATION_STATUS")]
    public string? OrganisationStatus { get; set; }

    [Column("ORGANISATION_STATUS_DATE")]
    public string? OrganisationStatusDate { get; set; }

    [Column("ADDRESS_SEQ_NO")]
    public int? AddressSeqNo { get; set; }
    
    [Column("STREET")]
    public string? Street { get; set; }
    
    [Column("BUILDING")]
    public string? Building { get; set; }
    
    [Column("TERRITORY")]
    public string? Territory { get; set; }
}