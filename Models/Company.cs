using System.Text.Json.Serialization;
using Postgrest.Models;

public class Company : BaseModel
{
    [JsonPropertyName("organisation_name")]
    public string OrganisationName { get; set; }
    
    [JsonPropertyName("registration_no")]
    public string RegistrationNo { get; set; }
    
    [JsonPropertyName("organisation_type_code")]
    public string OrganisationTypeCode { get; set; }
    
    [JsonPropertyName("organisation_type")]
    public string OrganisationType { get; set; }
    
    [JsonPropertyName("organisation_sub_type")]
    public string OrganisationSubType { get; set; }
    
    [JsonPropertyName("name_status_code")]
    public string NameStatusCode { get; set; }
    
    [JsonPropertyName("name_status")]
    public string NameStatus { get; set; }
    
    [JsonPropertyName("registration_date")]
    public string RegistrationDate { get; set; }
    
    [JsonPropertyName("organisation_status")]
    public string OrganisationStatus { get; set; }
    
    [JsonPropertyName("organisation_status_date")]
    public string OrganisationStatusDate { get; set; }
    
    [JsonPropertyName("address_seq_no")]
    public string AddressSeqNo { get; set; }
    
    [JsonPropertyName("entry_id")]
    public string EntryId { get; set; }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public ResultData Result { get; set; }
}

public class ResultData
{
    public List<Company> Records { get; set; }
}