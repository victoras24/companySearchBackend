using System.Text.Json.Serialization;
using Postgrest.Models;

namespace CompanySearchBackend.Models;

public class Officials : BaseModel
{
        [JsonPropertyName("organisation_name")]
        public string OrganisationName { get; set; }

        [JsonPropertyName("registration_no")]
        public string RegistrationNo { get; set; }

        [JsonPropertyName("organisation_type_code")]
        public string OrganisationTypeCode { get; set; }

        [JsonPropertyName("organisation_type")]
        public string OrganisationType { get; set; }

        [JsonPropertyName("person_or_organisation_name")]
        public string PersonOrOrganisationName { get; set; }

        [JsonPropertyName("official_position")]
        public string OfficialPosition { get; set; }

        [JsonPropertyName("entry_id")]
        public string EntryId { get; set; }
}