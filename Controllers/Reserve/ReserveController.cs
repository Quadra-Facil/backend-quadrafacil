using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.Models.Reserve;

namespace QuadraFacil_backend.Controllers.Reserve;

[ApiController]
[Route("/api/reserve")]
public class ReserveController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _appDbContext = context;

    [Authorize]
    [HttpPost]
    async public Task<IActionResult> Register([FromBody] ReserveModel reserve)

    {

        var existingReservation = await _appDbContext.Reserve
                .Where(r => r.ArenaId == reserve.ArenaId
                    && r.SpaceId == reserve.SpaceId // Mesmo space
                    && r.DataReserve == reserve.DataReserve // Mesmo dia
                    && (
                        (reserve.TimeInitial < r.TimeFinal && reserve.TimeFinal > r.TimeInitial) // Conflito de horários
                    ))
                .FirstOrDefaultAsync();


        if (existingReservation != null)
        {
            return BadRequest("Já existe uma reserva para a mesma arena, espaço, data e horário.");
        }

        var addReserve = new ReserveModel
        {
            UserId = reserve.UserId,
            ArenaId = reserve.ArenaId,
            SpaceId = reserve.SpaceId,
            DataReserve = reserve.DataReserve,
            TimeInitial = reserve.TimeInitial,
            TimeFinal = reserve.TimeFinal,
            Status = "Pendente",
            Observation = reserve.Observation,
            TypeReserve = reserve.TypeReserve
        };

        await _appDbContext.Reserve.AddAsync(addReserve);
        await _appDbContext.SaveChangesAsync();

        return Ok(new { Message = "Reserva criada, aguarde um administrador aprovar sua solicitação" });
    }

    [Authorize]
    [HttpGet("/getReserve/arena/data")]
    async public Task<IActionResult> GetReservesWithData([FromBody] GetReservesArenaWithDatareserveModel reserve)
    {
        // Verifica se a dataReserve fornecida está no formato correto
        if (reserve.DataReserve == null || reserve.DataReserve == DateTime.MinValue)
        {
            return BadRequest("Data de reserva não fornecida ou inválida.");
        }

        var getArena = await _appDbContext.Arenas.FirstOrDefaultAsync(a => a.Id == reserve.ArenaId);

        var getReservesWithArena = await _appDbContext.Reserve
            .Where(r => r.ArenaId == reserve.ArenaId && r.DataReserve == reserve.DataReserve)
            .ToListAsync();

        var arenaData = new
        {
            getArena?.Id,
            getArena?.Name,
            getArena?.Phone,
            getArena?.ValueHour
        };

        var reservaDetails = new List<object>();
        foreach (var result in getReservesWithArena)
        {
            var getSpace = await _appDbContext.Spaces.FirstOrDefaultAsync(s => s.SpaceId == result.SpaceId);
            var getUser = await _appDbContext.Users.FirstOrDefaultAsync(s => s.Id == result.UserId);

            // Pega dados específicos da reserva
            var reservaData = new
            {
                result.Id_reserve,
                result.DataReserve,
                getSpace?.Name,
                result.TimeInitial,
                result.TimeFinal,
                result.Observation,
                getUser?.UserName,
                getUser?.Phone,
                getUser?.Role
            };
            // Adiciona na lista
            reservaDetails.Add(reservaData);
        }

        // Retorna a resposta com os dados da arena e das reservas
        return Ok(new
        {
            ArenaName = arenaData,
            Reservas = reservaDetails
        });
    }

    [Authorize]
    [HttpPost("/getReserves/date")]
    async public Task<IActionResult> GetReservesDateSpace([FromBody] GetReservesWithDateAndSpaceModel reserve)
    {
        var existingReservation = await _appDbContext.Reserve
               .Where(r => r.ArenaId == reserve.ArenaId
                   && r.SpaceId == reserve.SpaceId // Mesmo space
                   && r.DataReserve == reserve.DataReserve // Mesmo dia
                   )
                   .ToArrayAsync();
        return Ok(existingReservation);

    }
}
