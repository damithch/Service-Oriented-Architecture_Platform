using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace KMCEventPlatform.Models
{
    /// <summary>
    /// Represents a Participant/User in the KMC Event Platform
    /// </summary>
    public class Participant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        /// <summary>
        /// User's full name
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// User's email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User's phone number
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// User's address
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// User role (Regular, Organizer, Admin)
        /// </summary>
        public ParticipantRole Role { get; set; } = ParticipantRole.Regular;

        /// <summary>
        /// List of event IDs the user has registered for
        /// </summary>
        public List<string> RegisteredEventIds { get; set; } = new();

        /// <summary>
        /// List of event IDs created by the organizer
        /// </summary>
        public List<string> OrganizedEventIds { get; set; } = new();

        /// <summary>
        /// Account creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Whether the account is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// User preferences
        /// </summary>
        public UserPreferences Preferences { get; set; } = new();
    }

    /// <summary>
    /// Participant role enumeration
    /// </summary>
    public enum ParticipantRole
    {
        Regular,
        Organizer,
        Admin
    }

    /// <summary>
    /// User preferences
    /// </summary>
    public class UserPreferences
    {
        /// <summary>
        /// Preferred event categories
        /// </summary>
        public List<string> PreferredCategories { get; set; } = new();

        /// <summary>
        /// Notify about new events
        /// </summary>
        public bool NotifyNewEvents { get; set; } = true;

        /// <summary>
        /// Notify about event changes
        /// </summary>
        public bool NotifyEventChanges { get; set; } = true;
    }
}
