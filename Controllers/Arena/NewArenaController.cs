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

    //public async Task<IActionResult> AddArena([FromBody] Arena arena)
    [HttpPost]
    public IActionResult Register([FromBody] ArenaModel arena)
    {

        var existingArena = _appDbContext.Arenas.FirstOrDefault(e => e.Name == arena.Name);


        var newArena = new ArenaModel
        {
            Name = arena.Name,
            Phone = arena.Phone
        };

        _appDbContext.Arenas.Add(newArena);
        _appDbContext.SaveChanges();

        return Ok(new { newArena.Id, newArena.Name, newArena.Phone });
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAllArenas()
    {
        // Incluindo corretamente os dados de AdressArenas
        var getArenas = _appDbContext.Arenas.Include(a => a.AdressArenas).ToList();
        //var getArenas = _appDbContext.Arenas.ToList();

        if (getArenas == null || getArenas.Count < 1)
        {
            return NotFound("Nenhuma arena encontrada.");
        }

        return Ok(getArenas);
    }


    //[HttpGet("arena/{arenaId}")]
    //public async Task<IActionResult> GetAddressByArenaId(int arenaId)
    //{
    //    // Busca o endereço com base no ArenaId e inclui os dados da Arena relacionados
    //    var address = await _appDbContext.AdressArenas
    //        .Include(a => a.Arena)  // Inclui os dados da Arena automaticamente
    //        .FirstOrDefaultAsync(a => a.ArenaId == arenaId);

    //    if (address == null)
    //    {
    //        return NotFound(new { message = "Address not found." });
    //    }

    //    // Retorna o objeto Address diretamente
    //    return Ok(address);
    //}
};
