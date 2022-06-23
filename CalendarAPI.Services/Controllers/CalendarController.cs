using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CalendarAPI.Services.Repository;
using CalendarAPI.Services.Logger;

namespace CalendarAPI.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IEventRepository _eventRepository;

        public CalendarController(IEventRepository eventRepository, ILoggerManager logger, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _mapper = mapper;   
        }

        // GET: api/<CalendarController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            try
            {
                var events = await _eventRepository.GetEventsAsync();
                _logger.LogInfo($"Returned all events from database.");

                var eventsResult = _mapper.Map<IEnumerable<EventDto>>(events);
                return Ok(eventsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllEvents action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }          
        }

        // GET api/<CalendarController>/5
        [HttpGet("{eventId}")]
        public async Task<ActionResult<EventDto>> GetOneEvent(string eventId)
        {
            try
            {
                var currEvent = await _eventRepository.GetEventByIDAsync(eventId);

                if (currEvent == null)
                {
                    _logger.LogError($"Event with id: {eventId}, hasn't been found in db.");
                    return NotFound($"Event with id: {eventId}, hasn't been found in db.");
                }

                _logger.LogInfo($"Returned Event with id: {eventId}");

                var eventResult = _mapper.Map<EventDto>(currEvent);
                return Ok(eventResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOneEvent action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }           
        }

        // POST api/<CalendarController>
        [HttpPost]
        public async Task<ActionResult> AddEvent([FromBody] EventDto eventDto)
        {
            try
            {
                var currEvent = _mapper.Map<Event>(eventDto);

                bool isAdded = _eventRepository.AddEvent(currEvent);

                if (!isAdded)
                {
                    _logger.LogError($"Event with id : {currEvent.Id} already exists.");
                    return BadRequest($"Event with id : {currEvent.Id} already exists.");
                }

                //return CreatedAtAction(nameof(GetOneEvent), new { id = currEvent.Id }, currEvent);

                _logger.LogInfo($"Event with id : {currEvent.Id} created.");

                return CreatedAtAction(nameof(AddEvent), new { id = currEvent.Id }, currEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddEvent action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }            
        }

        // PUT api/<CalendarController>/5
        [HttpPut]
        public async Task<ActionResult> PutEvent([FromBody] EventDto eventDto)
        {
            try
            {
                var currEvent = _mapper.Map<Event>(eventDto);

                bool isUpdated = _eventRepository.UpdateEvent(currEvent);

                if (!isUpdated)
                {
                    _logger.LogError($"Event with id: {currEvent.Id}, hasn't been found in db.");
                    return NotFound($"Event with id: {currEvent.Id}, hasn't been found in db");
                }

                _logger.LogInfo($"Event with id : {currEvent.Id} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PutEvent action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }            
        }

        // DELETE api/<CalendarController>/5
        [HttpDelete("{eventId}")]
        public async Task<ActionResult> DeleteEvent(string eventId)
        {
            try
            {
                bool isDeleted = _eventRepository.DeleteEvent(eventId);

                if (!isDeleted)
                {
                    _logger.LogError($"Event with id: {eventId}, hasn't been found in db.");
                    return NotFound($"Event with id: {eventId}, hasn't been found in db.");
                }

                _logger.LogInfo($"Event with id : {eventId} deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteEvent action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }           
        }
    }
}
