using QuadraFacil_backend.Models.Reserve;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuadraFacil_backend.Models.Arena.Space;

public class GetSpaceWithArenaAndSportsModel
{
    [Required]
    public int ArenaId { get; set; }

    [Required]
    public string? Sports { get; set; }

}
