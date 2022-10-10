using WeatherAPI.Services.Models;

namespace WeatherAPI.Services.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> GetForecast(double latitude, double longitude, int cnt);
    }
}