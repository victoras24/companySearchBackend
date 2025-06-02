using Stripe;
using Address = CompanySearchBackend.Models.Address;

namespace CompanySearchBackend.Interfaces;

public interface IAddressService
{
    Task<Address> GetDetailedAddressDataAsync(string addressSeqNo);
}