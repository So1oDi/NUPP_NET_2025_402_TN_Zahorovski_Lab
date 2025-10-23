using System;
using PharmacyApp.Common;

namespace PharmacyApp.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new MedicineCrudService();

            var med1 = new Medicine("Парацетамол", 10.5, 50);
            var med2 = new Medicine("Аспірін", 8.0, 30);
            service.Create(med1);
            service.Create(med2);

            Console.WriteLine("Ліки додано до БД:");
            foreach (var medicine in service.ReadAll())
                Console.WriteLine($"Id: {medicine.Id} | Назва: {medicine.Name}, Ціна: {medicine.Price}, Кількість: {medicine.QuantityInStock}");

            med1.Price = 12.0;
            service.Update(med1);

            Console.WriteLine("\nПісля оновлення:");
            foreach (var medicine in service.ReadAll())
                Console.WriteLine($"Id: {medicine.Id} | Назва: {medicine.Name}, Ціна: {medicine.Price}, Кількість: {medicine.QuantityInStock}");

            service.Save("medicines.json");
            Console.WriteLine("\nЛіки збережено в 'medicines.json'.");
        }
    }
}
