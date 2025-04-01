using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanySearchBackend.Dtos
{
    public class AddressAndOfficialsDto
    {
        public string? Name { get; set; }

        public string? OrganisationStatus { get; set; }

        public DateOnly? RegistrationDate { get; set; }

        public string? Street { get; set; }

        public string? Building { get; set; }

        public string? Territory { get; set; }

        public string? Officials { get; set; }
    }
}