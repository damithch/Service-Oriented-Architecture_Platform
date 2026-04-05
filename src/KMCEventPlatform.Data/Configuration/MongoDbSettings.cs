namespace KMCEventPlatform.Data.Configuration
{
    /// <summary>
    /// MongoDB connection settings configuration
    /// </summary>
    public class MongoDbSettings
    {
        public const string SectionName = "MongoDbSettings";

        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "KMCEventPlatformDb";
        public string EventsCollectionName { get; set; } = "Events";
        public string ParticipantsCollectionName { get; set; } = "Participants";
        public string RegistrationsCollectionName { get; set; } = "Registrations";
    }
}
