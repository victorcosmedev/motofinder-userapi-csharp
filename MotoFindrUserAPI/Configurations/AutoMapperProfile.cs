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
             opt => opt.Ignore()); // Evita sobrescrita do relacionamento

            // MotoqueiroEntity <-> MotoqueiroDTO
            CreateMap<MotoqueiroEntity, MotoqueiroDTO>()
                .ForMember(dest => dest.MotoId,
                          opt => opt.MapFrom(src => src.MotoId)) // Mapeia direto do MotoId da entity
                .ReverseMap()
                .ForMember(dest => dest.MotoId,
                          opt => opt.MapFrom(src => src.MotoId ?? 0)) // Trata null como 0
                .ForPath(dest => dest.Moto,
                        opt => opt.Ignore());
        }
    }
}
