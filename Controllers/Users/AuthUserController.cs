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

namespace QuadraFacil_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, IConfiguration configuration, TokenService tokenService)
        {
            _context = context;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
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
}
