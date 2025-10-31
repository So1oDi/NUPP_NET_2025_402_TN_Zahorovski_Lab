// --- SQLite і MongoDB паралельно ---
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmacyApp.Infrastructure;
using PharmacyApp.Infrastructure.Models;
using PharmacyApp.Infrastructure.Repositories;
using PharmacyApp.Infrastructure.Services;
using PharmacyApp.Nosql.Models;
using PharmacyApp.Nosql.Repositories;

namespace PharmacyApp.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // SQLite
            var rootPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../.."));
            var dbPath = Path.Combine(rootPath, "PharmacyApp.db");
            Console.WriteLine($"База даних SQLite: {dbPath}");

            var options = new DbContextOptionsBuilder<PharmacyAppContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            using (var ctx = new PharmacyAppContext(options))
            {
                ctx.Database.Migrate();
            }

            using (var ctx = new PharmacyAppContext(options))
            {
                var medRepo = new EfRepository<MedicineModel>(ctx);
                var pharmRepo = new EfRepository<PharmacyModel>(ctx);
                var custRepo = new EfRepository<CustomerModel>(ctx);
                var presRepo = new EfRepository<PrescriptionModel>(ctx);

                var medService = new EfCrudServiceAsync<MedicineModel>(medRepo);
                var pharmService = new EfCrudServiceAsync<PharmacyModel>(pharmRepo);
                var custService = new EfCrudServiceAsync<CustomerModel>(custRepo);
                var presService = new EfCrudServiceAsync<PrescriptionModel>(presRepo);

                var medicines = new[]
                {
                    new MedicineModel { Name = "Аспірин", Price = 55.5, QuantityInStock = 120 },
                    new MedicineModel { Name = "Ібупрофен", Price = 78.0, QuantityInStock = 80 },
                    new MedicineModel { Name = "Парацетамол", Price = 45.3, QuantityInStock = 150 }
                };

                var pharmacies = new[]
                {
                    new PharmacyModel { Name = "Аптека №1", Address = "вул. Шевченка, 12" },
                    new PharmacyModel { Name = "Аптека Добра", Address = "пр. Перемоги, 45" }
                };


                foreach (var m in medicines) await medService.CreateAsync(m);
                foreach (var p in pharmacies) await pharmService.CreateAsync(p);

                Console.WriteLine("\nЛіки:");
                foreach (var m in (await medService.ReadAllAsync()).ToList())
                    Console.WriteLine($"{m.Id} — {m.Name} — {m.Price} грн — {m.QuantityInStock} шт");

                Console.WriteLine("\nАптеки:");
                foreach (var p in (await pharmService.ReadAllAsync()).ToList())
                    Console.WriteLine($"{p.Id} — {p.Name} — {p.Address}");
            }

            // MongoDB
            Console.WriteLine("\nMongoDB (Atlas)");

            var connectionString = "mongodb+srv://user:pass@cluster0.7isku0n.mongodb.net/?retryWrites=true&w=majority";
            var databaseName = "PharmacyNoSqlDb";

            var medRepoNoSql = new NosqlRepository<MedicineDocument>("medicines", connectionString, databaseName);
            var pharmRepoNoSql = new NosqlRepository<PharmacyDocument>("pharmacies", connectionString, databaseName);
            var custRepoNoSql = new NosqlRepository<CustomerDocument>("customers", connectionString, databaseName);
            var presRepoNoSql = new NosqlRepository<PrescriptionDocument>("prescriptions", connectionString, databaseName);

            foreach (var m in new[] {
                new MedicineDocument { Name = "Аспірин", Price = 55.5, QuantityInStock = 120 },
                new MedicineDocument { Name = "Ібупрофен", Price = 78.0, QuantityInStock = 80 },
                new MedicineDocument { Name = "Парацетамол", Price = 45.3, QuantityInStock = 150 }
            }) await medRepoNoSql.AddAsync(m);

            foreach (var p in new[] {
                new PharmacyDocument { Name = "Аптека №1", Address = "вул. Шевченка, 12" },
                new PharmacyDocument { Name = "Аптека Добра", Address = "пр. Перемоги, 45" }
            }) await pharmRepoNoSql.AddAsync(p);

            foreach (var c in new[] {
                new CustomerDocument { Name = "Іван Петренко", Phone = "+380501112233", Email = "ivan.petrenko@example.com" },
                new CustomerDocument { Name = "Марія Іваненко", Phone = "+380671234567", Email = "maria.ivanenko@example.com" }
            }) await custRepoNoSql.AddAsync(c);

            foreach (var pr in new[] {
                new PrescriptionDocument { CustomerName = "Іван Петренко", MedicineName = "Аспірин", Dosage = "2 таблетки на день" }
            }) await presRepoNoSql.AddAsync(pr);

            Console.WriteLine("\nЛіки:");
            foreach (var m in await medRepoNoSql.GetAllAsync())
                Console.WriteLine($"{m.Id} — {m.Name} — {m.Price} грн — {m.QuantityInStock} шт");

            Console.WriteLine("\nАптеки:");
            foreach (var p in await pharmRepoNoSql.GetAllAsync())
                Console.WriteLine($"{p.Id} — {p.Name} — {p.Address}");

            Console.WriteLine("\nКлієнти:");
            foreach (var c in await custRepoNoSql.GetAllAsync())
                Console.WriteLine($"{c.Id} — {c.Name} — {c.Email}");

            Console.WriteLine("\nРецепти:");
            foreach (var pr in await presRepoNoSql.GetAllAsync())
                Console.WriteLine($"{pr.Id} — {pr.CustomerName} — {pr.MedicineName} — {pr.Dosage}");

            Console.WriteLine("\nГотово. Натисніть Enter для виходу.");
            Console.ReadLine();
        }
    }
}
