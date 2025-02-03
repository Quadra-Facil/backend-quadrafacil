using backend_quadrafacil.Models.Plan;
using backend_quadrafacil.Models.Promotion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;

namespace backend_quadrafacil.Controllers.Promotion;

[ApiController]
[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
  private readonly AppDbContext _appDbContext;

  public PromotionsController(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }

  [Authorize]
  [HttpPost("promotion")]
  public async Task<IActionResult> Registrar([FromBody] PromotionModel promotion)
  {
    // Se o "When" for "todo-dia", as datas StartDate e EndDate ficam como null
    if (promotion.When == "todo-dia")
    {
      promotion.StartDate = null;
      promotion.EndDate = null;
    }
    else
    {
      // Verifica se a data de início é anterior à data de término
      if (promotion.StartDate >= promotion.EndDate)
      {
        return BadRequest("A data de início deve ser anterior à data de término.");
      }
    }

    try
    {
      // Para cada dia da semana fornecido, cria o registro de promoção
      foreach (var dia in promotion.WeekDays)
      {
        // Verifica se já existe uma promoção para a mesma ArenaId e WeekDay
        var registroExistente = await _appDbContext.Promotions
            .FirstOrDefaultAsync(p => p.ArenaId == promotion.ArenaId && p.WeekDays.Contains(dia));

        if (registroExistente != null)
        {
          return Conflict($"Já existe uma promoção registrada para a arena {promotion.ArenaId} no dia {dia}.");
        }

        // Caso contrário, cria o novo registro de promoção para esse dia
        var registrar = new PromotionModel
        {
          ArenaId = promotion.ArenaId,
          WeekDays = new List<int> { dia },  // Atribui o dia individualmente
          StartDate = promotion.StartDate,
          EndDate = promotion.EndDate,
          Value = promotion.Value,
          QtdPeople = promotion.QtdPeople,
          PromotionType = promotion.PromotionType,
          When = promotion.When
        };

        // Adiciona o novo registro no banco de dados
        await _appDbContext.Promotions.AddAsync(registrar);
      }

      // Salva todas as alterações no banco de dados
      await _appDbContext.SaveChangesAsync();

      return Ok("Promoções registradas com sucesso.");
    }
    catch (Exception ex)
    {
      // Em caso de erro, retorna uma resposta genérica de erro com status 500
      return StatusCode(500, $"Erro ao salvar as promoções: {ex.Message}");
    }
  }

}