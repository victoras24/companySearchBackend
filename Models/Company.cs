namespace CompanySearchBackend.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public int RegistrationNumber { get; set; }
}
