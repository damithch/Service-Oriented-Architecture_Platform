using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KMCEventPlatform.Models
{
    /// <summary>
    /// Represents an Event in the KMC Event Platform
    /// </summary>
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// Event title/name
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the event
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ID of the event organizer/creator
        /// </summary>
        public string OrganizerId { get; set; } = string.Empty;

        /// <summary>
        /// Name of the event organizer
        /// </summary>
        public string OrganizerName { get; set; } = string.Empty;

        /// <summary>
        /// Event start date and time
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Event end date and time
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Event location/venue
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Category of the event (e.g., Sports, Cultural, Business, Entertainment)
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Maximum number of participants allowed
        /// </summary>
        public int MaxParticipants { get; set; }

        /// <summary>
        /// Number of current registrations
        /// </summary>
        public int RegisteredParticipants { get; set; }

        /// <summary>
        /// Event status (Active, Ongoing, Completed, Cancelled)
        /// </summary>
        public EventStatus Status { get; set; } = EventStatus.Active;

        /// <summary>
        /// Contact email for the event
        /// </summary>
        public string ContactEmail { get; set; } = string.Empty;

        /// <summary>
        /// Contact phone number
        /// </summary>
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// Event image stored as a URL or data URL
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Event creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// List of participant IDs
        /// </summary>
        public List<string> ParticipantIds { get; set; } = new();

        /// <summary>
        /// Additional metadata/tags
        /// </summary>
        public List<string> Tags { get; set; } = new();
    }

    /// <summary>
    /// Event status enumeration
    /// </summary>
    public enum EventStatus
    {
        Active,
        Ongoing,
        Completed,
        Cancelled
    }
}
