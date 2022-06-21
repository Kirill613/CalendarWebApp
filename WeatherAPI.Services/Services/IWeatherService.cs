using WeatherAPI.Services.Models;

namespace WeatherAPI.Services.Services
{
    public interface IWeatherService
    {
        Task<WeatherData> CityAsync(string city);
    }
}
