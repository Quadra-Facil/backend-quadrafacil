using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.Migrations;
using QuadraFacil_backend.Models.Arena;


namespace QuadraFacil_backend.Controllers.Arena;

[ApiController]
[Route("/api/adress")]
public class AdressArenaController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public AdressArenaController(AppDbContext context)
    {
        _appDbContext = context;
    }

    [HttpPost]
    public IActionResult Register([FromBody] AdressArena adress)
    {
        var existingAdress = _appDbContext.AdressArenas.FirstOrDefault(e => e.Street == adress.Street);


        if (existingAdress != null)
        {
            return BadRequest(new { Erro = $"{adress.Street} já existe!" });
        }

        var newAdress = new AdressArena
        {
            State = adress.State,
            City = adress.City,
            Street = adress.Street,
            Neighborhood = adress.Neighborhood,
            Number = adress.Number,
            Reference = adress.Reference,
            ArenaId = adress.ArenaId
        };

        _appDbContext.Add(newAdress);
        _appDbContext.SaveChanges();

        // Inclua arena ao adicionar o endereço
        var adressWithArena = _appDbContext.AdressArenas
            .Include(a => a.Arena)
            .FirstOrDefault(e => e.Id == newAdress.Id);

        return Ok(adressWithArena);
    }
}
