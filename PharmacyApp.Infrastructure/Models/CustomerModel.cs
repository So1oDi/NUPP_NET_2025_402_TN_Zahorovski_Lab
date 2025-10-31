using System.Collections.Generic;

namespace PharmacyApp.Infrastructure.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public ICollection<PrescriptionModel> Prescriptions { get; set; } = new List<PrescriptionModel>();

        public PharmacyModel PharmacyContactFor { get; set; }
    }
}
