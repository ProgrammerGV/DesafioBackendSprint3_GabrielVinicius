namespace DesafioBackendSprint3_GabrielVinicius.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // Necessário para evitar loop infinito ao listar dados

public abstract class ContaBancaria : ITransacao
{
    [Key]
    public int NumeroConta { get; private set; }
    public string Titular { get; private set; }
    public decimal Saldo { get; protected set; }
    public string TipoConta { get; protected set; }

    public int UsuarioId { get; set; }

    [JsonIgnore] // Impede que o C# tente listar o Usuário -> Conta -> Usuário infinitamente
    public Usuario? Usuario { get; set; }

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

    protected virtual bool PodeSacar(decimal valor)
    {
        if (valor <= 0) return false;
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