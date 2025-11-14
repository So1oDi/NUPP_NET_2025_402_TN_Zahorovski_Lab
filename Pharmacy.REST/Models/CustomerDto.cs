namespace Pharmacy.REST.Models;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class CreateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class UpdateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}