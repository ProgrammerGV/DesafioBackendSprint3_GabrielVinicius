namespace DesafioBackendSprint3_GabrielVinicius.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Role { get; set; } = "Cliente";
    public List<ContaBancaria> Contas { get; set; } = new();
}