using Microsoft.AspNetCore.Mvc;
using CalendarAPI.Services.Repository;
using System.Transactions;

namespace CalendarAPI.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public CalendarController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/<CalendarController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventRepository.GetEventsAsync();
            return Ok(events);
        }

        // GET api/<CalendarController>/5
        [HttpGet("{eventId}")]
        public async Task<ActionResult<Event>> GetOneEvent(string eventId)
        {
            var currEvent = await _eventRepository.GetEventByIDAsync(eventId);

            if (currEvent == null)
                return NotFound("There is no entity with this ID");

            return Ok(currEvent);
        }

        // POST api/<CalendarController>
        [HttpPost]
        public async Task<ActionResult> AddEvent([FromBody] Event currEvent)
        {
            bool isAdded = _eventRepository.AddEvent(currEvent);

            if (!isAdded)
                return BadRequest("Event with the same ID already exists");

            //return CreatedAtAction(nameof(GetOneEvent), new { id = currEvent.Id }, currEvent);
            return Ok();
        }

        // PUT api/<CalendarController>/5
        [HttpPut]
        public async Task<ActionResult> PutEvent([FromBody] Event currEvent)
        {
            bool isUpdated = _eventRepository.UpdateEvent(currEvent);

            if (!isUpdated)
                return NotFound("There is no entity with this ID");

            return Ok();
        }

        // DELETE api/<CalendarController>/5
        [HttpDelete("{eventId}")]
        public async Task<ActionResult> Delete(string eventId)
        {
            bool isDeleted = _eventRepository.DeleteEvent(eventId);

            if (!isDeleted)
                return NotFound("There is no entity with this ID");

            return Ok();
        }
    }
}
