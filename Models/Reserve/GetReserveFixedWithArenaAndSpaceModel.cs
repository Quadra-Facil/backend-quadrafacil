using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Reserve;

public class GetReserveFixedWithArenaAndSpaceModel
{

    [Required]
    public int ArenaId { get; set; }

    [Required]
    public int SpaceId { get; set; }

    [Required]
    public string? TypeReserve { get; set; }
}
