using backend_quadrafacil.Models.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.Models.Reserve;

namespace QuadraFacil_backend.Controllers.Reserve;

[ApiController]
[Route("/api/dash")]
public class DashboardController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _appDbContext = context;

    [Authorize]
    [HttpPost("/getdata/minicard")]
    async public Task<IActionResult> GetDataMiniChart([FromBody] MiniCardModel mini)
    {
        var getReserves = await _appDbContext.Reserve.Where(
            r => r.ArenaId == mini.ArenaId
        ).ToArrayAsync();

        return Ok(getReserves);
    }
}
