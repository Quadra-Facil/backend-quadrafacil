﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;
using QuadraFacil_backend.Models.Arena;
using QuadraFacil_backend.Models.Arena.Space;

namespace QuadraFacil_backend.Controllers.Arena.Space;

[ApiController]
[Route("/api/newSpace")]
public class SpaceController(AppDbContext context) : ControllerBase
{

    private readonly AppDbContext _appDbContext = context;

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] SpaceModel space)
    {
        var spaceExist = await _appDbContext.Spaces
       .Include(s => s.Arena) // Incluindo a Arena associada
       .FirstOrDefaultAsync(u => u.Name == space.Name && u.Arena.Id == space.ArenaId);


        if (spaceExist != null)
        {
            return BadRequest($"Espaço já cadastrado para {spaceExist?.Arena?.Name}");
        }

        var addSpace = new SpaceModel
        {
            Name = space.Name,
            ArenaId = space.ArenaId,
            Sports = space?.Sports,
            Status = "Disponível"
        };

        await _appDbContext.AddAsync(addSpace);
        await _appDbContext.SaveChangesAsync();

        var SpaceWithArena = _appDbContext.Spaces
        .Include(a => a.Arena)
        .FirstOrDefault(e => e.SpaceId == addSpace.ArenaId);

        return Ok(SpaceWithArena);
    }

    [Authorize]
    [HttpPost("get-spaces")]
    async public Task<IActionResult> GetSpacesWithArena([FromBody] GetSpaceAndArenaModel arena)
    {
        var getSpaces = await _appDbContext.Spaces
        .Where(s => s.ArenaId == arena.ArenaId)
        .ToListAsync();

        if (getSpaces == null)
        {
            return NotFound("Nenhum espaço encontrado");
        }

        return Ok(getSpaces);
    }

    [Authorize]
    [HttpPut("edit-space")]
    async public Task<IActionResult> EditStatusSpace([FromBody] EditStatusSpaceModel space)
    {
        var getSpace = await _appDbContext.Spaces.FirstOrDefaultAsync(s => s.SpaceId == space.SpaceId);

        getSpace.Status = space.Status;//alterando status

        try
        {
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Status do espaço foi alterado."
            });
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar status.");
        }
    }

}
