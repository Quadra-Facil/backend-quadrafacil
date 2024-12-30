using backend_quadrafacil.Models.Plan;
using backend_quadrafacil.Models.PlanModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;

namespace backend_quadrafacil.Controllers.Plan
{
    [ApiController]
    [Route("/api/[controller]")]
    public class Plan(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _appDbContext = context;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] PlanModel plan)

        {
            // VERIFICANDO SE O PLANO JÁ EXISTE PARA AQUELA ARENA
            var existingPlan = await _appDbContext.Plan
                .FirstOrDefaultAsync(p => p.ArenaId == plan.ArenaId);

            if (existingPlan != null)
            {
                // plano já existe
                return Conflict("Já temos um plano para esta arena.");
            }

            var dateNow = DateTime.Today;

            PlanModel newPlan;

            // PLANO DE TESTE (30 DIAS)
            if (plan.PlanSelect == "teste")
            {
                var planTestExpire = dateNow.AddDays(30);

                newPlan = new PlanModel
                {
                    PlanSelect = "teste",
                    PlanExpiry = planTestExpire,
                    ArenaId = plan.ArenaId,
                    Status = "ativo"
                };
            }
            // PLANO mensal
            else if (plan.PlanSelect == "mensal")
            {
                var planSemestreExpire = dateNow.AddMonths(1);

                newPlan = new PlanModel
                {
                    PlanSelect = "mensal",
                    PlanExpiry = planSemestreExpire,
                    ArenaId = plan.ArenaId,
                    Status = "pendente"
                };
            }
            // PLANO SEMESTRAL (6 MESES)
            else if (plan.PlanSelect == "semestral")
            {
                var planSemestreExpire = dateNow.AddMonths(6);

                newPlan = new PlanModel
                {
                    PlanSelect = "semestral",
                    PlanExpiry = planSemestreExpire,
                    ArenaId = plan.ArenaId,
                    Status = "pendente"
                };
            }
            // PLANO ANUAL (1 ANO)
            else if (plan.PlanSelect == "anual")
            {
                var planAnnualExpire = dateNow.AddYears(1);

                newPlan = new PlanModel
                {
                    PlanSelect = "anual",
                    PlanExpiry = planAnnualExpire,
                    ArenaId = plan.ArenaId,
                    Status = "pendente"
                };
            }
            else
            {
                return BadRequest("Plano inválido.");
            }

            await _appDbContext.Plan.AddAsync(newPlan);
            await _appDbContext.SaveChangesAsync();

            return Ok(newPlan);
        }

        [Authorize]
        [HttpPut("/edit")]
        public async Task<IActionResult> EditStatusPlan([FromBody] EditStatusPlanModel editPlan)
        {
            // Recupera o plano pelo ArenaId
            var getPlan = await _appDbContext.Plan.FirstOrDefaultAsync(p => p.ArenaId == editPlan.ArenaId);

            if (getPlan == null)
            {
                return NotFound(new { Message = "Plano não encontrado." });
            }

            var dateAtual = DateTime.Now;

            // Verifica se o tipo de plano está sendo alterado
            if (getPlan.PlanSelect != editPlan.PlanSelect)
            {
                // Lógica para alterar o tipo de plano:
                switch (editPlan.PlanSelect.ToLower())
                {
                    case "mensal":
                        // Se for mensal, a data de expiração é ajustada para +30 dias
                        getPlan.PlanExpiry = dateAtual.AddDays(30);
                        getPlan.PlanSelect = "mensal";
                        break;
                    case "semestral":
                        // Se for semestral, a data de expiração é ajustada para +6 meses
                        getPlan.PlanExpiry = dateAtual.AddMonths(6);
                        getPlan.PlanSelect = "semestral";
                        break;
                    case "anual":
                        // Se for anual, a data de expiração é ajustada para +1 ano
                        getPlan.PlanExpiry = dateAtual.AddYears(1);
                        getPlan.PlanSelect = "anual";
                        break;
                    default:
                        return BadRequest(new { Message = "Tipo de plano inválido." });
                }

                // Atualiza o Status para "ativo" quando o plano for alterado
                getPlan.Status = "ativo";
            }
            else
            {
                // Se o plano não foi alterado, apenas prolonga a validade
                if (getPlan.PlanExpiry.HasValue)
                {
                    switch (editPlan.PlanSelect.ToLower())
                    {
                        case "mensal":
                            getPlan.PlanExpiry = getPlan.PlanExpiry.Value.AddDays(30);
                            getPlan.PlanSelect = "mensal";
                            break;
                        case "semestral":
                            getPlan.PlanExpiry = getPlan.PlanExpiry.Value.AddMonths(6);
                            getPlan.PlanSelect = "semestral";
                            break;
                        case "anual":
                            getPlan.PlanExpiry = getPlan.PlanExpiry.Value.AddYears(1);
                            getPlan.PlanSelect = "anual";
                            break;
                        default:
                            return BadRequest(new { Message = "Tipo de plano inválido." });
                    }
                    // Atualiza o Status para "ativo" ao prolongar o plano
                    getPlan.Status = "ativo";
                }
                else
                {
                    return BadRequest(new { Message = "Data de expiração não está definida." });
                }
            }

            // Atualiza o ArenaId conforme passado no Postman
            getPlan.ArenaId = editPlan.ArenaId;

            // Verifica se o plano expirou, e se expirou, marca como "inativo"
            if (getPlan.PlanExpiry.HasValue && getPlan.PlanExpiry.Value <= dateAtual)
            {
                getPlan.Status = "inativo";
            }

            try
            {
                await _appDbContext.SaveChangesAsync();
                return Ok(new { Message = "Plano atualizado com sucesso." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar plano.");
            }
        }

        // [Authorize]
        // [HttpPost]
        // public async Task<IActionResult> GetPlanArena([FromBody] GetPlanOfArenaIdModel arena)
        // {
        //     var getArena = await _appDbContext.Plan.FirstOrDefaultAsync(a=>a.ArenaId == arena.ArenaId);

        //     if(getArena == null)
        //     {
        //         return NotFound("Nenhuma arena encontrada.");
        //     }

        //     0
        // }


    }
}