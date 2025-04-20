using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Reserve;

public class DeleteReserveWithModel
{

  [Required]
  public int reserveId { get; set; }

}