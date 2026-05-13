namespace DesafioBackendSprint3_GabrielVinicius.Interfaces;

using DesafioBackendSprint3_GabrielVinicius.Models;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByLoginAndSenhaAsync(string login, string senha);
    Task<bool> ExistsByLoginAsync(string login);
    Task<bool> ExistsByCpfAsync(string cpf);
    Task AddAsync(Usuario usuario);
    Task SaveChangesAsync();
}

public interface IContaRepository
{
    Task<IEnumerable<ContaBancaria>> GetAllAsync();
    Task<IEnumerable<object>> GetAllClientesAsync();
    Task<ContaBancaria?> GetByUsuarioIdAsync(int usuarioId);
    Task<ContaBancaria?> GetByNumeroContaAsync(int numeroConta);
    Task<ContaBancaria?> GetByCpfAsync(string cpf);
    Task AddAsync(ContaBancaria conta);
    Task DeleteAsync(ContaBancaria conta);
    Task AddTransacaoAsync(Transacao transacao);
    Task<IEnumerable<object>> GetExtratoAsync(int numeroConta);
    Task SaveChangesAsync();
}