using System.Text.Json.Serialization;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CompanySearchBackend.Models;

[Table("officials")]
public class Officials : BaseModel
{
        [Column("organisation_name")]
        public string OrganisationName { get; set; }

        [Column("registration_no")]
        public string RegistrationNo { get; set; }

        [Column("organisation_type_code")]
        public string OrganisationTypeCode { get; set; }

        [Column("organisation_type")]
        public string OrganisationType { get; set; }

        [Column("person_or_organisation_name")]
        public string PersonOrOrganisationName { get; set; }

        [Column("official_position")]
        public string OfficialPosition { get; set; }
}