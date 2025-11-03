using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IUserApplicationService
    {
        Task<OperationResult<UserDto?>> AuthenticateAsync(UserDto userDto);
        Task <OperationResult<UserDto?>> RegisterAsync(UserDto registerDto);
    }
}
