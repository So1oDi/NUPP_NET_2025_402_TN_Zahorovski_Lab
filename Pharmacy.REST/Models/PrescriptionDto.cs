namespace Pharmacy.REST.Models;

public class PrescriptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int QuantityInStock { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime PrescriptionDate { get; set; }
    public int? CustomerId { get; set; }
}

public class CreatePrescriptionDto
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int QuantityInStock { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int? CustomerId { get; set; }
}

public class UpdatePrescriptionDto
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int QuantityInStock { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int? CustomerId { get; set; }
}