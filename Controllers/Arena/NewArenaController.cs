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
    public async Task<IActionResult> GetAllArenas()
    {
        // Incluindo os dados de AdressArenas (mas sem mudar o modelo, sem adicionar a coleção de Plans)
        var getArenas = await _appDbContext.Arenas
                                           .Include(a => a.AdressArenas)  // Inclui os endereços de cada arena
                                           .ToListAsync();

        // Se as arenas não foram encontradas, retorna um erro
        if (getArenas == null || getArenas.Count < 1)
        {
            return NotFound("Nenhuma arena encontrada.");
        }

        // Agora, buscamos os planos associados a cada arena
        foreach (var arena in getArenas)
        {
            // Aqui fazemos a busca separada para os planos de cada arena usando o Id da arena
            var plans = await _appDbContext.Plan
                                           .Where(p => p.ArenaId == arena.Id)  // Filtrando os planos com base no ArenaId
                                           .ToListAsync();  // Buscando todos os planos relacionados a cada arena

            // Adicionamos os planos encontrados ao objeto da arena
            arena.Plans = plans; // Associa os planos à arena (caso a classe `ArenaModel` tenha a propriedade Plans)
        }

        // Agora, retornamos o JSON com as arenas e seus respectivos planos
        return Ok(new
        {
            Arenas = getArenas
        });
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

    [Authorize]
    [HttpPut("status-edit")]
    public async Task<IActionResult> StatusEdit([FromBody] StatusEditModel statusModel)
    {
        var getArena = await _appDbContext.Arenas.FirstOrDefaultAsync(u => u.Id == statusModel.RealArenaId);
        getArena.Status = statusModel.NewStatus;//passando a alteração para user

        try
        {
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Status alterado."
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar status.");
        }
    }

    [Authorize]
    [HttpPost("getArena")]
    public async Task<IActionResult> GetNameArenaWithUser([FromBody] GetNameArenaWithUser getArena)
    {
        var getArenaResult = await _appDbContext.Arenas.
        Include(a => a.AdressArenas).FirstOrDefaultAsync(u => u.Id == getArena.arenaId);

        if (getArenaResult == null)
        {
            return NotFound("Nenhuma arena encontrada.");
        }

        return Ok(getArenaResult);

    }
}
