using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Dashboards;

public class MiniCardModel
{

  [Required]
  public int ArenaId { get; set; }
}