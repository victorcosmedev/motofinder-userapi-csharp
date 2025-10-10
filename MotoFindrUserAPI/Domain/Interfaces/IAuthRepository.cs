using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserEntity?> GetUserByUsernameAsync(string username);
        Task<UserEntity> CreateUserAsync(UserEntity user, string password);
        Task<bool> ExistsByUsernameOrEmailAsync(string username, string email);
    }
}
