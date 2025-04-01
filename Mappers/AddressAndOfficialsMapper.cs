using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanySearchBackend.Dtos;
using CompanySearchBackend.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CompanySearchBackend.Mappers
{
    public static class AddressAndOfficialsMapper
    {
        public static AddressAndOfficialsDto ToAddressAndOfficials(AddressAndOfficialsDto addressAndOfficials)
        {
            return new AddressAndOfficialsDto
            {
                Name = addressAndOfficials.Name,
                OrganisationStatus = addressAndOfficials.OrganisationStatus,
                RegistrationDate = addressAndOfficials.RegistrationDate,
                Street = addressAndOfficials.Street,
                Building = addressAndOfficials.Building,
                Territory = addressAndOfficials.Territory,
                Officials = addressAndOfficials.Officials
            };
        }
    }
}