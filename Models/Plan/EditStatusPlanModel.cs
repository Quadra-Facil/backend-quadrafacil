using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Plan;
public class EditStatusPlanModel
{

    [Required]
    public int ArenaId { get; set; }

    public string? PlanSelect { get; set; }
    public string? PlanExpiry { get; set; }
    public string? Status { get; set; }
}
