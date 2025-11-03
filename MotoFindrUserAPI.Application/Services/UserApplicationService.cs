using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace MotoFindrUserAPI.Application.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserRepository _authRepository;
        private readonly IMapper _mapper;
        public UserApplicationService(IUserRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDto?>> AuthenticateAsync(UserDto userDto)
        {
            try
            {
                var userAuth = await _authRepository.AuthenticateAsync(userDto.Username, userDto.Password);
                var dto = _mapper.Map<UserDto?>(userAuth);

                return OperationResult<UserDto?>.Success(dto);
            }
            catch (Exception)
            {
                return OperationResult<UserDto?>.Failure("Ocorreu um erro ao buscar o cliente");
            }
        }

        public async Task<OperationResult<UserDto?>> RegisterAsync(UserDto registerDto)
        {
            try
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

                var entity = _authRepository.CreateUserAsync(user);
                var dto = _mapper.Map<UserDto?>(entity);

                return OperationResult<UserDto?>.Success(dto);
            }
            catch(Exception ex)
            {
                return OperationResult<UserDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }

        }
    }
}
