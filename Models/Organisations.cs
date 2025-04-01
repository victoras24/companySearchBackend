using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanySearchBackend.Models;

public class Organisations
{
    [Key]
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? RegistrationNo { get; set; }

    public string? OrganisationTypeCode { get; set; }

    public string? OrganisationType { get; set; }

    public string? OrganisationSubType { get; set; }

    public string? NameStatusCode { get; set; }

    public string? NameStatus { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public string? OrganisationStatus { get; set; }

    public DateOnly? OrganisationStatusDate { get; set; }

    public int? AddressSeqNumber { get; set; }

    public string? ExtraColumn { get; set; }

}
