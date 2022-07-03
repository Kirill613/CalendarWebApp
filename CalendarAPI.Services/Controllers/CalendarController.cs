using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CalendarAPI.Services.Repository;
using CalendarAPI.Services.Logger;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CalendarAPI.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IEventRepository _eventRepository;

        public CalendarController(IEventRepository eventRepository,
                                  ILoggerManager logger,
                                  IMapper mapper)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Calendar
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEventsByDate(DateOnly date)
        {
            try
            {
                var userId = GetCurrentUserId();

                if (userId == null)
                {
                    _logger.LogInfo($"Failed getting current user.");
                    return StatusCode(500, "Internal server error");                  
                }

                _logger.LogInfo($"Got Id of current user: {userId} .");

                var allEvents = await _eventRepository.GetEventsAsync();
                var events = allEvents.Where(ev => ev.UserId == userId
                                                && DateOnly.FromDateTime(ev.BeginTime) == date).ToListAsync();

                var eventsResult = _mapper.Map<IEnumerable<EventDto>>(events);

                _logger.LogInfo($"Returned events from database for user: {userId} and with date: {date}.");
                return Ok(eventsResult);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllEvents action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/Calendar/5
        [HttpGet("{eventId}")]
        public ActionResult<EventDto> GetOneEvent(string eventId)
        {
            try
            {
                var userId = GetCurrentUserId();

                if (userId == null)
                {
                    _logger.LogInfo($"Failed getting current user.");
                    return StatusCode(500, "Internal server error");
                }

                _logger.LogInfo($"Got Id of current user: {userId} .");

                var currEvent = await _eventRepository.GetEventByIDAsync(eventId);

                if (currEvent == null)
                {
                    _logger.LogError($"Event with id: {eventId}, hasn't been found in db.");
                    return NotFound($"Event with id: {eventId}, hasn't been found in db.");
                }

                if (currEvent.UserId != userId)
                {
                    _logger.LogInfo($"Id of current user: {userId} and Event's userId: {currEvent.UserId} are diffrent.");
                    return StatusCode(403, "Forbidden, Current user id and Event's userId are diffrent.");
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

        // POST api/Calendar
        [HttpPost]
        public ActionResult AddEvent([FromBody] EventDto eventDto)
        {
            try
            {             
                var userId = GetCurrentUserId();

                if (userId == null)
                {
                    _logger.LogInfo($"Failed getting current user.");
                    return StatusCode(500, "Internal server error");
                }

                _logger.LogInfo($"Got Id of current user: {userId} .");

                var currEvent = _mapper.Map<Event>(eventDto);

                if (currEvent.UserId != userId)
                {
                    _logger.LogInfo($"Id of current user: {userId} and Event's userId: {currEvent.UserId} are diffrent.");
                    return StatusCode(403, "Forbidden, Current user id and Event's userId are diffrent.");
                }

                bool isAdded = _eventRepository.AddEvent(currEvent);

                if (!isAdded)
                {
                    _logger.LogError($"Event with id : {currEvent.Id} already exists.");
                    return BadRequest($"Event with id : {currEvent.Id} already exists.");
                }

                _logger.LogInfo($"Event with id : {currEvent.Id} created.");

                return CreatedAtAction(nameof(AddEvent), new { id = currEvent.Id }, currEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside AddEvent action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/Calendar/5
        [HttpPut]
        public ActionResult PutEvent([FromBody] EventDto eventDto)
        {
            try
            {
                var userId = GetCurrentUserId();

                if (userId == null)
                {
                    _logger.LogInfo($"Failed getting current user.");
                    return StatusCode(500, "Internal server error");
                }

                _logger.LogInfo($"Got Id of current user: {userId} .");

                var currEvent = _mapper.Map<Event>(eventDto);

                if (currEvent.UserId != userId)
                {
                    _logger.LogInfo($"Id of current user: {userId} and Event's userId: {currEvent.UserId} are diffrent.");
                    return StatusCode(403, "Forbidden, Current user id and Event's userId are diffrent.");
                }

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

        // DELETE api/Calendar/5
        [HttpDelete("{eventId}")]
        public async Task<ActionResult> DeleteEvent(string eventId)
        {
            try
            {
                var userId = GetCurrentUserId();

                if (userId == null)
                {
                    _logger.LogInfo($"Failed getting current user.");
                    return StatusCode(500, "Internal server error");
                }

                _logger.LogInfo($"Got Id of current user: {userId} .");

                var currEvent = await _eventRepository.GetEventByIDAsync(eventId);

                if (currEvent == null)
                {
                    _logger.LogError($"Event with id: {eventId}, hasn't been found in db.");
                    return NotFound($"Event with id: {eventId}, hasn't been found in db.");
                }

                if (currEvent.UserId != userId)
                {
                    _logger.LogInfo($"Id of current user: {userId} and Event's userId: {currEvent.UserId} are diffrent.");
                    return StatusCode(403, "Forbidden, Current user id and Event's userId are diffrent.");
                }

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



        private Guid? GetCurrentUserId()
        {
            var userIdString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdString != null)
            {
                return new Guid(userIdString);
            }
            else
            {
                return null;
            }
        }
    }
}
