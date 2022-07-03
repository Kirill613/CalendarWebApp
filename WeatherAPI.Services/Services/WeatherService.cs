using AutoMapper;
using Newtonsoft.Json;
using WeatherAPI.Services.Models;

namespace WeatherAPI.Services.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        private readonly string apiKey = "91c57af0a890e5643306f5a5cd09a7e3";
        private readonly Uri baseAddress = new Uri("https://api.openweathermap.org");
        private readonly int cnt = 40;

        public WeatherService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }
        public async Task<OneDayForecastDto> GetForecast(double latitude, double longitude, int dt) 
        {
            _httpClient.BaseAddress = baseAddress;

            var response = await _httpClient.GetStringAsync($"/data/2.5/forecast?lat={latitude}" +
                                                                        $"&lon={longitude}" +
                                                                        $"&appid={apiKey}" +
                                                                        $"&cnt={cnt}");

            var weatherData = JsonConvert.DeserializeObject<WeatherData>(response);

            var weatherDataResult = _mapper.Map<WeatherDataDto>(weatherData);

            return FindClosestTemp(weatherDataResult.list, dt);
        }

        private OneDayForecastDto FindClosestTemp(List<OneDayForecastDto> resTemperatures, int dt)
        {
            int min = 0;
            var dtMinTime = new OneDayForecastDto();
            foreach (var item in resTemperatures)
            {
                int result = Math.Abs(dt - item.dt);

                if (result <= min || min == 0)
                {
                    min = result;
                    dtMinTime = item;
                }
            }
            return dtMinTime;
        }      
    }
}
