using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Arena;

public class StatusEditModel
{
    [Required]
    public int RealArenaId { get; set; }

    [Required]
    public string? NewStatus { get; set; }
}
