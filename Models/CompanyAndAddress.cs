using System.Text.Json.Serialization;
using Postgrest.Models;

namespace CompanySearchBackend.Models;

public class CompanyAndAddress : BaseModel
{

    [JsonPropertyName("organisation_name")]
    public string OrganisationName { get; set; }

    [JsonPropertyName("registration_no")]
    public string RegistrationNo { get; set; }

    [JsonPropertyName("registration_date")]
    public string RegistrationDate { get; set; }

    [JsonPropertyName("organisation_status")]
    public string OrganisationStatus { get; set; }

    [JsonPropertyName("address_seq_no")]
    public string AddressSeqNo { get; set; }

    [JsonPropertyName("entry_id")]
    public string EntryId { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("building")]
    public string Building { get; set; }

    [JsonPropertyName("territory")]
    public string Territory { get; set; }

}