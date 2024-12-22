using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.PlanModel;
public class PlanModel
{
    [Key]
    public int Id { get; set; }

    public string? PlanSelect { get; set; }

    public DateTime? PlanExpiry { get; set; }

    [Required]
    public int ArenaId { get; set; }

    public string? Status { get; set; }
}