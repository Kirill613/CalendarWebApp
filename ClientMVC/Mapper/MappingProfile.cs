using AutoMapper;
using ClientMvc.Models;
using ClientMvc.ModelViews;

namespace ClientMvc.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EvCreateViewModel, EventDto>().ReverseMap();
        }
    }
}
