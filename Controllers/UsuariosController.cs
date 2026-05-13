using DesafioBackendSprint3_GabrielVinicius.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackendSprint3_GabrielVinicius.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService) { _usuarioService = usuarioService; }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] DTOs.RegistrarUsuarioDto dto)
    {
        try
        {
            await _usuarioService.RegistrarAsync(dto);
            return Ok(new { mensagem = "Conta e usuário criados com sucesso!" });
        }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }
}