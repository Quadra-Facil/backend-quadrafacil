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
            Observation = reserve.Observation
        };

        await _appDbContext.Reserve.AddAsync(addReserve);
        await _appDbContext.SaveChangesAsync();

        return Ok(new { Message = "Reserva criada, aguarde um administrador aprovar sua solicitação" });
    }

}
