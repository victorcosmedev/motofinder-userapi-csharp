using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
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
    public async Task<IActionResult> Register([FromBody] UserDto request)
    {
        try
        {
            await _userService.RegisterAsync(request);
            return Ok(new { message = "Usuário criado com sucesso!" });
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

        return StatusCode(result.StatusCode, new
        {
            user = result.Value.Username,
            token = tokenHandler.WriteToken(token),
        });
    }
}
