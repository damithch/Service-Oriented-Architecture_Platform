using System.Security.Claims;
using KMCEventPlatform.Services.DTOs;
using KMCEventPlatform.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KMCEventPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IParticipantService participantService, ILogger<AuthController> logger)
        {
            _participantService = participantService;
            _logger = logger;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto registerDto)
        {
            _logger.LogInformation("Registering user with email: {Email}", registerDto.Email);

            var authResponse = await _participantService.RegisterAsync(registerDto);
            if (authResponse == null)
                return BadRequest(new { message = "A user with that email already exists." });

            return Ok(authResponse);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            _logger.LogInformation("Logging in user with email: {Email}", loginDto.Email);

            var authResponse = await _participantService.LoginAsync(loginDto);
            if (authResponse == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(authResponse);
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ParticipantDto>> Me()
        {
            var participantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(participantId))
                return Unauthorized();

            var participant = await _participantService.GetParticipantByIdAsync(participantId);
            if (participant == null)
                return Unauthorized();

            return Ok(participant);
        }
    }
}
