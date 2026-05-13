namespace DesafioBackendSprint3_GabrielVinicius.Interfaces;

using DesafioBackendSprint3_GabrielVinicius.DTOs;

public interface IAuthService
{
    Task<string?> AutenticarAsync(LoginDto loginDto);
}

public interface IUsuarioService
{
    Task RegistrarAsync(RegistrarUsuarioDto dto);
}

public interface IContaService
{
    Task<object> GetMinhaContaAsync(int usuarioId);
    Task<IEnumerable<object>> GetTodasAsContasAsync();
    Task DepositarAsync(int usuarioId, decimal valor);
    Task SacarAsync(int usuarioId, decimal valor);
    Task TransferirAsync(int usuarioOrigemId, TransferenciaDto dto);
    Task SolicitarEmprestimoAsync(int usuarioId, decimal valor);
    Task AplicarRendimentoAsync(int usuarioId);
    Task AtualizarTitularAsync(int numeroConta, AtualizarUsuarioDto dto);
    Task DeletarContaAsync(int numeroConta);
    Task<IEnumerable<object>> GetExtratoAsync(int usuarioId);
}