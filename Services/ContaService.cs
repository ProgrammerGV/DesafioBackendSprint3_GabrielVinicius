using DesafioBackendSprint3_GabrielVinicius.Interfaces;
using DesafioBackendSprint3_GabrielVinicius.Models;

namespace DesafioBackendSprint3_GabrielVinicius.Services;

public class ContaService : IContaService
{
    private readonly IContaRepository _contaRepo;

    public ContaService(IContaRepository contaRepo) { _contaRepo = contaRepo; }

    public async Task<object> GetMinhaContaAsync(int usuarioId)
    {
        var conta = await _contaRepo.GetByUsuarioIdAsync(usuarioId);
        if (conta == null) throw new KeyNotFoundException("Conta não encontrada.");

        return new
        {
            numeroConta = conta.NumeroConta,
            saldo = conta.Saldo,
            tipoConta = conta.TipoConta,
            limiteEmprestimo = (conta is ContaEmpresarial emp) ? emp.LimiteEmprestimo - emp.LimiteEmprestimoEmUso : 0,
            ehEmpresarial = conta is ContaEmpresarial,
            ehPoupanca = conta is ContaPoupanca,
            ehCorrente = conta is ContaCorrente
        };
    }

    public async Task<IEnumerable<object>> GetTodasAsContasAsync() => await _contaRepo.GetAllClientesAsync();

    public async Task DepositarAsync(int usuarioId, decimal valor)
    {
        var conta = await _contaRepo.GetByUsuarioIdAsync(usuarioId) ?? throw new KeyNotFoundException("Conta não encontrada.");
        conta.Depositar(valor);
        await _contaRepo.AddTransacaoAsync(new Transacao { ContaBancariaId = conta.NumeroConta, Tipo = "Depósito", Valor = valor });
        await _contaRepo.SaveChangesAsync();
    }

    public async Task SacarAsync(int usuarioId, decimal valor)
    {
        var conta = await _contaRepo.GetByUsuarioIdAsync(usuarioId) ?? throw new KeyNotFoundException("Conta não encontrada.");
        conta.Sacar(valor);
        await _contaRepo.AddTransacaoAsync(new Transacao { ContaBancariaId = conta.NumeroConta, Tipo = "Saque", Valor = valor });
        await _contaRepo.SaveChangesAsync();
    }

    public async Task TransferirAsync(int usuarioOrigemId, DTOs.TransferenciaDto dto)
    {
        var contaOrigem = await _contaRepo.GetByUsuarioIdAsync(usuarioOrigemId) ?? throw new KeyNotFoundException("Sua conta não foi encontrada.");
        var contaDestino = await _contaRepo.GetByCpfAsync(dto.CpfDestino) ?? throw new KeyNotFoundException("Não encontramos nenhuma conta com este CPF.");

        if (contaDestino.Usuario!.Role == "Admin") throw new InvalidOperationException("Não é permitido realizar transferências para contas corporativas.");
        if (contaOrigem.NumeroConta == contaDestino.NumeroConta) throw new InvalidOperationException("Você não pode transferir para si mesmo.");

        contaOrigem.Sacar(dto.Valor);
        contaDestino.Depositar(dto.Valor);

        await _contaRepo.AddTransacaoAsync(new Transacao { ContaBancariaId = contaOrigem.NumeroConta, Tipo = $"Transferência Enviada (Para: {contaDestino.Usuario!.Nome})", Valor = dto.Valor });
        await _contaRepo.AddTransacaoAsync(new Transacao { ContaBancariaId = contaDestino.NumeroConta, Tipo = $"Transferência Recebida (De: {contaOrigem.Usuario!.Nome})", Valor = dto.Valor });

        await _contaRepo.SaveChangesAsync();
    }

    public async Task SolicitarEmprestimoAsync(int usuarioId, decimal valor)
    {
        var conta = await _contaRepo.GetByUsuarioIdAsync(usuarioId) ?? throw new KeyNotFoundException("Conta não encontrada.");
        if (conta is not ContaEmpresarial contaEmp) throw new InvalidOperationException("Operação permitida apenas para Contas Empresariais.");

        contaEmp.SolicitarEmprestimo(valor);
        await _contaRepo.SaveChangesAsync();
    }

    public async Task AplicarRendimentoAsync(int usuarioId)
    {
        var conta = await _contaRepo.GetByUsuarioIdAsync(usuarioId) ?? throw new KeyNotFoundException("Conta não encontrada.");
        if (conta is not ContaPoupanca contaPoup) throw new InvalidOperationException("Operação permitida apenas para Contas Poupança.");

        contaPoup.AplicarRendimento(0.01m);
        await _contaRepo.SaveChangesAsync();
    }

    public async Task AtualizarTitularAsync(int numeroConta, DTOs.AtualizarUsuarioDto dto)
    {
        var conta = await _contaRepo.GetByNumeroContaAsync(numeroConta) ?? throw new KeyNotFoundException("Conta não encontrada.");
        conta.Usuario!.Nome = dto.Nome;
        conta.Usuario.Cpf = dto.Cpf;
        await _contaRepo.SaveChangesAsync();
    }

    public async Task DeletarContaAsync(int numeroConta)
    {
        var conta = await _contaRepo.GetByNumeroContaAsync(numeroConta) ?? throw new KeyNotFoundException("Conta não encontrada.");
        await _contaRepo.DeleteAsync(conta);
        await _contaRepo.SaveChangesAsync();
    }

    public async Task<IEnumerable<object>> GetExtratoAsync(int usuarioId)
    {
        var conta = await _contaRepo.GetByUsuarioIdAsync(usuarioId) ?? throw new KeyNotFoundException("Conta não encontrada.");
        return await _contaRepo.GetExtratoAsync(conta.NumeroConta);
    }
}