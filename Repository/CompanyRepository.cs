using CompanySearchBackend.Dtos;
using CompanySearchBackend.Interfaces;
using CompanySearchBackend.Mappers;
using CompanySearchBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanySearchBackend.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyDbContext _companyDb;

        public CompanyRepository(CompanyDbContext companyDb)
        {
            _companyDb = companyDb;
        }

        public async Task<List<CompanyNameDto>> GetCompanyAsync(string name)
        {
            var query = $@"
        SELECT TOP 10 
            o.Registration_No as RegistrationNo, o.Name, o.Organisation_Status as OrganisationStatus
        FROM organisations o
        WHERE o.Name LIKE '%{name}%'";
            var res = await _companyDb.Organisations.FromSqlRaw(query).ToListAsync();

            return res;

        }

        public async Task<AddressAndOfficialsDto> GetAddressAndOfficials(int registrationNo)
        {
            var query = $@"
            SELECT
                o.id, 
                o.Name, 
                o.Name_Status AS NameStatus, 
                o.Organisation_Status AS OrganisationStatus, 
                o.Organisation_Status_Date AS OrganisationStatusDate,
                o.Organisation_Type AS OrganisationType, 
                o.Registration_Date AS RegistrationDate, 
                o.Registration_No AS RegistrationNo,

            -- Address details
                ro.ADDRESS_SEQ_NO,
                ro.Street,
                ro.Building,
                ro.Territory,
                
            -- Aggregate officials in a single row
                STRING_AGG(oo.Person_Or_Organisation_Name + ' (' + oo.OFFICIAL_POSITION + ')', ', ') AS Officials
                FROM organisations o
                LEFT JOIN registered_office ro ON o.Address_Seq_Number = ro.ADDRESS_SEQ_NO
                LEFT JOIN organisation_officials oo ON o.Registration_No = oo.Registration_No
                WHERE o.Registration_No = '{registrationNo}'
                GROUP BY o.id, o.Name, o.Name_Status, o.Organisation_Status, o.Organisation_Status_Date,
                o.Organisation_Type, o.Registration_Date, o.Registration_No,
                ro.Street, ro.Building, ro.Territory, ro.ADDRESS_SEQ_NO
            ";

            var result = await _companyDb.RegisteredOffices.FromSqlRaw(query).FirstOrDefaultAsync();

            return result;
        }

    }
}