namespace DesafioBackendSprint3_GabrielVinicius.Models;

public class ContaEmpresarial : ContaBancaria
{
    protected ContaEmpresarial() { }

    public ContaEmpresarial(int numeroConta, string titular, decimal saldoInicial, string tipoConta)
        : base(numeroConta, titular, saldoInicial, tipoConta) { }
    public decimal LimiteEmprestimo { get; private set; } = 1000.00m;
    public decimal LimiteEmprestimoEmUso { get; private set; }

    public void DefinirLimiteEmprestimo(decimal limite) => LimiteEmprestimo = limite;

    public void SolicitarEmprestimo(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do empréstimo deve ser maior que zero.");

        if (valor > (LimiteEmprestimo - LimiteEmprestimoEmUso))
            throw new InvalidOperationException("Valor do empréstimo excede o limite disponível.");

        LimiteEmprestimoEmUso += valor;
        Saldo += valor;
    }
}