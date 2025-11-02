using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<string?> AuthenticateAsync(string username, string password);
        Task<UserEntity> CreateUserAsync(UserEntity user, string password);
        Task<bool> ExistsByUsernameOrEmailAsync(string username, string email);
    }
}
