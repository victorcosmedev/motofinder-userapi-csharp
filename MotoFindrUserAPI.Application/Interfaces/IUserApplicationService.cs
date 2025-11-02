using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IUserApplicationService
    {
        Task<OperationResult<UserEntity?>> AuthenticateAsync(UserDto userDto);
        Task RegisterAsync(UserDto registerDto);
    }
}
