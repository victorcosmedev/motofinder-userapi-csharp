using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MotoEntity, MotoDTO>()
                .ReverseMap()
                .ForMember(dest => dest.Motoqueiro,
             opt => opt.Ignore()); 

           
            CreateMap<MotoqueiroEntity, MotoqueiroDTO>()
                .ForMember(dest => dest.MotoId,
                          opt => opt.MapFrom(src => src.MotoId)) 
                .ReverseMap()
                .ForMember(dest => dest.MotoId,
                          opt => opt.MapFrom(src => src.MotoId ?? 0)) 
                .ForPath(dest => dest.Moto,
                        opt => opt.Ignore());

            CreateMap<EnderecoEntity, EnderecoDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Motoqueiro,
                opt => opt.Ignore());
        }
    }
}
