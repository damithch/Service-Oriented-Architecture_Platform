using MongoDB.Driver;
using KMCEventPlatform.Models;

namespace KMCEventPlatform.Data.Repositories
{
    /// <summary>
    /// Specialized repository interface for Registration entity
    /// </summary>
    public interface IRegistrationRepository : IRepository<Registration>
    {
        Task<List<Registration>> GetByEventIdAsync(string eventId);
        Task<List<Registration>> GetByParticipantIdAsync(string participantId);
        Task<Registration?> GetRegistrationAsync(string eventId, string participantId);
        Task<bool> UpdateRegistrationStatusAsync(string registrationId, RegistrationStatus status);
    }

    /// <summary>
    /// Registration repository implementation
    /// </summary>
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public RegistrationRepository(IMongoCollection<Registration> collection) : base(collection)
        {
        }

        public async Task<List<Registration>> GetByEventIdAsync(string eventId)
        {
            var filter = Builders<Registration>.Filter.Eq(x => x.EventId, eventId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Registration>> GetByParticipantIdAsync(string participantId)
        {
            var filter = Builders<Registration>.Filter.Eq(x => x.ParticipantId, participantId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<Registration?> GetRegistrationAsync(string eventId, string participantId)
        {
            var filter = Builders<Registration>.Filter.And(
                Builders<Registration>.Filter.Eq(x => x.EventId, eventId),
                Builders<Registration>.Filter.Eq(x => x.ParticipantId, participantId)
            );
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateRegistrationStatusAsync(string registrationId, RegistrationStatus status)
        {
            var filter = Builders<Registration>.Filter.Eq(x => x.Id, registrationId);
            var update = Builders<Registration>.Update.Set(x => x.Status, status)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
