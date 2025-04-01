using System;
using System.Collections.Generic;

namespace CompanySearchBackend.Models;

public partial class OrganisationOfficial
{
    public string? OrganisationName { get; set; }

    public string? RegistrationNo { get; set; }

    public string? OrganisationTypeCode { get; set; }

    public string? OrganisationType { get; set; }

    public string? PersonOrOrganisationName { get; set; }

    public string? OfficialPosition { get; set; }
}
