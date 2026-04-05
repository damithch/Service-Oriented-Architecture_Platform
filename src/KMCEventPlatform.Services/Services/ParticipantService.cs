using KMCEventPlatform.Models;
using KMCEventPlatform.Data.Repositories;
using KMCEventPlatform.Services.DTOs;

namespace KMCEventPlatform.Services.Services
{
    /// <summary>
    /// Service interface for Participant operations
    /// </summary>
    public interface IParticipantService
    {
        Task<List<ParticipantDto>> GetAllParticipantsAsync();
        Task<ParticipantDto?> GetParticipantByIdAsync(string id);
        Task<ParticipantDto?> GetParticipantByEmailAsync(string email);
        Task<ParticipantDto> CreateParticipantAsync(ParticipantDto participantDto);
        Task<ParticipantDto?> UpdateParticipantAsync(string id, ParticipantDto participantDto);
        Task<bool> DeleteParticipantAsync(string id);
        Task<List<ParticipantDto>> GetOrganizersAsync();
    }

    /// <summary>
    /// Participant service implementation
    /// </summary>
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly AutoMapper.IMapper _mapper;

        public ParticipantService(
            IParticipantRepository participantRepository,
            AutoMapper.IMapper mapper)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
        }

        public async Task<List<ParticipantDto>> GetAllParticipantsAsync()
        {
            var participants = await _participantRepository.GetAllAsync();
            return _mapper.Map<List<ParticipantDto>>(participants);
        }

        public async Task<ParticipantDto?> GetParticipantByIdAsync(string id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);
            return _mapper.Map<ParticipantDto?>(participant);
        }

        public async Task<ParticipantDto?> GetParticipantByEmailAsync(string email)
        {
            var participant = await _participantRepository.GetByEmailAsync(email);
            return _mapper.Map<ParticipantDto?>(participant);
        }

        public async Task<ParticipantDto> CreateParticipantAsync(ParticipantDto participantDto)
        {
            var participant = _mapper.Map<Participant>(participantDto);
            participant.CreatedAt = DateTime.UtcNow;
            participant.UpdatedAt = DateTime.UtcNow;

            var createdParticipant = await _participantRepository.CreateAsync(participant);
            return _mapper.Map<ParticipantDto>(createdParticipant);
        }

        public async Task<ParticipantDto?> UpdateParticipantAsync(string id, ParticipantDto participantDto)
        {
            var existingParticipant = await _participantRepository.GetByIdAsync(id);
            if (existingParticipant == null)
                return null;

            var participant = _mapper.Map<Participant>(participantDto);
            participant.Id = id;
            participant.CreatedAt = existingParticipant.CreatedAt;
            participant.UpdatedAt = DateTime.UtcNow;

            var updatedParticipant = await _participantRepository.UpdateAsync(id, participant);
            return _mapper.Map<ParticipantDto?>(updatedParticipant);
        }

        public async Task<bool> DeleteParticipantAsync(string id)
        {
            return await _participantRepository.DeleteAsync(id);
        }

        public async Task<List<ParticipantDto>> GetOrganizersAsync()
        {
            var organizers = await _participantRepository.GetOrganizersByRoleAsync();
            return _mapper.Map<List<ParticipantDto>>(organizers);
        }
    }
}
