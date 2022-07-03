using WeatherAPI.Services.Models;

namespace WeatherAPI.Services.Services
{
    public interface IWeatherService
    {
        Task<OneDayForecastDto> GetForecast(double latitude, double longitude, int dt);
    }
}