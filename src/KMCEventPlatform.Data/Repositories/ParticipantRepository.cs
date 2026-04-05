using MongoDB.Driver;
using KMCEventPlatform.Models;

namespace KMCEventPlatform.Data.Repositories
{
    /// <summary>
    /// Specialized repository interface for Participant entity
    /// </summary>
    public interface IParticipantRepository : IRepository<Participant>
    {
        Task<Participant?> GetByEmailAsync(string email);
        Task<List<Participant>> GetOrganizersByRoleAsync();
        Task<bool> UpdateRegisteredEventsAsync(string participantId, string eventId, bool register);
    }

    /// <summary>
    /// Participant repository implementation
    /// </summary>
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(IMongoCollection<Participant> collection) : base(collection)
        {
        }

        public async Task<Participant?> GetByEmailAsync(string email)
        {
            var filter = Builders<Participant>.Filter.Eq(x => x.Email, email);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Participant>> GetOrganizersByRoleAsync()
        {
            var filter = Builders<Participant>.Filter.Eq(x => x.Role, ParticipantRole.Organizer);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateRegisteredEventsAsync(string participantId, string eventId, bool register)
        {
            var filter = Builders<Participant>.Filter.Eq(x => x.Id, participantId);
            var update = register
                ? Builders<Participant>.Update.AddToSet(x => x.RegisteredEventIds, eventId)
                : Builders<Participant>.Update.Pull(x => x.RegisteredEventIds, eventId);

            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
