using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KMCEventPlatform.Models
{
    /// <summary>
    /// Represents a Registration (Participant registered for an Event)
    /// </summary>
    public class Registration
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Event ID that the participant is registering for
        /// </summary>
        public string EventId { get; set; } = string.Empty;

        /// <summary>
        /// Participant ID who is registering
        /// </summary>
        public string ParticipantId { get; set; } = string.Empty;

        /// <summary>
        /// Registration status (Registered, Attended, NoShow, Cancelled)
        /// </summary>
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Registered;

        /// <summary>
        /// Number of accompanying persons
        /// </summary>
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Registration feedback/comments
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// Registration timestamp
        /// </summary>
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Special requirements or accessibility needs
        /// </summary>
        public List<string> SpecialRequirements { get; set; } = new();
    }

    /// <summary>
    /// Registration status enumeration
    /// </summary>
    public enum RegistrationStatus
    {
        Registered,
        Attended,
        NoShow,
        Cancelled
    }
}
