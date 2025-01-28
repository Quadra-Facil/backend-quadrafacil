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
    if (horas.StartTime >= horas.EndTime)
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
          EndTime = horas.EndTime
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








  // [Authorize]
  // [HttpPost("get")]
  // async public Task<IActionResult> GetDesativeProgram([FromBody] GetDesativeProgramArenaModel getProgram)
  // {
  //   var getProg = await _appDbContext.DesativeProgram
  //   .Where(s => s.ArenaId == getProgram.ArenaId)
  //   .ToListAsync();

  //   if (getProg == null)
  //   {
  //     return NotFound("Nenhuma programação.");
  //   }

  //   return Ok(getProg);
  // }

  // [Authorize]
  // [HttpDelete("delete")]
  // async public Task<IActionResult> DeleteDesativeProgram([FromBody] DeleteDesativeProgramArenaModel getProgram)
  // {
  //   var programToDelete = await _appDbContext.DesativeProgram
  //       .Where(s => s.Id == getProgram.Id)
  //       .FirstOrDefaultAsync();

  //   if (programToDelete == null)
  //   {
  //     return NotFound("Programação não encontrada.");
  //   }

  //   _appDbContext.DesativeProgram.Remove(programToDelete);

  //   await _appDbContext.SaveChangesAsync();

  //   return Ok("Programação desativada com sucesso.");
  // }

}
