namespace WeatherAPI.Services.Models
{
    public class WeatherData
    {
        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string name { get; set; }
    }
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}
