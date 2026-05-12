namespace DesafioBackendSprint3_GabrielVinicius.Models;

public class ContaCorrente : ContaBancaria

{
    protected ContaCorrente() { }

    public ContaCorrente(int numeroConta, string titular, decimal saldoInicial, string tipoConta)
        : base(numeroConta, titular, saldoInicial, tipoConta) { }
    protected override bool PodeSacar(decimal valor)
    {
        if (valor <= 0)
        {
            return false;
        }
        return Saldo >= (valor + 5.00m);
    }
    public override void Sacar(decimal valor)
    {
        if (!PodeSacar(valor))
            throw new InvalidOperationException("Saldo insuficiente para saque (incluindo taxa de R$ 5,00).");
        Saldo -= (valor + 5.00m);
    }
}