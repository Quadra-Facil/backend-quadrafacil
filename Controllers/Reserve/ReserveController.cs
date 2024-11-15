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
        // Verifica se já existe uma reserva para o mesmo espaço, arena e data com horário conflitando
        var existeConflito = await _appDbContext.Reserve
            .AnyAsync(r =>
                r.ArenaId == reserve.ArenaId &&         
                r.SpaceId == reserve.SpaceId &&         
                r.DataReserve == reserve.DataReserve &&
                (
                    (r.TimeInitial < reserve.TimeFinal && r.TimeFinal > reserve.TimeInitial)
                )
            );

        if (existeConflito)
        {
            return BadRequest(new { Message = "Já existe uma reserva para o mesmo espaço e horário. Tente outro horário." });
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

        return Ok(new { Message = "Reserva criada, aguarde um administrador aprovar seu pedido" });
    }

}
