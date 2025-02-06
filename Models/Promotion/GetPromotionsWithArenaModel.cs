using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Promotion;

public class GetPromotionsWithArenaModel
{

  [Required]
  public int? ArenaId { get; set; }
}