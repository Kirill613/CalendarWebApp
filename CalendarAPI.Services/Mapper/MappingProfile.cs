using AutoMapper;
using CalendarAPI.Services.Models;

namespace CalendarAPI.Services.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
        }
    }
}
