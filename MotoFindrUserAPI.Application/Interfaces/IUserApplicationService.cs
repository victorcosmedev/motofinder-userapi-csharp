using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IUserApplicationService
    {
        Task<OperationResult<UserDto?>> AuthenticateAsync(UserDto userDto);
        Task <OperationResult<UserDto?>> RegisterAsync(UserDto registerDto);
        Task<OperationResult<UserDto?>> GetUserByUsernameAsync(string username);
        Task<OperationResult<PageResultModel<IEnumerable<UserResponseDto>>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
    }
}
