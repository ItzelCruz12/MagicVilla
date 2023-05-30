using AutoMapper;
using MagicVillaApi.Models;
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

            CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();





        }
    }
}
