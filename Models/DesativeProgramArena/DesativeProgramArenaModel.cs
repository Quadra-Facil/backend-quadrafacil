using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Plan;

public class DesativeProgramArenaModel
{
  [Required]
  [Key]
  public int Id { get; set; }

  [Required]
  public DateTime? StartDate { get; set; }

  [Required]
  public DateTime? EndDate { get; set; }

  [Required]
  public int ArenaId { get; set; }

  public string? Reason { get; set; }
}