namespace DesafioBackendSprint3_GabrielVinicius.Models;

public class Transacao
{
    public int Id { get; set; }

    public int ContaBancariaId { get; set; }

    public string Tipo { get; set; }

    public decimal Valor { get; set; }

    public DateTime Data { get; set; } = DateTime.Now;

    public ContaBancaria? Conta { get; set; }
}