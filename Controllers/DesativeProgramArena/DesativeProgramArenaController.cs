using backend_quadrafacil.Models.Plan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;

namespace backend_quadrafacil.Controllers.DesativeProgram
{

  [ApiController]
  [Route("/api/[controller]")]
  public class DesativeProgram(AppDbContext context) : ControllerBase
  {
    private readonly AppDbContext _appDbContext = context;


    [Authorize]
    [HttpPost("desative/program")]
    public async Task<IActionResult> Register([FromBody] DesativeProgramArenaModel desativiProgram)
    {
      // Verifica se já existe um registro de programação para a arena
      var existingProgram = await _appDbContext.DesativeProgram
          .Where(p => p.ArenaId == desativiProgram.ArenaId)
          .FirstOrDefaultAsync();

      // Se já existir um registro para a arena, retorna um erro
      if (existingProgram != null)
      {
        return BadRequest("Somente um registro por vez.");
      }

      // Caso contrário, cria o novo registro
      var register = new DesativeProgramArenaModel
      {
        StartDate = desativiProgram.StartDate,
        EndDate = desativiProgram.EndDate,
        ArenaId = desativiProgram.ArenaId,
        Reason = desativiProgram.Reason
      };

      // Adiciona o novo registro no banco
      await _appDbContext.AddAsync(register);
      await _appDbContext.SaveChangesAsync();

      // Retorna o registro criado com status 200 OK
      return Ok(register);
    }


    [Authorize]
    [HttpPost("get")]
    async public Task<IActionResult> GetDesativeProgram([FromBody] GetDesativeProgramArenaModel getProgram)
    {
      var getProg = await _appDbContext.DesativeProgram
      .Where(s => s.ArenaId == getProgram.ArenaId)
      .ToListAsync();

      if (getProg == null)
      {
        return NotFound("Nenhuma programação.");
      }

      return Ok(getProg);
    }

    [Authorize]
    [HttpDelete("delete")]
    async public Task<IActionResult> DeleteDesativeProgram([FromBody] DeleteDesativeProgramArenaModel getProgram)
    {
      var programToDelete = await _appDbContext.DesativeProgram
          .Where(s => s.Id == getProgram.Id)
          .FirstOrDefaultAsync();

      if (programToDelete == null)
      {
        return NotFound("Programação não encontrada.");
      }

      _appDbContext.DesativeProgram.Remove(programToDelete);

      await _appDbContext.SaveChangesAsync();

      return Ok("Programação desativada com sucesso.");
    }

  }
}