using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Models.Hateoas;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.User
{
    public class UserResponseSample : IExamplesProvider<HateoasResponse<UserResponseDto>>
    {
        public HateoasResponse<UserResponseDto> GetExamples()
        {
            return new HateoasResponse<UserResponseDto>
            {
                Data = new UserResponseDto
                {
                    Id = 1,
                    Username = "usuario_exemplo"
                },
                Links = new List<LinkDto>
                {
                    new LinkDto { Rel = "self", Href = "/api/v1/User?username=usuario_exemplo", Method = "GET" }
                }
            };
        }
    }
}
