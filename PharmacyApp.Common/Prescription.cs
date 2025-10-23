using System;

namespace PharmacyApp.Common
{
    public class Prescription : Medicine
    {
        public string DoctorName { get; set; }
        public DateTime PrescriptionDate { get; set; }

        public Prescription(string name, double price, int quantityInStock, string doctorName)
            : base(name, price, quantityInStock)
        {
            DoctorName = doctorName;
            PrescriptionDate = DateTime.Now;
        }
    }
}
