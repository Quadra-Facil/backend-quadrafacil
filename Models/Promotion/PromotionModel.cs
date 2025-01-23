using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Promotion;

public class PromotionModel
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string? PromotionType { get; set; }

  [Required]
  public DateTime? StartDate { get; set; }

  [Required]
  public DateTime? EndDate { get; set; }

  [Required]
  public int Value { get; set; }

  [Required]
  public int ArenaId { get; set; }
}