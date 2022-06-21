using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherAPI.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private IWeatherService _weatherService;

        public WeatherController(IWeatherService wetherService)
        {
            _weatherService = wetherService;
        }

        // GET: api/<WeatherController>/Minsk
        [HttpGet("{city}")]
        public async Task<IActionResult> City(string city)
        {
            try
            {
                var rawWeather = await _weatherService.CityAsync(city);
                return Ok(rawWeather);
            }
            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }

    }
}
