using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Promotion;

public class PromotionModel
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string? PromotionType { get; set; }

  public string? When { get; set; }

  public DateTime? StartDate { get; set; }

  public DateTime? EndDate { get; set; }

  [Required]
  public List<int>? WeekDays { get; set; } // List of weekdays (e.g. [1, 2, 3, 4, 5])

  public int Value { get; set; }

  public int QtdPeople { get; set; }

  [Required]
  public int ArenaId { get; set; }
}