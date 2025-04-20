using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Reserve;

public class GetReservesWithArenaModel
{

  [Required]
  public int? ArenaId { get; set; }

}