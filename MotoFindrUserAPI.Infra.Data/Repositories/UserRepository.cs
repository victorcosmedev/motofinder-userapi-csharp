using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infra.Data.AppData;
using System.Security.Claims;
using System.Text;

namespace MotoFindrUserAPI.Infra.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == password);
            
            if(user is null)
            {
                throw new Exception("Invalid username or password");
            }

            return user;
        }

        public async Task<UserEntity> CreateUserAsync(UserEntity user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<bool> ExistsByUsernameOrEmailAsync(string username)
        {
            return _context.User
                .AnyAsync(u => u.Username == username);
        }

        public Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return _context.User.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
