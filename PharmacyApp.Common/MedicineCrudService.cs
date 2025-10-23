using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace PharmacyApp.Common
{
    public class MedicineCrudService : ICrudService<Medicine>
    {
        private List<Medicine> _medicines = new List<Medicine>();

        public event Action<Medicine> MedicineCreated;

        public void Create(Medicine medicine)
        {
            if (medicine == null) throw new ArgumentNullException(nameof(medicine));
            _medicines.Add(medicine);
            MedicineCreated?.Invoke(medicine);
        }

        public Medicine Read(Guid id)
        {
            return _medicines.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Medicine> ReadAll()
        {
            return _medicines.AsReadOnly();
        }

        public void Update(Medicine medicine)
        {
            if (medicine == null) throw new ArgumentNullException(nameof(medicine));
            var existingMedicine = Read(medicine.Id);
            if (existingMedicine != null)
            {
                existingMedicine.Name = medicine.Name;
                existingMedicine.Price = medicine.Price;
                existingMedicine.QuantityInStock = medicine.QuantityInStock;
            }
        }

        public void Remove(Medicine medicine)
        {
            if (medicine == null) throw new ArgumentNullException(nameof(medicine));
            _medicines.RemoveAll(m => m.Id == medicine.Id);
        }

        public void Save(string filePath)
        {
            var json = JsonConvert.SerializeObject(_medicines, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                _medicines = new List<Medicine>();
                return;
            }

            var json = File.ReadAllText(filePath);
            var loaded = JsonConvert.DeserializeObject<List<Medicine>>(json);
            _medicines = loaded ?? new List<Medicine>();
        }
    }
}
