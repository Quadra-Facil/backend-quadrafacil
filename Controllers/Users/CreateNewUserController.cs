using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuadraFacil_backend.API.Data;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using QuadraFacil_backend.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace QuadraFacil_backend.Controllers.Users;

[Route("api/user")] // Define a rota para este controller
[ApiController] // Informa que este controller é uma API
public class Users : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfiguration _configuration;

    public Users(AppDbContext context, IConfiguration configuration)//injeção
    {
        _appDbContext = context;
        _configuration = configuration;

    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] User user)
    {

        var existingUser = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return BadRequest(new { Erro = "Usuário já existe" });
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

        var register = new User
        {
            UserName = user.UserName,
            Email = user.Email,
            Password = hashedPassword,
            Role = "admin",
            Phone = user.Phone,
            ArenaId = 0
        };

        //add user
        await _appDbContext.Users.AddAsync(register);
        await _appDbContext.SaveChangesAsync();

        return Ok(new
        { 
            user.UserName, user.Email, user.Phone 
        }
        );
    }

    [HttpPut("reset-pass")]
    public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromBody] ResetPasswordModel resetpass)
    {
        // Verificação se a senha foi informada
        if (string.IsNullOrEmpty(resetpass.Password))
        {
            return BadRequest("Senha não informada");
        }

        // Verificação se o email foi informado
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email não informado");
        }

        // Encontrar o usuário pelo email
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        // Se não encontrar o usuário
        if (user == null)
        {
            return NotFound("Usuário não encontrado");
        }

        // Atualizar a senha (com hash)
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetpass.Password);

        user.Password = hashedPassword;//passando a alteração para user

        try
        {
            // Salvar alterações no banco
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Senha alterada com sucesso!"
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar a senha.");
        }
    }
}
