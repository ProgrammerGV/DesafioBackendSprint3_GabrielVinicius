namespace DesafioBackendSprint3_GabrielVinicius.DTOs;

public class CriarContaDto
{
    public string Titular { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }

    // Pode ser "1" (Corrente), "2" (Poupança) ou "3" (Empresarial)
    public string TipoOpcao { get; set; } = string.Empty;
}