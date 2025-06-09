using System.Text.Json.Serialization;

namespace CompanySearchBackend.Dtos;

public class RelatedCompanyDto 
{
    [JsonPropertyName("person_or_organisation_name")]
    public string CompanyName { get; set; }
    [JsonPropertyName("organisation_name")]
    public string RelatedCompany { get; set; }
    [JsonPropertyName("official_position")]
    public string OfficialPosition { get; set; }
    [JsonPropertyName("registration_no")]
    public string RegistrationNo { get; set; }
}