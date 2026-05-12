namespace DesafioBackendSprint3_GabrielVinicius.Models;

public interface ITransacao
{
    void Sacar(decimal valor);
    void Depositar(decimal valor);
}

