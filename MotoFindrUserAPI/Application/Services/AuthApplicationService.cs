using Microsoft.IdentityModel.Tokens;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static BCrypt.Net.BCrypt;

namespace MotoFindrUserAPI.Application.Services
{
    public class AuthApplicationService : IAuthApplicationService
    {
        private readonly IAuthRepository _authRepository;
        public AuthApplicationService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<string?> LoginAsync(UserLoginDto loginDto)
        {
            var user = await _authRepository.GetUserByUsernameAsync(loginDto.Username);
            if(user == null)
            {
                return null;
            }

            bool isPasswordValid = Verify(loginDto.Password, user.PasswordHash);
            if(!isPasswordValid)
            {
                return null;
            }
            
            return GenerateJwtToken(user.Username);
        }

        public async Task RegisterAsync(UserRegisterDto registerDto)
        {
            var userExists = await _authRepository.ExistsByUsernameOrEmailAsync(registerDto.Username, registerDto.Email);
            if (userExists)
            {
                throw new InvalidOperationException("Usuário ou email já existe.");
            }

            string passwordHash = HashPassword(registerDto.Password);

            var user = new UserEntity 
            { 
                Username = registerDto.Username, 
                Email = registerDto.Email, 
                PasswordHash = passwordHash 
            };

            await _authRepository.CreateUserAsync(user, registerDto.Password);
        }

        #region Helpers
        private string GenerateJwtToken(string username)
        {
            var jwtKey = "B368E2721B7B7C4EF75D3CFA44F9F";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var issuer = "MotoFindrUserAPI";
            var audience = "MotoFindrUserAPI";

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
