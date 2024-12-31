using System.ComponentModel.DataAnnotations;

namespace backend_quadrafacil.Models.Plan;
public class GetPlanForUserModel
{

    [Required]
    public int ArenaId { get; set; }
}
