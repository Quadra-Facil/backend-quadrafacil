using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Arena;

public class AssociationArenaUserModel
{
    [Required]
    public int RealArenaId { get; set; }
}
