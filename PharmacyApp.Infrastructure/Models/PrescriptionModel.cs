using System;

namespace PharmacyApp.Infrastructure.Models
{
    public class PrescriptionModel : MedicineModel
    {
        public string DoctorName { get; set; }
        public DateTime PrescriptionDate { get; set; }

        public int? CustomerId { get; set; }
        public CustomerModel Customer { get; set; }
    }
}
