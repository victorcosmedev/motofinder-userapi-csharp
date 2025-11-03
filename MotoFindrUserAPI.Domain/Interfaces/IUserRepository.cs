using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> AuthenticateAsync(string username, string password);
        Task<UserEntity> CreateUserAsync(UserEntity user);
        Task<bool> ExistsByUsernameOrEmailAsync(string username);
    }
}
