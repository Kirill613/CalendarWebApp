namespace WeatherAPI.Services.Models
{
    public class WeatherDataDto
    {
        public List<OneDayForecastDto> list { get; set; }     
    }
    public class OneDayForecastDto
    {
        public int dt { get; set; }
        public MainDto main { get; set; }
        public List<WeatherDto> weather { get; set; }
    }
    public class MainDto
    {
        public double temp { get; set; }
    }
    public class WeatherDto
    {
        public string description { get; set; }
        public string icon { get; set; }
    }
}
