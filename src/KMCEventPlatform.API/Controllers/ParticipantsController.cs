using Microsoft.AspNetCore.Mvc;
using KMCEventPlatform.Services.Services;
using KMCEventPlatform.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace KMCEventPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly ILogger<ParticipantsController> _logger;

        public ParticipantsController(IParticipantService participantService, ILogger<ParticipantsController> logger)
        {
            _participantService = participantService;
            _logger = logger;
        }

        /// <summary>
        /// Get all participants
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetAllParticipants()
        {
            _logger.LogInformation("Getting all participants");
            var participants = await _participantService.GetAllParticipantsAsync();
            return Ok(participants);
        }

        /// <summary>
        /// Get participant by ID
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParticipantDto>> GetParticipantById(string id)
        {
            _logger.LogInformation($"Getting participant with ID: {id}");

            if (!User.IsInRole("Admin"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.Equals(currentUserId, id, StringComparison.Ordinal))
                    return Forbid();
            }

            var participant = await _participantService.GetParticipantByIdAsync(id);

            if (participant == null)
                return NotFound(new { message = "Participant not found" });

            return Ok(participant);
        }

        /// <summary>
        /// Get participant by email
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParticipantDto>> GetParticipantByEmail(string email)
        {
            _logger.LogInformation($"Getting participant by email: {email}");
            var participant = await _participantService.GetParticipantByEmailAsync(email);

            if (participant == null)
                return NotFound(new { message = "Participant not found" });

            return Ok(participant);
        }

        /// <summary>
        /// Create a new participant
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParticipantDto>> CreateParticipant([FromBody] ParticipantDto participantDto)
        {
            _logger.LogInformation($"Creating new participant: {participantDto.FullName}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdParticipant = await _participantService.CreateParticipantAsync(participantDto);
            return CreatedAtAction(nameof(GetParticipantById), new { id = createdParticipant.Id }, createdParticipant);
        }

        /// <summary>
        /// Update an existing participant
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ParticipantDto>> UpdateParticipant(string id, [FromBody] ParticipantDto participantDto)
        {
            _logger.LogInformation($"Updating participant with ID: {id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!User.IsInRole("Admin"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.Equals(currentUserId, id, StringComparison.Ordinal))
                    return Forbid();

                var currentRole = User.FindFirstValue(ClaimTypes.Role);
                if (!string.Equals(currentRole, participantDto.Role.ToString(), StringComparison.Ordinal))
                    return BadRequest(new { message = "Users cannot change their own role." });
            }

            var updatedParticipant = await _participantService.UpdateParticipantAsync(id, participantDto);

            if (updatedParticipant == null)
                return NotFound(new { message = "Participant not found" });

            return Ok(updatedParticipant);
        }

        /// <summary>
        /// Delete a participant
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteParticipant(string id)
        {
            _logger.LogInformation($"Deleting participant with ID: {id}");
            var result = await _participantService.DeleteParticipantAsync(id);

            if (!result)
                return NotFound(new { message = "Participant not found" });

            return NoContent();
        }

        /// <summary>
        /// Get all event organizers
        /// </summary>
        [Authorize(Roles = "Organizer,Admin")]
        [HttpGet("organizers/list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParticipantDto>>> GetOrganizers()
        {
            _logger.LogInformation("Getting all organizers");
            var organizers = await _participantService.GetOrganizersAsync();
            return Ok(organizers);
        }
    }
}
