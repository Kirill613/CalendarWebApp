using AutoMapper;
using WeatherAPI.Services.Models;

namespace WeatherAPI.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WeatherData, WeatherDataDto>();
            CreateMap<WeatherList, WeatherListDto>();
            CreateMap<Weather, WeatherDto>();
            CreateMap<Main, MainDto>();
        }
    }
}
