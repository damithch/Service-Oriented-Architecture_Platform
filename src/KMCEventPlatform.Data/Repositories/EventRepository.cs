using MongoDB.Driver;
using KMCEventPlatform.Models;

namespace KMCEventPlatform.Data.Repositories
{
    /// <summary>
    /// Specialized repository interface for Event entity
    /// </summary>
    public interface IEventRepository : IRepository<Event>
    {
        Task<List<Event>> SearchByTitleAsync(string title);
        Task<List<Event>> SearchByCategoryAsync(string category);
        Task<List<Event>> SearchByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Event>> SearchByStatusAsync(EventStatus status);
        Task<List<Event>> GetEventsByOrganizerAsync(string organizerId);
    }

    /// <summary>
    /// Event repository implementation
    /// </summary>
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(IMongoCollection<Event> collection) : base(collection)
        {
        }

        public async Task<List<Event>> SearchByTitleAsync(string title)
        {
            var filter = Builders<Event>.Filter.Regex("Title", new MongoDB.Bson.BsonRegularExpression(title, "i"));
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Event>> SearchByCategoryAsync(string category)
        {
            var filter = Builders<Event>.Filter.Eq(x => x.Category, category);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Event>> SearchByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Event>.Filter.And(
                Builders<Event>.Filter.Gte(x => x.StartDate, startDate),
                Builders<Event>.Filter.Lte(x => x.EndDate, endDate)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Event>> SearchByStatusAsync(EventStatus status)
        {
            var filter = Builders<Event>.Filter.Eq(x => x.Status, status);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Event>> GetEventsByOrganizerAsync(string organizerId)
        {
            var filter = Builders<Event>.Filter.Eq(x => x.OrganizerId, organizerId);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
