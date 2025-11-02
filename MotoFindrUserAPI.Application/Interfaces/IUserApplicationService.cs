using MotoFindrUserAPI.Application.DTOs;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IUserApplicationService
    {
        Task RegisterAsync(UserRegisterDto registerDto);

        Task<string> LoginAsync(UserLoginDto loginDto);
    }
}
