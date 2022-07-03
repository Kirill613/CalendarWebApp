using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Services.Services;
using WeatherAPI.Services.Logger;
using Microsoft.AspNetCore.Authorization;

namespace WeatherAPI.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        private ILoggerManager _logger;
        private IWeatherService _weatherService;
        public WeatherController(IWeatherService wetherService, ILoggerManager logger)
        {
            _logger = logger;
            _weatherService = wetherService;
        }

        // GET: api/Weather?lat=53.893009&lon=27.567444&dt=1647345600
        [HttpGet]
        public async Task<IActionResult> GetWeatherForecast(double lat, double lon, int dt)
        {
            try
            {
                var weatherDataResult = await _weatherService.GetForecast(lat, lon, dt);
                _logger.LogInfo($"Got weather forecast from remote api with params: latitide={lat}, lontitude={lon}, DateTime={DateTime.FromBinary(dt)}.");

                return Ok(weatherDataResult);
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogError($"Error getting weather from OpenWeather inside GetWeatherForecast action: {httpRequestException.Message}");
                return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }
    }
}
