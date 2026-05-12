using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackendSprint3_GabrielVinicius.Data;
using DesafioBackendSprint3_GabrielVinicius.Models;
using Microsoft.AspNetCore.Authorization;

namespace DesafioBackendSprint3_GabrielVinicius.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetContas()
    {
        var contas = await _context.ContasBancarias.ToListAsync();
        return Ok(contas);
    }

    [HttpPost]
    public async Task<IActionResult> PostConta([FromBody] DTOs.CriarContaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titular))
            return BadRequest("O nome do titular é obrigatório.");

        ContaBancaria novaConta;


        switch (dto.TipoOpcao)
        {
            case "1":
                novaConta = new ContaCorrente(0, dto.Titular, dto.SaldoInicial, "Conta Corrente");
                break;
            case "2":
                novaConta = new ContaPoupanca(0, dto.Titular, dto.SaldoInicial, "Conta Poupança");
                break;
            case "3":
                novaConta = new ContaEmpresarial(0, dto.Titular, dto.SaldoInicial, "Conta Empresarial");
                break;
            default:
                return BadRequest("Tipo de conta inválido. Use 1 para Corrente, 2 para Poupança ou 3 para Empresarial.");
        }

        _context.ContasBancarias.Add(novaConta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContas), new { id = novaConta.NumeroConta }, novaConta);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutConta(int id, [FromBody] DTOs.AtualizarContaDto dto)
    {
        var conta = await _context.ContasBancarias.FindAsync(id);

        if (conta == null)
            return NotFound("Conta não encontrada.");

        if (string.IsNullOrWhiteSpace(dto.Titular))
            return BadRequest("O nome do titular é obrigatório.");

        conta.SetTitular(dto.Titular);

        await _context.SaveChangesAsync();

        return Ok(conta);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConta(int id)
    {
        var conta = await _context.ContasBancarias.FindAsync(id);

        if (conta == null)
            return NotFound("Conta não encontrada.");

        _context.ContasBancarias.Remove(conta);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}