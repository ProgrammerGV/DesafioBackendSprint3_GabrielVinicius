using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackendSprint3_GabrielVinicius.Data;
using DesafioBackendSprint3_GabrielVinicius.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

    [HttpGet("minha-conta")]
    [Authorize]
    public async Task<IActionResult> GetMinhaConta()
    {
        var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(usuarioIdString)) return Unauthorized();

        var conta = await _context.ContasBancarias
            .FirstOrDefaultAsync(c => c.UsuarioId == int.Parse(usuarioIdString));

        if (conta == null) return NotFound();

        return Ok(new
        {
            numeroConta = conta.NumeroConta,
            saldo = conta.Saldo,
            tipoConta = conta.TipoConta,
            limiteEmprestimo = (conta is ContaEmpresarial emp) ? emp.LimiteEmprestimo - emp.LimiteEmprestimoEmUso : 0,
            ehEmpresarial = conta is ContaEmpresarial,
            ehPoupanca = conta is ContaPoupanca,
            ehCorrente = conta is ContaCorrente
        });
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

    [HttpPost("emprestimo")]
    [Authorize]
    public async Task<IActionResult> SolicitarEmprestimo([FromBody] DTOs.TransacaoDto dto)
    {
        var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var conta = await _context.ContasBancarias
            .FirstOrDefaultAsync(c => c.UsuarioId == int.Parse(usuarioIdString!));

        if (conta is ContaEmpresarial contaEmp)
        {
            try
            {
                contaEmp.SolicitarEmprestimo(dto.Valor);
                await _context.SaveChangesAsync();
                return Ok(new { mensagem = "Empréstimo liberado com sucesso!" });
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        return BadRequest("Esta operação só é permitida para Contas Empresariais.");
    }

    [HttpPost("rendimento")]
    [Authorize]
    public async Task<IActionResult> AplicarRendimento()
    {
        var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var conta = await _context.ContasBancarias
            .FirstOrDefaultAsync(c => c.UsuarioId == int.Parse(usuarioIdString!));

        if (conta is ContaPoupanca contaPoupanca)
        {
            try
            {
                contaPoupanca.AplicarRendimento(0.01m);

                await _context.SaveChangesAsync();
                return Ok(new { mensagem = "Simulação concluída! Rendimento de 1% aplicado ao seu saldo." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return BadRequest("Esta operação só é permitida para Contas Poupança.");
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
    [HttpPost("depositar")]
    [Authorize]
    public async Task<IActionResult> Depositar([FromBody] DTOs.TransacaoDto dto)
    {
        var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(usuarioIdString)) return Unauthorized();

        var conta = await _context.ContasBancarias.FirstOrDefaultAsync(c => c.UsuarioId == int.Parse(usuarioIdString));
        if (conta == null) return NotFound("Conta não encontrada.");

        try
        {
            conta.Depositar(dto.Valor);
            var transacao = new Models.Transacao
            {
                ContaBancariaId = conta.NumeroConta,
                Tipo = "Depósito",
                Valor = dto.Valor
            };
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Depósito realizado com sucesso!", novoSaldo = conta.Saldo });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("sacar")]
    [Authorize]
    public async Task<IActionResult> Sacar([FromBody] DTOs.TransacaoDto dto)
    {
        var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(usuarioIdString)) return Unauthorized();

        var conta = await _context.ContasBancarias.FirstOrDefaultAsync(c => c.UsuarioId == int.Parse(usuarioIdString));
        if (conta == null) return NotFound("Conta não encontrada.");

        try
        {
            conta.Sacar(dto.Valor);
            var transacao = new Models.Transacao
            {
                ContaBancariaId = conta.NumeroConta,
                Tipo = "Saque",
                Valor = dto.Valor
            };
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Saque realizado com sucesso!", novoSaldo = conta.Saldo });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("todas")]
    [Authorize]
    public async Task<IActionResult> GetTodasAsContas()
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (role != "Admin") return Forbid("Acesso negado.");

        var todasAsContas = await _context.ContasBancarias
            .Include(c => c.Usuario)
            .Where(c => c.Usuario!.Role == "Cliente")
            .Select(c => new
            {
                numeroConta = c.NumeroConta,
                titular = c.Usuario!.Nome,
                cpf = c.Usuario.Cpf,
                saldo = c.Saldo,
                tipoConta = c.TipoConta
            })
            .ToListAsync();

        return Ok(todasAsContas);
    }

    [HttpPut("{numeroConta}/titular")]
    [Authorize]
    public async Task<IActionResult> AtualizarTitular(int numeroConta, [FromBody] DTOs.AtualizarUsuarioDto dto)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        if (role != "Admin") return Forbid("Acesso negado. Apenas administradores podem editar.");

        var conta = await _context.ContasBancarias
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.NumeroConta == numeroConta);

        if (conta == null) return NotFound("Conta não encontrada.");

        conta.Usuario!.Nome = dto.Nome;
        conta.Usuario.Cpf = dto.Cpf;

        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Dados do titular atualizados com sucesso!" });
    }
    [HttpPost("transferir")]
    [Authorize]
    public async Task<IActionResult> Transferir([FromBody] DTOs.TransferenciaDto dto)
    {
        var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var contaOrigem = await _context.ContasBancarias.FirstOrDefaultAsync(c => c.UsuarioId == int.Parse(usuarioIdString!));

        if (contaOrigem == null) return NotFound("Sua conta não foi encontrada.");

        var contaDestino = await _context.ContasBancarias
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Usuario!.Cpf == dto.CpfDestino);

        if (contaDestino == null) return NotFound("Não encontramos nenhuma conta com este CPF.");

        if (contaDestino.Usuario!.Role == "Admin")
            return BadRequest("Não é permitido realizar transferências para contas corporativas/administrativas.");

        if (contaOrigem.NumeroConta == contaDestino.NumeroConta)
            return BadRequest("Você não pode transferir para si mesmo.");

        try
        {
            contaOrigem.Sacar(dto.Valor);
            contaDestino.Depositar(dto.Valor);
            _context.Transacoes.Add(new Models.Transacao
            {
                ContaBancariaId = contaOrigem.NumeroConta,
                Tipo = $"Transferência Enviada (Para: {contaDestino.Usuario!.Nome})",
                Valor = dto.Valor
            });

            _context.Transacoes.Add(new Models.Transacao
            {
                ContaBancariaId = contaDestino.NumeroConta,
                Tipo = $"Transferência Recebida (De: {contaOrigem.Usuario!.Nome})",
                Valor = dto.Valor
            });

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Transferência de R$ {dto.Valor} realizada com sucesso para {contaDestino.Usuario!.Nome}!" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("extrato")]
    [Authorize]
    public async Task<IActionResult> GetExtrato()
    {
        var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var conta = await _context.ContasBancarias.FirstOrDefaultAsync(c => c.UsuarioId == int.Parse(usuarioIdString!));

        if (conta == null) return NotFound("Conta não encontrada.");

        var transacoes = await _context.Transacoes
            .Where(t => t.ContaBancariaId == conta.NumeroConta)
            .OrderByDescending(t => t.Data)
            .Select(t => new
            {
                data = t.Data.ToString("dd/MM/yyyy HH:mm"), 
                tipo = t.Tipo,
                valor = t.Valor
            })
            .ToListAsync();

        return Ok(transacoes);
    }

}