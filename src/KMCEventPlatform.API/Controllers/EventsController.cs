using Microsoft.AspNetCore.Mvc;
using KMCEventPlatform.Services.Services;
using KMCEventPlatform.Services.DTOs;

namespace KMCEventPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(IEventService eventService, ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        /// <summary>
        /// Get all events
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            _logger.LogInformation("Getting all events");
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        /// <summary>
        /// Get event by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventDto>> GetEventById(string id)
        {
            _logger.LogInformation($"Getting event with ID: {id}");
            var @event = await _eventService.GetEventByIdAsync(id);

            if (@event == null)
                return NotFound(new { message = "Event not found" });

            return Ok(@event);
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] EventDto eventDto)
        {
            _logger.LogInformation($"Creating new event: {eventDto.Title}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEvent = await _eventService.CreateEventAsync(eventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        /// <summary>
        /// Update an existing event
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EventDto>> UpdateEvent(string id, [FromBody] EventDto eventDto)
        {
            _logger.LogInformation($"Updating event with ID: {id}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedEvent = await _eventService.UpdateEventAsync(id, eventDto);

            if (updatedEvent == null)
                return NotFound(new { message = "Event not found" });

            return Ok(updatedEvent);
        }

        /// <summary>
        /// Delete an event
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            _logger.LogInformation($"Deleting event with ID: {id}");
            var result = await _eventService.DeleteEventAsync(id);

            if (!result)
                return NotFound(new { message = "Event not found" });

            return NoContent();
        }

        /// <summary>
        /// Search events by title
        /// </summary>
        [HttpGet("search/title/{title}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDto>>> SearchByTitle(string title)
        {
            _logger.LogInformation($"Searching events by title: {title}");
            var events = await _eventService.SearchEventsByTitleAsync(title);
            return Ok(events);
        }

        /// <summary>
        /// Search events by category
        /// </summary>
        [HttpGet("search/category/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDto>>> SearchByCategory(string category)
        {
            _logger.LogInformation($"Searching events by category: {category}");
            var events = await _eventService.SearchEventsByCategoryAsync(category);
            return Ok(events);
        }

        /// <summary>
        /// Search events by date range
        /// </summary>
        [HttpGet("search/daterange")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDto>>> SearchByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            _logger.LogInformation($"Searching events from {startDate} to {endDate}");
            var events = await _eventService.SearchEventsByDateRangeAsync(startDate, endDate);
            return Ok(events);
        }

        /// <summary>
        /// Get events by organizer
        /// </summary>
        [HttpGet("organizer/{organizerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetByOrganizer(string organizerId)
        {
            _logger.LogInformation($"Getting events for organizer: {organizerId}");
            var events = await _eventService.GetEventsByOrganizerAsync(organizerId);
            return Ok(events);
        }

        /// <summary>
        /// Register a participant for an event
        /// </summary>
        [HttpPost("{eventId}/register/{participantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterParticipant(string eventId, string participantId)
        {
            _logger.LogInformation($"Registering participant {participantId} for event {eventId}");
            var result = await _eventService.RegisterParticipantAsync(eventId, participantId);

            if (!result)
                return BadRequest(new { message = "Failed to register participant. Event may be full or not found." });

            return Ok(new { message = "Participant registered successfully" });
        }

        /// <summary>
        /// Unregister a participant from an event
        /// </summary>
        [HttpDelete("{eventId}/unregister/{participantId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnregisterParticipant(string eventId, string participantId)
        {
            _logger.LogInformation($"Unregistering participant {participantId} from event {eventId}");
            var result = await _eventService.UnregisterParticipantAsync(eventId, participantId);

            if (!result)
                return BadRequest(new { message = "Failed to unregister participant" });

            return Ok(new { message = "Participant unregistered successfully" });
        }
    }
}
