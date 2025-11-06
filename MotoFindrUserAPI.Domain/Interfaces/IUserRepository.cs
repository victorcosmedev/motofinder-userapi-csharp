using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> AuthenticateAsync(string username, string password);
        Task<UserEntity> CreateUserAsync(UserEntity user);
        Task<bool> ExistsByUsernameOrEmailAsync(string username);
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task<PageResultModel<IEnumerable<UserEntity>>> BuscarTodos(int pageNumber, int pageSize);
    }
}
