using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PharmacyApp.Common;

namespace PharmacyApp.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var service = new InMemoryCrudServiceAsync<Medicine>("medicines_async.json");

            DemonstrateSyncPrimitives();

            const int total = 5555;
            Console.WriteLine($"Паралельне створення {total} ліків...");

            var sw = Stopwatch.StartNew();

            var meds = Enumerable.Range(0, total).Select(_ => Medicine.CreateNew()).ToList();

            await Parallel.ForEachAsync(meds, async (med, token) =>
            {
                await service.CreateAsync(med);
            });

            sw.Stop();
            Console.WriteLine($"Створено {total} об'єктів за {sw.ElapsedMilliseconds} мс");

            Console.WriteLine("\nЗбереження у файл...");
            await service.SaveAsync();
            Console.WriteLine("medicines_async.json збережено.");

            var all = (await service.ReadAllAsync()).ToList();
            Console.WriteLine($"\nУсього ліків у пам’яті: {all.Count}");

            var minPrice = all.Min(m => m.Price);
            var maxPrice = all.Max(m => m.Price);
            var avgPrice = all.Average(m => m.Price);

            var minQty = all.Min(m => m.QuantityInStock);
            var maxQty = all.Max(m => m.QuantityInStock);
            var avgQty = all.Average(m => m.QuantityInStock);

            Console.WriteLine("\nСтатистика цін:");
            Console.WriteLine($"Мінімальна: {minPrice:F2}, Максимальна: {maxPrice:F2}, Середня: {avgPrice:F2}");

            Console.WriteLine("\nСтатистика запасів:");
            Console.WriteLine($"Мінімальна: {minQty}, Максимальна: {maxQty}, Середня: {avgQty:F0}");

            Console.WriteLine("\nПерші 5 ліків (сторінка 1):");
            var page1 = await service.ReadAllAsync(1, 5);
            foreach (var m in page1)
                Console.WriteLine($"{m.Name} — {m.Price} грн — {m.QuantityInStock} шт");

            Console.WriteLine("\nРоботу завершено.");
            Console.ReadLine();
        }

        static void DemonstrateSyncPrimitives()
        {
            Console.WriteLine("Демонстрація примітивів синхронізації:");

            object locker = new object();
            int counter = 0;
            Parallel.For(0, 500, i =>
            {
                lock (locker)
                {
                    counter++;
                }
            });
            Console.WriteLine($"lock: counter = {counter} (очікується 500)");

            var semaphore = new SemaphoreSlim(2);
            Parallel.For(0, 5, i =>
            {
                semaphore.Wait();
                try
                {
                    Console.WriteLine($"Потік {Thread.CurrentThread.ManagedThreadId} виконує роботу...");
                    Thread.Sleep(100);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            Console.WriteLine("SemaphoreSlim: завершено");

            var are = new AutoResetEvent(false);
            var t = new Thread(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("AutoResetEvent: сигнал від потоку");
                are.Set();
            });
            t.Start();
            are.WaitOne();
            Console.WriteLine("AutoResetEvent: сигнал отримано\n");
        }
    }
}
