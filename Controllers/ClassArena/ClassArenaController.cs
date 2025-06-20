using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.Models.ClassArena;

namespace QuadraFacil_backend.Controllers.ClassArena;

[ApiController]
[Route("/api/[controller]")]
public class ClassArena(AppDbContext context) : ControllerBase
{
  private readonly AppDbContext _appDbContext = context;

  [Authorize]
  [HttpPost]
  async public Task<IActionResult> Register([FromBody] ClassArenaModel classArena)
  {
    var existingArena = await _appDbContext.ClassArena.FirstOrDefaultAsync(
        a => a.NameClass == classArena.NameClass && a.ArenaId == classArena.ArenaId);

    if (existingArena != null)
    {
      return NotFound("Esta turma já está criada.");
    }

    var newClass = new ClassArenaModel // Note que mudou para ClassArena
    {
      NameClass = classArena.NameClass,
      Teacher = classArena.Teacher,
      PhoneTeacher = classArena.PhoneTeacher,
      CreateClass = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
      ArenaId = classArena.ArenaId,
    };

    await _appDbContext.ClassArena.AddAsync(newClass);
    await _appDbContext.SaveChangesAsync();

    return Ok(new { newClass });
  }

  [Authorize]
  [HttpPost("get")]
  public async Task<IActionResult> GetAllClassArena([FromBody] GetAllClassOfArenaModel getClass)
  {
    var turmas = await _appDbContext.ClassArena
                                   .Where(c => c.ArenaId == getClass.ArenaId)
                                   .ToListAsync();

    if (turmas == null || turmas.Count < 1)
    {
      return NotFound("Nenhuma turma encontrada.");
    }

    return Ok(turmas);
  }
  // [Authorize]
  // [HttpPost("get/class/client")]
  // public async Task<IActionResult> GetClassArenaClient([FromBody] GetClassArenaClientDto getClass)
  // {
  //   var turmas = await _appDbContext.Users
  //                                  .Where(c => c.== getClass.ClientId)
  //                                  .ToListAsync();

  //   if (turmas == null || turmas.Count < 1)
  //   {
  //     return NotFound("Nenhuma turma encontrada.");
  //   }

  //   return Ok(turmas);
  // }
  // public class GetClassArenaClientDto
  // {
  //   public int ClientId { get; set; }
  // }
}
