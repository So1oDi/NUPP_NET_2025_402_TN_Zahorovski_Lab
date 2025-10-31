using System.Collections.Generic;

namespace PharmacyApp.Infrastructure.Models
{
    public class MedicineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityInStock { get; set; }

        public ICollection<PrescriptionModel> Prescriptions { get; set; } = new List<PrescriptionModel>();
    }
}
