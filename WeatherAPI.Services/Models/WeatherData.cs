namespace WeatherAPI.Services.Models
{
    public class WeatherData
    {
        public int cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List<WeatherList> list { get; set; }
        public City city { get; set; }
    }
    public class WeatherList
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public List<Weather> weather { get; set; }
        public string dt_txt { get; set; }
    }
    public class Main
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public int population { get; set; }
    }

    /*public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }*/
}

/*{
  "cod": "200",
  "message": 0,
  "cnt": 40,
  "list": [
    {
      "dt": 1647345600,
      "main": {
        "temp": 286.88,
        "feels_like": 285.93,
        "temp_min": 286.74,
        "temp_max": 286.88,
        "pressure": 1021,
        "sea_level": 1021,
        "grnd_level": 1018,
        "humidity": 62,
        "temp_kf": 0.14
      },

      "clouds": {
            "all": 85
      },
      "wind": {
            "speed": 3.25,
        "deg": 134,
        "gust": 4.45
      },
      "visibility": 10000,
      "pop": 0,
      "sys": {
            "pod": "d"
      },
      "dt_txt": "2022-03-15 12:00:00"
    },   
    ...

 ],
  "city": {
    "id": 2643743,
    "name": "London",
    "coord": {
            "lat": 51.5073,
      "lon": -0.1277
    },
    "country": "GB",
    "population": 1000000,
    "timezone": 0,
    "sunrise": 1647324903,
    "sunset": 1647367441
  }
}*/
