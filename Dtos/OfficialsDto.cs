using System.Text.Json.Serialization;

namespace CompanySearchBackend.Dtos;

public class OfficialsDto
{
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Position")]
    public string? Position { get; set; }

    [JsonPropertyName("RegistrationNo")]
    public string? RegistrationNo { get; set; }
    
}