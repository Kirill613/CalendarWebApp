using Newtonsoft.Json;
using WeatherAPI.Services.Models;

namespace WeatherAPI.Services.Services
{
    public class WeatherService : IWeatherService
    {
        private HttpClient _httpClient;
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherData> CityAsync(string city)
        {
            WeatherData rawWeather = new WeatherData();

            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org");

            string apiKey = "91c57af0a890e5643306f5a5cd09a7e3";
            double latitude = 53.893009;
            double longitude = 27.567444;
            
            var response = await _httpClient.GetAsync($"/data/2.5/weather?lat={latitude}&lon={longitude}&appid={apiKey}");
            response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WeatherData>(stringResult);
        }

    }
}
