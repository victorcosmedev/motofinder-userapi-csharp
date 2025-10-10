using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infrastructure.Data.AppData;

namespace MotoFindrUserAPI.Infrastructure.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationContext _context;

        public AuthRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> CreateUserAsync(UserEntity user, string password)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<bool> ExistsByUsernameOrEmailAsync(string username, string email)
        {
            return _context.User
                .AnyAsync(u => u.Username == username || u.Email == email);
        }

        public Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return _context.User.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
