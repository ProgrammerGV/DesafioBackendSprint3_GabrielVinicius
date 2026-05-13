using System.Security.Claims;
using DesafioBackendSprint3_GabrielVinicius.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackendSprint3_GabrielVinicius.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ContasController : ControllerBase
{
    private readonly IContaService _contaService;

    public ContasController(IContaService contaService) { _contaService = contaService; }

    private int GetUsuarioId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet("minha-conta")]
    public async Task<IActionResult> GetMinhaConta()
    {
        try { return Ok(await _contaService.GetMinhaContaAsync(GetUsuarioId())); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet("todas")]
    public async Task<IActionResult> GetTodasAsContas()
    {
        if (User.FindFirst(ClaimTypes.Role)?.Value != "Admin") return Forbid("Acesso negado.");
        return Ok(await _contaService.GetTodasAsContasAsync());
    }

    [HttpPost("depositar")]
    public async Task<IActionResult> Depositar([FromBody] DTOs.TransacaoDto dto)
    {
        try { await _contaService.DepositarAsync(GetUsuarioId(), dto.Valor); return Ok(new { mensagem = "Depósito realizado com sucesso!" }); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("sacar")]
    public async Task<IActionResult> Sacar([FromBody] DTOs.TransacaoDto dto)
    {
        try { await _contaService.SacarAsync(GetUsuarioId(), dto.Valor); return Ok(new { mensagem = "Saque realizado com sucesso!" }); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("transferir")]
    public async Task<IActionResult> Transferir([FromBody] DTOs.TransferenciaDto dto)
    {
        try { await _contaService.TransferirAsync(GetUsuarioId(), dto); return Ok(new { mensagem = "Transferência realizada com sucesso!" }); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("emprestimo")]
    public async Task<IActionResult> SolicitarEmprestimo([FromBody] DTOs.TransacaoDto dto)
    {
        try { await _contaService.SolicitarEmprestimoAsync(GetUsuarioId(), dto.Valor); return Ok(new { mensagem = "Empréstimo liberado com sucesso!" }); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("rendimento")]
    public async Task<IActionResult> AplicarRendimento()
    {
        try { await _contaService.AplicarRendimentoAsync(GetUsuarioId()); return Ok(new { mensagem = "Rendimento aplicado com sucesso!" }); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{numeroConta}/titular")]
    public async Task<IActionResult> AtualizarTitular(int numeroConta, [FromBody] DTOs.AtualizarUsuarioDto dto)
    {
        if (User.FindFirst(ClaimTypes.Role)?.Value != "Admin") return Forbid("Acesso negado.");
        try { await _contaService.AtualizarTitularAsync(numeroConta, dto); return Ok(new { mensagem = "Titular atualizado com sucesso!" }); }
        catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConta(int id)
    {
        if (User.FindFirst(ClaimTypes.Role)?.Value != "Admin") return Forbid("Acesso negado.");
        try { await _contaService.DeletarContaAsync(id); return NoContent(); }
        catch (KeyNotFoundException) { return NotFound("Conta não encontrada."); }
    }

    [HttpGet("extrato")]
    public async Task<IActionResult> GetExtrato()
    {
        try { return Ok(await _contaService.GetExtratoAsync(GetUsuarioId())); }
        catch (KeyNotFoundException) { return NotFound("Conta não encontrada."); }
    }
}