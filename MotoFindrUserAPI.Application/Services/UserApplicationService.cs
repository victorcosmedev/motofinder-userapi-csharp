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

        public async Task<OperationResult<UserEntity?>> AuthenticateAsync(UserDto userDto)
        {
            try
            {
                var userAuth = await _authRepository.AuthenticateAsync(userDto.Username, userDto.Password);

                return OperationResult<UserEntity?>.Success(userAuth);
            }
            catch (Exception)
            {
                return OperationResult<UserEntity?>.Failure("Ocorreu um erro ao buscar o cliente");
            }
        }

        public async Task RegisterAsync(UserDto registerDto)
        {
            var userExists = await _authRepository.ExistsByUsernameOrEmailAsync(registerDto.Username);
            if (userExists)
            {
                throw new InvalidOperationException("Usuário ou email já existe.");
            }

            var user = new UserEntity 
            { 
                Username = registerDto.Username, 
                PasswordHash = registerDto.Password 
            };

            await _authRepository.CreateUserAsync(user);
        }
    }
}
