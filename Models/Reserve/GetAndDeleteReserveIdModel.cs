using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Reserve;

public class GetAndDeleteReserveeModel
{

  [Required]
  public string? Observation { get; set; }

}