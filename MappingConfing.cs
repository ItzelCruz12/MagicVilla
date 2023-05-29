using AutoMapper;
using MagicVillaApi.Models.Dto;

namespace MagicVillaApi
{
    public class MappingConfing : Profile
    {
        public MappingConfing()
        {
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();



        }
    }
}
