using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyApp.Nosql.Repositories
{
    public class NosqlRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public NosqlRepository(string collectionName, string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task AddAsync(T entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task<List<T>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task DeleteAsync(FilterDefinition<T> filter) =>
            await _collection.DeleteOneAsync(filter);
    }
}
