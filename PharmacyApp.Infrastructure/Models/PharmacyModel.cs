namespace PharmacyApp.Infrastructure.Models
{
    public class PharmacyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MedicineModel> Medicines { get; set; } = new List<MedicineModel>();

        public int? ContactCustomerId { get; set; }
        public CustomerModel ContactCustomer { get; set; }
				public string Address { get; set; }
    }
}
