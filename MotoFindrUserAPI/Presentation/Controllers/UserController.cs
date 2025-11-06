using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Models.Hateoas;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using MotoFindrUserAPI.Utils.Doc;
using MotoFindrUserAPI.Utils.Samples.User;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MotoFindrUserAPI.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserApplicationService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserApplicationService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation(
            Summary = ApiDoc.RegisterUserSummary,
            Description = ApiDoc.RegisterUserDescription
        )]
    [SwaggerRequestExample(typeof(UserDto), typeof(UserRequestSample))]
    [SwaggerResponse(StatusCodes.Status200OK, "Usuário criado com sucesso", typeof(MessageResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Dados inválidos ou usuário já existente", typeof(MessageResponseDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor", typeof(MessageResponseDto))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegisterResponseSample))]
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        try
        {
            await _userService.RegisterAsync(request);
            return Ok(new MessageResponseDto { Message = "Usuário criado com sucesso!" });
        }
        catch (InvalidOperationException ex)
        {

            return BadRequest(ex.Message);
        }
        catch (Exception)
        {

            return StatusCode(500, "Ocorreu um erro interno ao registrar o usuário.");
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(
            Summary = ApiDoc.LoginUserSummary,
            Description = ApiDoc.LoginUserDescription
        )]
    [SwaggerRequestExample(typeof(UserDto), typeof(UserRequestSample))]
    [SwaggerResponse(StatusCodes.Status200OK, "Login bem-sucedido", typeof(LoginResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisição inválida", typeof(MessageResponseDto))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Credenciais inválidas", typeof(MessageResponseDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Usuário não encontrado", typeof(MessageResponseDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor", typeof(MessageResponseDto))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoginResponseSample))]
    public async Task<IActionResult> Login([FromBody] UserDto request)
    {
        var result = await _userService.AuthenticateAsync(request);

        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result.Error);
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!.ToString());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Name, result.Value!.Username.ToString()),
            })
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return StatusCode(result.StatusCode, new LoginResponseDto
        {
            User = result.Value.Username,
            Token = tokenHandler.WriteToken(token),
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserName([FromQuery] string username)
    {
        if (username is null) return StatusCode((int)HttpStatusCode.BadRequest, "Username nulo");

        var result = await _userService.GetUserByUsernameAsync(username);

        if (!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result.Error);
        }

        var hateoas = new HateoasResponse<UserDto>
        {
            Data = result.Value,
            Links = new List<LinkDto>
            {
                new LinkDto { Rel = "self", Href = Url.Action(nameof(GetByUserName), new { username }), Method = "GET" },
            }
        };

        return Ok(hateoas);
    }

    [HttpGet("buscar-todos")]
    [SwaggerOperation(
        Summary = ApiDoc.BuscarTodosUsuariosSummary,
        Description = ApiDoc.BuscarTodosUsuariosDescription
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Lista de usuários obtida com sucesso")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Não autorizado")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Erro interno do servidor")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserResponseListSample))]
    [EnableRateLimiting("rateLimitPolicy")]
    public async Task<IActionResult> BuscarTodos(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var result = await _userService.ObterTodos(pageNumber, pageSize);
            if (!result.IsSuccess) return StatusCode(result.StatusCode, result.Error);

            var pageResults = new PageResultModel<IEnumerable<HateoasResponse<UserResponseDto>>>
            {
                Items = result.Value.Items.Select(user => new HateoasResponse<UserResponseDto>
                {
                    Data = user,
                    Links = new List<LinkDto>
                {
                    new LinkDto { Rel = "self", Href = Url.Action(nameof(BuscarTodos)), Method = "GET" }
                }
                }),
                TotalItens = result.Value.TotalItens,
                NumeroPagina = result.Value.NumeroPagina,
                TamanhoPagina = result.Value.TamanhoPagina
            };

            var response = new HateoasResponse<PageResultModel<IEnumerable<HateoasResponse<UserResponseDto>>>>
            {
                Data = pageResults,
                Links = new List<LinkDto>
            {
                new LinkDto { Rel = "self", Href = Url.Action(nameof(BuscarTodos), new { pageNumber, pageSize }), Method = "GET" },
                new LinkDto { Rel = "create", Href = Url.Action(nameof(Register)), Method = "POST" }
            }
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new MessageResponseDto { Message = ex.Message });
        }
    }
}
