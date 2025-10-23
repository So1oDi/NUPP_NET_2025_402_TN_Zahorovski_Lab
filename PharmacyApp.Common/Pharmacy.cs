using System;
using System.Collections.Generic;

namespace PharmacyApp.Common
{
    public class Pharmacy
    {
        public static int TotalMedicinesCreated;
        static Pharmacy()
        {
            TotalMedicinesCreated = 0;
        }

        public List<Medicine> Medicines { get; set; }

        public Pharmacy()
        {
            Medicines = new List<Medicine>();
        }

        public void AddMedicine(Medicine medicine)
        {
            Medicines.Add(medicine);
            TotalMedicinesCreated++;
        }
    }
}
