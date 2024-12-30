using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Plan;
public class GetPlanOfArenaIdModel
{

    [Required]
    public int ArenaId { get; set; }
}
