using KMCEventPlatform.Models;
using KMCEventPlatform.Data.Repositories;
using KMCEventPlatform.Services.DTOs;

namespace KMCEventPlatform.Services.Services
{
    /// <summary>
    /// Service interface for Event operations
    /// </summary>
    public interface IEventService
    {
        Task<List<EventDto>> GetAllEventsAsync();
        Task<EventDto?> GetEventByIdAsync(string id);
        Task<EventDto> CreateEventAsync(EventDto eventDto);
        Task<EventDto?> UpdateEventAsync(string id, EventDto eventDto);
        Task<bool> DeleteEventAsync(string id);
        Task<List<EventDto>> SearchEventsByTitleAsync(string title);
        Task<List<EventDto>> SearchEventsByCategoryAsync(string category);
        Task<List<EventDto>> SearchEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<EventDto>> GetEventsByOrganizerAsync(string organizerId);
        Task<List<ParticipantDto>> GetParticipantsForEventAsync(string eventId);
        Task<bool> RegisterParticipantAsync(string eventId, string participantId);
        Task<bool> UnregisterParticipantAsync(string eventId, string participantId);
    }

    /// <summary>
    /// Event service implementation
    /// </summary>
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly AutoMapper.IMapper _mapper;

        public EventService(
            IEventRepository eventRepository,
            IParticipantRepository participantRepository,
            IRegistrationRepository registrationRepository,
            AutoMapper.IMapper mapper)
        {
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;
            _registrationRepository = registrationRepository;
            _mapper = mapper;
        }

        public async Task<List<EventDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<EventDto?> GetEventByIdAsync(string id)
        {
            var @event = await _eventRepository.GetByIdAsync(id);
            return _mapper.Map<EventDto?>(@event);
        }

        public async Task<EventDto> CreateEventAsync(EventDto eventDto)
        {
            var @event = _mapper.Map<Event>(eventDto);
            @event.ParticipantIds ??= new List<string>();
            @event.CreatedAt = DateTime.UtcNow;
            @event.UpdatedAt = DateTime.UtcNow;

            var createdEvent = await _eventRepository.CreateAsync(@event);
            await _participantRepository.UpdateOrganizedEventsAsync(createdEvent.OrganizerId, createdEvent.Id, true);
            return _mapper.Map<EventDto>(createdEvent);
        }

        public async Task<EventDto?> UpdateEventAsync(string id, EventDto eventDto)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(id);
            if (existingEvent == null)
                return null;

            var @event = _mapper.Map<Event>(eventDto);
            @event.Id = id;
            @event.ParticipantIds = existingEvent.ParticipantIds;
            @event.CreatedAt = existingEvent.CreatedAt;
            @event.UpdatedAt = DateTime.UtcNow;

            var updatedEvent = await _eventRepository.UpdateAsync(id, @event);
            if (!string.Equals(existingEvent.OrganizerId, @event.OrganizerId, StringComparison.Ordinal))
            {
                await _participantRepository.UpdateOrganizedEventsAsync(existingEvent.OrganizerId, id, false);
                await _participantRepository.UpdateOrganizedEventsAsync(@event.OrganizerId, id, true);
            }

            return _mapper.Map<EventDto?>(updatedEvent);
        }

        public async Task<bool> DeleteEventAsync(string id)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(id);
            if (existingEvent == null)
                return false;

            var deleted = await _eventRepository.DeleteAsync(id);
            if (deleted)
            {
                await _participantRepository.UpdateOrganizedEventsAsync(existingEvent.OrganizerId, id, false);
            }

            return deleted;
        }

        public async Task<List<EventDto>> SearchEventsByTitleAsync(string title)
        {
            var events = await _eventRepository.SearchByTitleAsync(title);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> SearchEventsByCategoryAsync(string category)
        {
            var events = await _eventRepository.SearchByCategoryAsync(category);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> SearchEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var events = await _eventRepository.SearchByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<EventDto>> GetEventsByOrganizerAsync(string organizerId)
        {
            var events = await _eventRepository.GetEventsByOrganizerAsync(organizerId);
            return _mapper.Map<List<EventDto>>(events);
        }

        public async Task<List<ParticipantDto>> GetParticipantsForEventAsync(string eventId)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId);
            if (@event == null || @event.ParticipantIds.Count == 0)
                return new List<ParticipantDto>();

            var participants = await _participantRepository.GetByIdsAsync(@event.ParticipantIds);
            return _mapper.Map<List<ParticipantDto>>(participants);
        }

        public async Task<bool> RegisterParticipantAsync(string eventId, string participantId)
        {
            var @event = await _eventRepository.GetByIdAsync(eventId);
            if (@event == null || @event.RegisteredParticipants >= @event.MaxParticipants)
                return false;

            if (@event.ParticipantIds.Contains(participantId))
                return false;

            var participant = await _participantRepository.GetByIdAsync(participantId);
            if (participant == null || !participant.IsActive)
                return false;

            // Create registration
            var registration = new Registration
            {
                EventId = eventId,
                ParticipantId = participantId,
                Status = RegistrationStatus.Registered
            };

            await _registrationRepository.CreateAsync(registration);

            // Update event participant count
            @event.RegisteredParticipants++;
            @event.ParticipantIds.Add(participantId);
            @event.UpdatedAt = DateTime.UtcNow;
            await _eventRepository.UpdateAsync(eventId, @event);

            // Update participant registered events
            await _participantRepository.UpdateRegisteredEventsAsync(participantId, eventId, true);

            return true;
        }

        public async Task<bool> UnregisterParticipantAsync(string eventId, string participantId)
        {
            var registration = await _registrationRepository.GetRegistrationAsync(eventId, participantId);
            if (registration == null)
                return false;

            await _registrationRepository.DeleteAsync(registration.Id);

            // Update event participant count
            var @event = await _eventRepository.GetByIdAsync(eventId);
            if (@event != null)
            {
                @event.RegisteredParticipants--;
                @event.ParticipantIds.Remove(participantId);
                @event.UpdatedAt = DateTime.UtcNow;
                await _eventRepository.UpdateAsync(eventId, @event);
            }

            // Update participant registered events
            await _participantRepository.UpdateRegisteredEventsAsync(participantId, eventId, false);

            return true;
        }
    }
}
