namespace DesafioBackendSprint3_GabrielVinicius.Models;
using System.ComponentModel.DataAnnotations;

public abstract class ContaBancaria : ITransacao
{
    [Key]
    public int NumeroConta { get; private set; }
    public string Titular { get; private set; }
    public decimal Saldo { get; protected set; }
    public string TipoConta { get; protected set; }

    public int CountCorrente { get; private set; }
    public int CountPoupanca { get; private set; }
    public int CountEmpresarial { get; private set; }

    protected ContaBancaria(int numeroConta, string titular, decimal saldoInicial, string tipoConta)
    {
        NumeroConta = numeroConta;
        Titular = titular;
        Saldo = saldoInicial;
        TipoConta = tipoConta;
    }

    protected ContaBancaria()
    {
        Titular = string.Empty;
        TipoConta = string.Empty;
    }
    public void IncrementarContador(string tipoConta)
    {
        if (tipoConta == "1") CountCorrente++;
        else if (tipoConta == "2") CountPoupanca++;
        else if (tipoConta == "3") CountEmpresarial++;
    }

    public int ObterContador(string tipoConta)
    {
        return tipoConta switch
        {
            "1" => CountCorrente,
            "2" => CountPoupanca,
            "3" => CountEmpresarial,
            _ => 0
        };
    }

    protected virtual bool PodeSacar(decimal valor)
    {
        if (valor <= 0)
        {
            return false;
        }
        return Saldo >= valor;
    }

    public virtual void Sacar(decimal valor)
    {
        if (!PodeSacar(valor))
            throw new InvalidOperationException("Saldo insuficiente para saque.");
        Saldo -= valor;
    }

    public virtual void Depositar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser positivo.");
        Saldo += valor;
    }

    public void SetNumeroConta(int numero) => NumeroConta = numero;
    public void SetTitular(string titular) => Titular = titular;
    public void SetTipoConta(string tipo) => TipoConta = tipo;
}
