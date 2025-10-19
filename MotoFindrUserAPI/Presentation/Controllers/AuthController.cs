using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;

namespace MotoFindrUserAPI.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthApplicationService _authService;
    public AuthController(IAuthApplicationService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
    {
        try
        {
            await _authService.RegisterAsync(request);
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

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginDto request)
    {
        var token = await _authService.LoginAsync(request);

        if (token == null)
        {
            return Unauthorized("Usuário ou senha inválidos.");
        }

        return Ok(new { token });
    }
}
