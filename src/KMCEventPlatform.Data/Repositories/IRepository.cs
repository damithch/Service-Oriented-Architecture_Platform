using MongoDB.Driver;
using KMCEventPlatform.Models;

namespace KMCEventPlatform.Data.Repositories
{
    /// <summary>
    /// Interface for generic repository operations
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(string id, T entity);
        Task<bool> DeleteAsync(string id);
    }

    /// <summary>
    /// Generic repository implementation for MongoDB
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<T?> UpdateAsync(string id, T entity)
        {
            var options = new FindOneAndReplaceOptions<T>
            {
                ReturnDocument = ReturnDocument.After
            };
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.FindOneAndReplaceAsync(filter, entity, options);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
