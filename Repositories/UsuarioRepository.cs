using DesafioBackendSprint3_GabrielVinicius.Data;
using DesafioBackendSprint3_GabrielVinicius.Interfaces;
using DesafioBackendSprint3_GabrielVinicius.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendSprint3_GabrielVinicius.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context) { _context = context; }

    public async Task<Usuario?> GetByLoginAndSenhaAsync(string login, string senha) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login && u.Senha == senha);

    public async Task<bool> ExistsByLoginAsync(string login) =>
        await _context.Usuarios.AnyAsync(u => u.Login == login);

    public async Task<bool> ExistsByCpfAsync(string cpf) =>
        await _context.Usuarios.AnyAsync(u => u.Cpf == cpf);

    public async Task AddAsync(Usuario usuario) => await _context.Usuarios.AddAsync(usuario);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}