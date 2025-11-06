using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Models.Hateoas;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.User
{
    public class UserResponseListSample : IExamplesProvider<HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<UserResponseDto>>>>>
    {
        public HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<UserResponseDto>>>> GetExamples()
        {
            return new HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<UserResponseDto>>>>
            {
                Data = new PageResultModel<IEnumerable<HateoasResponse<UserResponseDto>>>
                {
                    Items = new List<HateoasResponse<UserResponseDto>>
                    {
                        new HateoasResponse<UserResponseDto>
                        {
                            Data = new UserResponseDto { Id = 1, Username = "admin" },
                            Links = new List<LinkDto> { new LinkDto { Rel = "self", Href = "", Method = "GET" } }
                        },
                        new HateoasResponse<UserResponseDto>
                        {
                            Data = new UserResponseDto { Id = 2, Username = "usuario_teste" },
                            Links = new List<LinkDto> { new LinkDto { Rel = "self", Href = "", Method = "GET" } }
                        }
                    },
                    TotalItens = 2,
                    NumeroPagina = 1,
                    TamanhoPagina = 10
                },
                Links = new List<LinkDto>
                {
                    new LinkDto { Rel = "self", Href = "/api/v1/User?pageNumber=1&pageSize=10", Method = "GET" },
                    new LinkDto { Rel = "create", Href = "/api/v1/User/register", Method = "POST" }
                }
            };
        }
    }
}
