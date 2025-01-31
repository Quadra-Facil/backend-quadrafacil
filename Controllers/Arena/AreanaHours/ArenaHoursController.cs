using backend_quadrafacil.Models.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;

namespace backend_quadrafacil.Controllers.ArenaHours;

[ApiController]
[Route("api/[controller]")]
public class ArenaHoursController : ControllerBase
{
  private readonly AppDbContext _appDbContext;

  public ArenaHoursController(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }

  [Authorize]
  [HttpPost("arena-hours")]
  public async Task<IActionResult> Registrar([FromBody] ArenaHoursModel horas)
  {
    // Valida se HoraInicio é menor que HoraFim
    if (horas.Open == true && (horas.StartTime >= horas.EndTime))
    {
      return BadRequest("A hora de início deve ser anterior à hora de término.");
    }

    try
    {
      // Para cada dia da semana fornecido, cria o horário
      foreach (var dia in horas.WeekDays)
      {
        // Verifica se já existe um horário para a mesma ArenaId e WeekDay
        var registroExistente = await _appDbContext.ArenaHours
            .FirstOrDefaultAsync(ah => ah.ArenaId == horas.ArenaId && ah.WeekDays.Contains(dia));

        if (registroExistente != null)
        {
          return Conflict($"Já existe um horário registrado para a arena {horas.ArenaId} no dia {dia}.");
        }

        // Caso contrário, cria o novo registro para esse dia
        var registrar = new ArenaHoursModel
        {
          ArenaId = horas.ArenaId,
          WeekDays = new List<int> { dia },  // Atribui o dia individualmente
          StartTime = horas.StartTime,
          EndTime = horas.EndTime,
          Open = horas.Open
        };

        // Adiciona o novo registro no banco de dados
        await _appDbContext.AddAsync(registrar);
      }

      // Salva todas as alterações no banco de dados
      await _appDbContext.SaveChangesAsync();

      return Ok("Horários registrados com sucesso.");
    }
    catch (Exception ex)
    {
      // Em caso de erro, retorna uma resposta genérica de erro com status 500
      return StatusCode(500, $"Erro ao salvar os horários da arena: {ex.Message}");
    }
  }

  [Authorize]
  [HttpPost("get")]
  async public Task<IActionResult> GetHoursArena([FromBody] GetHoursWithArenaModel hours)
  {
    var getHours = await _appDbContext.ArenaHours
    .Where(s => s.ArenaId == hours.ArenaId)
    .ToListAsync();

    if (getHours == null)
    {
      return NotFound("Nenhum horário.");
    }

    return Ok(getHours);
  }

  [Authorize]
  [HttpDelete("delete")]
  public async Task<IActionResult> DeleteHours([FromBody] DeleteHoursModel hours)
  {
    var getHoursBd = await _appDbContext.ArenaHours
        .Where(s => s.Id == hours.Id)
        .FirstOrDefaultAsync();

    if (getHoursBd == null)
    {
      return NotFound("Não encontrado o expediente.");
    }

    _appDbContext.ArenaHours.Remove(getHoursBd);

    await _appDbContext.SaveChangesAsync();

    return Ok("Expediente deletado com sucesso.");
  }

}
