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
        Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerDto);
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginDto);
    }

    /// <summary>
    /// Participant service implementation
    /// </summary>
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly AutoMapper.IMapper _mapper;
        private readonly IAuthService _authService;

        public ParticipantService(
            IParticipantRepository participantRepository,
            AutoMapper.IMapper mapper,
            IAuthService authService)
        {
            _participantRepository = participantRepository;
            _mapper = mapper;
            _authService = authService;
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
            participant.PasswordHash = string.Empty;

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
            participant.PasswordHash = existingParticipant.PasswordHash;
            participant.RegisteredEventIds = existingParticipant.RegisteredEventIds;
            participant.OrganizedEventIds = existingParticipant.OrganizedEventIds;
            participant.Preferences = existingParticipant.Preferences;
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

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerDto)
        {
            var normalizedEmail = registerDto.Email.Trim().ToLowerInvariant();
            var existingParticipant = await _participantRepository.GetByEmailAsync(normalizedEmail);
            if (existingParticipant != null)
                return null;

            var role = registerDto.Role == ParticipantRole.Admin
                ? ParticipantRole.Regular
                : registerDto.Role;

            var participant = new Participant
            {
                FullName = registerDto.FullName,
                Email = normalizedEmail,
                PasswordHash = _authService.HashPassword(registerDto.Password),
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                Role = role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdParticipant = await _participantRepository.CreateAsync(participant);
            var userDto = _mapper.Map<ParticipantDto>(createdParticipant);
            var token = _authService.GenerateToken(createdParticipant);

            return new AuthResponseDto
            {
                Token = token.Token,
                ExpiresAtUtc = token.ExpiresAtUtc,
                User = userDto
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginDto)
        {
            var participant = await _participantRepository.GetByEmailAsync(loginDto.Email.Trim().ToLowerInvariant());
            if (participant == null || !participant.IsActive)
                return null;

            if (!_authService.VerifyPassword(participant.PasswordHash, loginDto.Password))
                return null;

            var userDto = _mapper.Map<ParticipantDto>(participant);
            var token = _authService.GenerateToken(participant);

            return new AuthResponseDto
            {
                Token = token.Token,
                ExpiresAtUtc = token.ExpiresAtUtc,
                User = userDto
            };
        }
    }
}
