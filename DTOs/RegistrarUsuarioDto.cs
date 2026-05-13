namespace DesafioBackendSprint3_GabrielVinicius.DTOs;

public class RegistrarUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string TipoContaInicial { get; set; } = string.Empty;
}