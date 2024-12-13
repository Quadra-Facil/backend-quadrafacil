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
using Microsoft.AspNetCore.Authorization;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace QuadraFacil_backend.Controllers.Users;

[Route("api/user")]
[ApiController]
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
            Role = "client",
            Phone = user.Phone,
            ArenaId = 0
        };

        await _appDbContext.Users.AddAsync(register);
        await _appDbContext.SaveChangesAsync();

        return Ok(new
        {
            user.UserName,
            user.Email,
            user.Phone,
            user.Role
        }
        );
    }

    [HttpPut("reset-pass")]
    public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromBody] ResetPasswordModel resetpass)
    {
        if (string.IsNullOrEmpty(resetpass.Password))
        {
            return BadRequest("Senha não informada");
        }

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email não informado");
        }

        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return NotFound("Usuário não encontrado");
        }

        // Atualizar a senha (com hash)
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetpass.Password);

        user.Password = hashedPassword;//passando a alteração para user

        try
        {
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

    [Authorize]
    [HttpPut("edit/rule")]
    public async Task<IActionResult> EditRoleUser([FromBody] EditRoleUserModel user)
    {
        var userFind = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == user.UserId);
        if (userFind == null)
        {
            return NotFound("Usuário não encontrado");
        }

        userFind.Role = "admin";//passando a alteração para user

        try
        {
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Vinculo admin registrado"
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao vincular usuário.");
        }


    }

    [Authorize]
    [HttpGet("getUsers")]
    async public Task<IActionResult> GetAllUser()
    {
        // Consulta para pegar todos os usuários
        var users = await _appDbContext.Users.Select(user => new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.Role,
            user.Phone,
            user.ArenaId
        }).ToListAsync();

        return Ok(new
        {
            Users = users

        });
    }






}