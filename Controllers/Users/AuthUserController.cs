using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.Models.Users;
using QuadraFacil_backend.Services;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QuadraFacil_backend.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class AuthController(AppDbContext context, IConfiguration configuration, TokenService tokenService) : ControllerBase
{
    private readonly AppDbContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly TokenService _tokenService = tokenService;

    [HttpPost("login")]
    async public Task<IActionResult> Login([FromBody] UserLogin login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
        if (user == null)
        {
            return Unauthorized(new { Erro = "Usuário não encontrado" });
        }

        bool senhaCorreta = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);

        if (!senhaCorreta)
        {
            return Unauthorized(new { Erro = "Senha incorreta" });
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new { login.Email, token });
    }
}

// Modelo para login
public class UserLogin
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }
}
