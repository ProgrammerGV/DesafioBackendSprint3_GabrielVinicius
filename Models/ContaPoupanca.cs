namespace DesafioBackendSprint3_GabrielVinicius.Models;

public class ContaPoupanca : ContaBancaria
{
    protected ContaPoupanca() { }

    public ContaPoupanca(int numeroConta, string titular, decimal saldoInicial, string tipoConta)
        : base(numeroConta, titular, saldoInicial, tipoConta) { }

    public void AplicarRendimento(decimal taxa)
    {
        if (taxa <= 0 || taxa > 1)
        {
            throw new ArgumentException("A taxa de rendimento deve ser maior que 0 e no máximo 1 (100%).");
        }

        Saldo += Saldo * taxa;
    }
}
