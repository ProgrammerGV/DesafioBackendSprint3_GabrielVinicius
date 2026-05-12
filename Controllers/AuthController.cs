using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DesafioBackendSprint3_GabrielVinicius.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] DTOs.LoginDto loginDto)
    {
        if (loginDto.Usuario == "admin" && loginDto.Senha == "123456")
        {
            var token = GerarTokenJwt();
            return Ok(new { token = token });
        }

        return Unauthorized("Usuário ou senha inválidos.");
    }

    private string GerarTokenJwt()
    {
        var jwtKey = _configuration["Jwt:Key"];
        var key = Encoding.ASCII.GetBytes(jwtKey!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Gerente") 
            }),
            Expires = DateTime.UtcNow.AddHours(2), 
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token); 
    }
}