using DesafioBackendSprint3_GabrielVinicius.Data;
using DesafioBackendSprint3_GabrielVinicius.Interfaces;
using DesafioBackendSprint3_GabrielVinicius.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendSprint3_GabrielVinicius.Repositories;

public class ContaRepository : IContaRepository
{
    private readonly AppDbContext _context;

    public ContaRepository(AppDbContext context) { _context = context; }

    public async Task<IEnumerable<ContaBancaria>> GetAllAsync() => await _context.ContasBancarias.ToListAsync();

    public async Task<ContaBancaria?> GetByUsuarioIdAsync(int usuarioId) =>
        await _context.ContasBancarias.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

    public async Task<ContaBancaria?> GetByNumeroContaAsync(int numeroConta) =>
        await _context.ContasBancarias.Include(c => c.Usuario).FirstOrDefaultAsync(c => c.NumeroConta == numeroConta);

    public async Task<ContaBancaria?> GetByCpfAsync(string cpf) =>
        await _context.ContasBancarias.Include(c => c.Usuario).FirstOrDefaultAsync(c => c.Usuario!.Cpf == cpf);

    public async Task<IEnumerable<object>> GetAllClientesAsync() =>
        await _context.ContasBancarias
            .Include(c => c.Usuario)
            .Where(c => c.Usuario!.Role == "Cliente")
            .Select(c => new { c.NumeroConta, titular = c.Usuario!.Nome, cpf = c.Usuario.Cpf, c.Saldo, c.TipoConta })
            .ToListAsync();

    public async Task AddAsync(ContaBancaria conta) => await _context.ContasBancarias.AddAsync(conta);

    public async Task DeleteAsync(ContaBancaria conta) => _context.ContasBancarias.Remove(conta);

    public async Task AddTransacaoAsync(Transacao transacao) => await _context.Transacoes.AddAsync(transacao);

    public async Task<IEnumerable<object>> GetExtratoAsync(int numeroConta) =>
        await _context.Transacoes
            .Where(t => t.ContaBancariaId == numeroConta)
            .OrderByDescending(t => t.Data)
            .Select(t => new { data = t.Data.ToString("dd/MM/yyyy HH:mm"), tipo = t.Tipo, valor = t.Valor })
            .ToListAsync();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}