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
      var register = new DesativeProgramArenaModel
      {
        StartDate = desativiProgram.StartDate,
        EndDate = desativiProgram.EndDate,
        ArenaId = desativiProgram.ArenaId,
        Reason = desativiProgram.Reason
      };

      await _appDbContext.AddAsync(register);
      await _appDbContext.SaveChangesAsync();

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