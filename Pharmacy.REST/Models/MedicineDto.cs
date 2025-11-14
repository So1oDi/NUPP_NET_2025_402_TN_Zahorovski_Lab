namespace Pharmacy.REST.Models;

public class MedicineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int QuantityInStock { get; set; }
}

public class CreateMedicineDto
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int QuantityInStock { get; set; }
}

public class UpdateMedicineDto
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int QuantityInStock { get; set; }
}