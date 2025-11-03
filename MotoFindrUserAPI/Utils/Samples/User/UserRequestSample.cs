using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.User
{
    public class UserRequestSample : IExamplesProvider<UserDto>
    {
        public UserDto GetExamples()
        {
            return new UserDto
            {
                Username = "exampleUser",
                Password = "examplePassword123"
            };
        }
    }
}
