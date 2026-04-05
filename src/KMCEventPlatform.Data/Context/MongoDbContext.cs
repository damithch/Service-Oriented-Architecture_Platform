using MongoDB.Driver;
using KMCEventPlatform.Models;
using Microsoft.Extensions.Options;

namespace KMCEventPlatform.Data.Context
{
    /// <summary>
    /// MongoDB database context for managing collections
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<Configuration.MongoDbSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Event> Events => _database.GetCollection<Event>("Events");
        public IMongoCollection<Participant> Participants => _database.GetCollection<Participant>("Participants");
        public IMongoCollection<Registration> Registrations => _database.GetCollection<Registration>("Registrations");
    }
}
