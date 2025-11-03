using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.User
{
    public class LoginResponseSample : IExamplesProvider<LoginResponseDto>
    {
        public LoginResponseDto GetExamples()
        {
            return new LoginResponseDto
            {
                User = "exampleUser",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZXhhbXBsZVVzZXIiLCJleHAiOjE3MzAyMDUyMTh9.exemploDeTokenJwt"
            };
        }
    }
}
