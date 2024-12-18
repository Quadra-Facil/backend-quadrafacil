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
                .FirstOrDefaultAsync(p => p.PlanSelect == plan.PlanSelect && p.ArenaId == plan.ArenaId);

            if (existingPlan != null)
            {
                // plano já existe
                return Conflict("Plano já foi inserido nesta arena.");
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
                    Status = "pendente"
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
    }
}