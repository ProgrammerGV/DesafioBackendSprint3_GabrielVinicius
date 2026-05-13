using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DesafioBackendSprint3_GabrielVinicius.Interfaces;
using DesafioBackendSprint3_GabrielVinicius.Models;
using Microsoft.IdentityModel.Tokens;

namespace DesafioBackendSprint3_GabrielVinicius.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IConfiguration _config;

    public AuthService(IUsuarioRepository usuarioRepo, IConfiguration config)
    {
        _usuarioRepo = usuarioRepo;
        _config = config;
    }

    public async Task<string?> AutenticarAsync(DTOs.LoginDto dto)
    {
        var usuario = await _usuarioRepo.GetByLoginAndSenhaAsync(dto.Usuario, dto.Senha);
        if (usuario == null) return null;

        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(tokenDescriptor));
    }
}

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepo;

    public UsuarioService(IUsuarioRepository usuarioRepo) { _usuarioRepo = usuarioRepo; }

    public async Task RegistrarAsync(DTOs.RegistrarUsuarioDto dto)
    {
        if (await _usuarioRepo.ExistsByLoginAsync(dto.Usuario)) throw new Exception("Este nome de usuário já está em uso.");
        if (await _usuarioRepo.ExistsByCpfAsync(dto.Cpf)) throw new Exception("Este CPF já está cadastrado no sistema.");

        var novoUsuario = new Usuario { Nome = dto.Nome, Login = dto.Usuario, Senha = dto.Senha, Cpf = dto.Cpf };

        ContaBancaria novaConta = dto.TipoContaInicial switch
        {
            "2" => new ContaPoupanca(0, dto.Nome, 0, "Poupança"),
            "3" => new ContaEmpresarial(0, dto.Nome, 0, "Empresarial"),
            _ => new ContaCorrente(0, dto.Nome, 0, "Corrente")
        };

        novaConta.Usuario = novoUsuario;
        novoUsuario.Contas.Add(novaConta);

        await _usuarioRepo.AddAsync(novoUsuario);
        await _usuarioRepo.SaveChangesAsync();
    }
}