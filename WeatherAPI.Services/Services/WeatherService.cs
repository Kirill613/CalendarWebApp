using Newtonsoft.Json;
using WeatherAPI.Services.Models;

namespace WeatherAPI.Services.Services
{
    public class WeatherService : IWeatherService
    {
        private HttpClient _httpClient;
        private readonly string apiKey = "91c57af0a890e5643306f5a5cd09a7e3";
        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<WeatherData> GetForecast(double latitude, double longitude, int cnt) 
        {
            _httpClient.BaseAddress = new Uri("https://api.openweathermap.org");

            var response = await _httpClient.GetStringAsync($"/data/2.5/forecast?lat={latitude}" +
                                                                        $"&lon={longitude}" +
                                                                        $"&appid={apiKey}" +
                                                                        $"&cnt={cnt}");

            return JsonConvert.DeserializeObject<WeatherData>(response);
        }
    }
}
