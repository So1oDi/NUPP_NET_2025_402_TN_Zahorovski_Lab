using PharmacyApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyApp.Infrastructure.Services
{
    public interface ICrudServiceAsync<T> where T : class
    {
        Task<bool> CreateAsync(T element);
        Task<T> ReadAsync(int id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<IEnumerable<T>> ReadAllAsync(int page, int amount);
        Task<bool> UpdateAsync(T element);
        Task<bool> RemoveAsync(T element);
        Task<bool> SaveAsync();
    }

    public class EfCrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repo;

        public EfCrudServiceAsync(IRepository<T> repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            await _repo.AddAsync(element);
            return true;
        }

        public async Task<T> ReadAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var all = await _repo.GetAllAsync();
            var skip = (page - 1) * amount;
            return System.Linq.Enumerable.Skip(all, skip).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            await _repo.UpdateAsync(element);
            return true;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            await _repo.DeleteAsync(element);
            return true;
        }

        public Task<bool> SaveAsync()
        {
            return Task.FromResult(true);
        }
    }
}
