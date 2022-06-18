using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MapAPI.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        // GET: api/<MapController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "map1", "map2" };
        }

        // GET api/<MapController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "map";
        }

        // POST api/<MapController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MapController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MapController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
