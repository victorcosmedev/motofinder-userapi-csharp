using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infrastructure.Data.AppData;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace MotoFindrUserAPI.Infrastructure.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
            if(user == null)
            {
                return null;
            }

            if(!Verify(password, user.PasswordHash))
            {
                return null;
            }

            return GenerateJwt
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
