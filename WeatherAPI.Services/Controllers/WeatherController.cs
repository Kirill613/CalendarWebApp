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

        // GET: api/Weather
        [HttpGet]
        public async Task<IActionResult> GetWeatherForecast(double lat, double lon, int cnt)
        {
            /*            
            double latitude = 53.893009;
            double longitude = 27.567444;
            int cnt = 8;
            */

            try
            {
                var rawWeather = await _weatherService.GetForecast(lat, lon, cnt);
                return Ok(rawWeather);
            }
            catch (HttpRequestException httpRequestException)
            {
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }

    }
}
