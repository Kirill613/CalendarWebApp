namespace WeatherAPI.Services.Models
{
    public class WeatherDataDto
    {
        public List<WeatherListDto> list { get; set; }     
    }
    public class WeatherListDto
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
