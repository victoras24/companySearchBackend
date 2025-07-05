using Postgrest.Attributes;
using Postgrest.Models;

namespace CompanySearchBackend.Models;

[Table("address")]
public class Address : BaseModel
{
    [PrimaryKey("address_seq_no")]
    public int AddressSeqNo { get; set; }
    
    [Column("street")]
    public string Street { get; set; }
    
    [Column("building")]
    public string Building { get; set; }
    
    [Column("territory")]
    public string Territory { get; set; }
}