using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.API.Models.Users;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuadraFacil_backend.Controllers.Users;

[Route("api/user")] // Define a rota para este controller
[ApiController] // Informa que este controller é uma API
public class CreateNewUserController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfiguration _configuration;

    public CreateNewUserController(AppDbContext context, IConfiguration configuration)//injeção
    {
        _appDbContext = context;
        _configuration = configuration;

    }

    [HttpPost]
    public IActionResult Register([FromBody] User user)
    {

        var existingUser = _appDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
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
            Phone = user.Phone
        };

        //add user
        _appDbContext.Users.Add(register);
        _appDbContext.SaveChanges();

        return Ok(new
        { 
            user.UserName, user.Email, user.Phone 
        }
        );
    }
}
