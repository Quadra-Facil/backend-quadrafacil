using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.Models.Arena;
using QuadraFacil_backend.API.Data;

namespace QuadraFacil_backend.Controllers.Arena;

[ApiController]
[Route("/api/[controller]")]
public class Arena : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public Arena(AppDbContext context)
    {
        _appDbContext = context;
    }

    [Authorize]
    [HttpPost]
    async public Task<IActionResult> Register([FromBody] ArenaModel arena)
    {

        var existingArena = await _appDbContext.Arenas.FirstOrDefaultAsync(a => a.Name == arena.Name);

        if (existingArena != null)
        {
            return NotFound("Arena já cadastrada");
        }


        var newArena = new ArenaModel
        {
            Name = arena.Name,
            Phone = arena.Phone,
            Status = arena.Status,
            ValueHour = arena.ValueHour
        };

        await _appDbContext.Arenas.AddAsync(newArena);
        await _appDbContext.SaveChangesAsync();

        return Ok(new { newArena.Id, newArena.Name, newArena.Phone, newArena.ValueHour, newArena.Status });
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult GetAllArenas()
    {
        // Incluindo corretamente os dados de AdressArenas
        var getArenas = _appDbContext.Arenas.Include(a => a.AdressArenas).ToList();

        if (getArenas == null || getArenas.Count < 1)
        {
            return NotFound("Nenhuma arena encontrada.");
        }

        return Ok(getArenas);
    }

    [Authorize]
    [HttpPut("association-arena-user")]
    public async Task<IActionResult> AssociationArenaUser([FromQuery] int id_user, [FromBody] AssociationArenaUserModel arenaUser)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id_user);
        user.ArenaId = arenaUser.RealArenaId;//passando a alteração para user

        try
        {
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Vínculo Arena/Usuário registrado."
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar a idArena.");
        }
    }
}
