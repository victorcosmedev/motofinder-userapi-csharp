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

        #region Helpers
        private string GenerateJwt(UserEntity user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }   

        #endregion
    }
}
