using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using System.Net;
using System.Net.Http.Headers;

namespace MotoFindrUserAPI.Application.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserApplicationService(IUserRepository authRepository, IMapper mapper)
        {
            _userRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDto?>> AuthenticateAsync(UserDto userDto)
        {
            try
            {
                var userAuth = await _userRepository.AuthenticateAsync(userDto.Username, userDto.Password);
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
                var userExists = await _userRepository.ExistsByUsernameOrEmailAsync(registerDto.Username);
                if (userExists)
                {
                    throw new InvalidOperationException("Usuário ou email já existe.");
                }

                var user = new UserEntity
                {
                    Username = registerDto.Username,
                    PasswordHash = registerDto.Password
                };

                var entity = await _userRepository.CreateUserAsync(user);
                var dto = _mapper.Map<UserDto?>(entity);

                return OperationResult<UserDto?>.Success(dto);
            }
            catch(Exception ex)
            {
                return OperationResult<UserDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }

        }

        public async Task<OperationResult<UserDto?>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var entity = await _userRepository.GetUserByUsernameAsync(username);

                if (entity is null) return OperationResult<UserDto?>.Failure("Usuário não existe", (int)HttpStatusCode.BadRequest);

                var dto = _mapper.Map<UserDto?>(entity);
                return OperationResult<UserDto?>.Success(dto);
            }
            catch(Exception ex)
            {
                return OperationResult<UserDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<UserResponseDto>>>> ObterTodos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pageResult = await _userRepository.BuscarTodos(pageNumber, pageSize);

                var dtos = _mapper.Map<IEnumerable<UserResponseDto>>(pageResult.Items);

                var pageResultDto = new PageResultModel<IEnumerable<UserResponseDto>>
                {
                    Items = dtos,
                    TotalItens = pageResult.TotalItens,
                    NumeroPagina = pageResult.NumeroPagina,
                    TamanhoPagina = pageResult.TamanhoPagina
                };

                return OperationResult<PageResultModel<IEnumerable<UserResponseDto>>>.Success(pageResultDto);
            }
            catch (Exception ex)
            {
                return OperationResult<PageResultModel<IEnumerable<UserResponseDto>>>.Failure($"Erro ao obter usuários: {ex.Message}", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
