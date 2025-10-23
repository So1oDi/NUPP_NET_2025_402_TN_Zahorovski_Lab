using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PharmacyApp.Common;
using Xunit;

namespace PharmacyApp.Tests
{
    public class InMemoryCrudServiceAsyncTests
    {
        private readonly string _testFile = "test_medicines.json";

        private InMemoryCrudServiceAsync<Medicine> CreateService()
        {
            if (File.Exists(_testFile))
                File.Delete(_testFile);
            return new InMemoryCrudServiceAsync<Medicine>(_testFile);
        }

        [Fact]
        public async Task CreateAndRead_ShouldWorkCorrectly()
        {
            var service = CreateService();
            var med = new Medicine("Аспірин", 15.5, 100);

            var created = await service.CreateAsync(med);
            Assert.True(created);

            var read = await service.ReadAsync(med.Id);
            Assert.NotNull(read);
            Assert.Equal("Аспірин", read.Name);
        }

        [Fact]
        public async Task Update_ShouldChangeData()
        {
            var service = CreateService();
            var med = new Medicine("Парацетамол", 10, 50);
            await service.CreateAsync(med);

            med.Price = 12.5;
            var updated = await service.UpdateAsync(med);
            Assert.True(updated);

            var read = await service.ReadAsync(med.Id);
            Assert.Equal(12.5, read.Price);
        }

        [Fact]
        public async Task Remove_ShouldDeleteItem()
        {
            var service = CreateService();
            var med = new Medicine("Но-шпа", 25, 70);
            await service.CreateAsync(med);

            var removed = await service.RemoveAsync(med);
            Assert.True(removed);

            var read = await service.ReadAsync(med.Id);
            Assert.Null(read);
        }

        [Fact]
        public async Task SaveAndLoad_ShouldPersistData()
        {
            var service = CreateService();
            var med = new Medicine("Ібупрофен", 30, 200);
            await service.CreateAsync(med);
            await service.SaveAsync();

            var newService = new InMemoryCrudServiceAsync<Medicine>(_testFile);
            await newService.LoadAsync();

            var all = await newService.ReadAllAsync();
            Assert.Single(all);
            Assert.Equal("Ібупрофен", all.First().Name);
        }

        [Fact]
        public async Task ReadAll_WithPagination_ShouldReturnCorrectAmount()
        {
            var service = CreateService();
            for (int i = 0; i < 10; i++)
                await service.CreateAsync(new Medicine($"Med{i}", i + 10, 100));

            var page1 = await service.ReadAllAsync(1, 3);
            var page2 = await service.ReadAllAsync(2, 3);

            Assert.Equal(3, page1.Count());
            Assert.Equal(3, page2.Count());
        }
    }
}
