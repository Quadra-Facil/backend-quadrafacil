using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.API.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuadraFacil_backend.Controllers.Users;

[Route("api/[controller]")] // Define a rota para este controller
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
        
        var register = new User
        {
            UserName = user.UserName,
            Email = user.Email,
            Password = user.Password,
            Role = "admin",
        };

        //add user
        _appDbContext.Users.Add(register);
        _appDbContext.SaveChanges();

        //gerar token
        var token = GenerateJwtToken(register);

        return Ok(new 
            { user.UserName, user.Email,token }
        );
    }

    // Método auxiliar para gerar um token JWT (já existente)
    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            //expires: DateTime.Now.AddMinutes(1440),
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
