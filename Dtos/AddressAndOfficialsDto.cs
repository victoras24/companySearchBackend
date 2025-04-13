using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public string Officials { get; set; }
    }

    public class Official
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string RegistrationNo { get; set; }
    }
}