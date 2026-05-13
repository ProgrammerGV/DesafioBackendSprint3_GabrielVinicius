using DesafioBackendSprint3_GabrielVinicius.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackendSprint3_GabrielVinicius.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) { _authService = authService; }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DTOs.LoginDto loginDto)
    {
        var token = await _authService.AutenticarAsync(loginDto);
        if (token != null) return Ok(new { token });

        return Unauthorized("Usuário ou senha inválidos.");
    }
}