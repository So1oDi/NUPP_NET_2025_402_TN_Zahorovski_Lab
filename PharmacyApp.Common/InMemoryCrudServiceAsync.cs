using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PharmacyApp.Common
{
    public class InMemoryCrudServiceAsync<T> : ICrudServiceAsync<T> where T : IEntity
    {
        private readonly ConcurrentDictionary<Guid, T> _store = new ConcurrentDictionary<Guid, T>();
        private readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1);
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        public string FilePath { get; set; }

        public InMemoryCrudServiceAsync(string filePath = "data.json")
        {
            FilePath = filePath;
        }

        public async Task<bool> CreateAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (element.Id == Guid.Empty) element.Id = Guid.NewGuid();
            var added = _store.TryAdd(element.Id, element);
            await Task.CompletedTask;
            return added;
        }

        public async Task<T> ReadAsync(Guid id)
        {
            _store.TryGetValue(id, out var element);
            await Task.CompletedTask;
            return element;
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            _rwLock.EnterReadLock();
            try
            {
                var list = _store.Values.ToList();
                return await Task.FromResult(list);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            _rwLock.EnterReadLock();
            try
            {
                var result = _store.Values.Skip((page - 1) * amount).Take(amount).ToList();
                return await Task.FromResult(result);
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public async Task<bool> UpdateAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (element.Id == Guid.Empty) return false;

            var updated = false;
            _rwLock.EnterUpgradeableReadLock();
            try
            {
                if (_store.ContainsKey(element.Id))
                {
                    _rwLock.EnterWriteLock();
                    try
                    {
                        _store[element.Id] = element;
                        updated = true;
                    }
                    finally
                    {
                        _rwLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _rwLock.ExitUpgradeableReadLock();
            }

            await Task.CompletedTask;
            return updated;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var removed = _store.TryRemove(element.Id, out _);
            await Task.CompletedTask;
            return removed;
        }

        public async Task<bool> SaveAsync()
        {
            await _fileSemaphore.WaitAsync();
            try
            {
                string json;
                _rwLock.EnterReadLock();
                try
                {
                    json = JsonConvert.SerializeObject(_store.Values, Formatting.Indented);
                }
                finally
                {
                    _rwLock.ExitReadLock();
                }

                await File.WriteAllTextAsync(FilePath, json);

                return true;
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }


        public async Task LoadAsync()
        {
            if (!File.Exists(FilePath)) return;
            await _fileSemaphore.WaitAsync();
            try
            {
                var json = await File.ReadAllTextAsync(FilePath);
                var items = JsonConvert.DeserializeObject<List<T>>(json);
                if (items == null) return;

                _rwLock.EnterWriteLock();
                try
                {
                    _store.Clear();
                    foreach (var item in items)
                        _store.TryAdd(item.Id, item);
                }
                finally
                {
                    _rwLock.ExitWriteLock();
                }
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var snapshot = _store.Values.ToList();
            return snapshot.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
