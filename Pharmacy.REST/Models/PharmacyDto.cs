namespace Pharmacy.REST.Models;

public class PharmacyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int? ContactCustomerId { get; set; }
}

public class CreatePharmacyDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int? ContactCustomerId { get; set; }
}

public class UpdatePharmacyDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int? ContactCustomerId { get; set; }
}