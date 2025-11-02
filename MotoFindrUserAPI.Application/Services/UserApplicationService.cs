using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;

namespace MotoFindrUserAPI.Application.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserRepository _authRepository;
        public UserApplicationService(IUserRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<string?> LoginAsync(UserLoginDto loginDto)
        {
            return await _authRepository.AuthenticateAsync(loginDto.Username, loginDto.Password);
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
    }
}
