using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackendSprint3_GabrielVinicius.Data;
using DesafioBackendSprint3_GabrielVinicius.Models;

namespace DesafioBackendSprint3_GabrielVinicius.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] DTOs.RegistrarUsuarioDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Login == dto.Usuario))
            return BadRequest("Este nome de usuário já está em uso.");

        if (await _context.Usuarios.AnyAsync(u => u.Cpf == dto.Cpf))
            return BadRequest("Este CPF já está cadastrado no sistema.");

        var novoUsuario = new Usuario
        {
            Nome = dto.Nome,
            Login = dto.Usuario,
            Senha = dto.Senha,
            Cpf = dto.Cpf
        };


        ContaBancaria novaConta = dto.TipoContaInicial switch
        {
            "2" => new ContaPoupanca(0, dto.Nome, 0, "Poupança"),
            "3" => new ContaEmpresarial(0, dto.Nome, 0, "Empresarial"),
            _ => new ContaCorrente(0, dto.Nome, 0, "Corrente") 
        };

        novaConta.Usuario = novoUsuario;
        novoUsuario.Contas.Add(novaConta);

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Conta e usuário criados com sucesso!" });
    }
}