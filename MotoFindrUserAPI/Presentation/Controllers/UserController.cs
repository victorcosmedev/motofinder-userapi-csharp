using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Utils.Doc;
using MotoFindrUserAPI.Utils.Samples.User;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MotoFindrUserAPI.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
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

        if(!result.IsSuccess)
        {
            return StatusCode(result.StatusCode, result.Error);
        }

        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jtw:Key"]!.ToString());

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
}
