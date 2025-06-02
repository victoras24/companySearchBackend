using System.Text.Json.Serialization;
using Postgrest.Models;

namespace CompanySearchBackend.Models;

public class Address : BaseModel
{
    [JsonPropertyName("address_seq_no")]
    public string AddressSeqNo { get; set; }
    
    [JsonPropertyName("street")]
    public string Street { get; set; }
    
    [JsonPropertyName("building")]
    public string Building { get; set; }
    
    [JsonPropertyName("territory")]
    public string Territory { get; set; }
    
    [JsonPropertyName("entry_id")]
    public string EntryId { get; set; }
}