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
                .ReverseMap();

            // MotoqueiroEntity <-> MotoqueiroDTO
            CreateMap<MotoqueiroEntity, MotoqueiroDTO>()
                .ForMember(dest => dest.MotoId,
                          opt => opt.MapFrom(src => src.Moto != null ? src.Moto.Id : (int?)null))
                .ReverseMap();
        }
    }
}
