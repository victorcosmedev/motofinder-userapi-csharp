using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Application.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MotoEntity, MotoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Motoqueiro,
             opt => opt.Ignore()); 

           
            CreateMap<MotoqueiroEntity, MotoqueiroDto>()
                .ForMember(dest => dest.MotoId,
                          opt => opt.MapFrom(src => src.MotoId)) 
                .ReverseMap()
                .ForMember(dest => dest.MotoId,
                          opt => opt.MapFrom(src => src.MotoId ?? 0)) 
                .ForPath(dest => dest.Moto,
                        opt => opt.Ignore());

            CreateMap<EnderecoEntity, EnderecoDto>()
            .ReverseMap()
            .ForMember(dest => dest.Motoqueiro,
                opt => opt.Ignore());

            CreateMap<UserDto, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<UserEntity, UserDto>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash));

            CreateMap<UserEntity, UserResponseDto>();
        }
    }
}
