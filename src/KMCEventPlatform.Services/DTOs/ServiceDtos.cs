using KMCEventPlatform.Models;

namespace KMCEventPlatform.Services.DTOs
{
    /// <summary>
    /// Data Transfer Object for Event
    /// </summary>
    public class EventDto
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OrganizerId { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int MaxParticipants { get; set; }
        public int RegisteredParticipants { get; set; }
        public EventStatus Status { get; set; }
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public List<string> ParticipantIds { get; set; } = new();
    }

    /// <summary>
    /// Data Transfer Object for Participant
    /// </summary>
    public class ParticipantDto
    {
        public string? Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ParticipantRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public List<string> RegisteredEventIds { get; set; } = new();
        public List<string> OrganizedEventIds { get; set; } = new();
    }

    /// <summary>
    /// Data Transfer Object for Registration
    /// </summary>
    public class RegistrationDto
    {
        public string? Id { get; set; }
        public string EventId { get; set; } = string.Empty;
        public string ParticipantId { get; set; } = string.Empty;
        public RegistrationStatus Status { get; set; }
        public int NumberOfGuests { get; set; }
        public string Comments { get; set; } = string.Empty;
    }

    public class RegisterRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ParticipantRole Role { get; set; } = ParticipantRole.Regular;
    }

    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
        public ParticipantDto User { get; set; } = new();
    }
}
