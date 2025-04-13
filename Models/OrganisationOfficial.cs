using System;
using System.Collections.Generic;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CompanySearchBackend.Models;

[Table("organisation_officials")]
public partial class OrganisationOfficial : BaseModel
{
    [Column("ORGANISATION_NAME")]
    public string? OrganisationName { get; set; }
    
    [Column("REGISTRATION_NO")]
    public string? RegistrationNo { get; set; }

    [Column("PERSON_OR_ORGANISATION_NAME")]
    public string? PersonOrOrganisationName { get; set; }

    [Column("OFFICIAL_POSITION")]
    public string? OfficialPosition { get; set; }
}
