using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services.Services;
using WeatherAPI.Services.Logger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherAPI.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        private IWeatherService _weatherService;

        public WeatherController(IWeatherService wetherService, ILoggerManager logger)
        {
            _logger = logger;
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
                _logger.LogInfo($"Got weather forecast from remote api with params: latitide={lat}, lontitude={lon}, cnt={cnt}.");

                return Ok(rawWeather);
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogError($"Error getting weather from OpenWeather inside GetWeatherForecast action: {httpRequestException.Message}");
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }

    }
}
