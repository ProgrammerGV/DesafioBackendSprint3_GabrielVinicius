using Microsoft.EntityFrameworkCore;
using DesafioBackendSprint3_GabrielVinicius.Models;

namespace DesafioBackendSprint3_GabrielVinicius.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ContaBancaria> ContasBancarias { get; set; }
    public DbSet<ContaCorrente> ContasCorrentes { get; set; }
    public DbSet<ContaPoupanca> ContasPoupancas { get; set; }
    public DbSet<ContaEmpresarial> ContasEmpresariais { get; set; }
}