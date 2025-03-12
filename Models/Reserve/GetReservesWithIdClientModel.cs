using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Reserve;

public class GetReservesWithIdClientModel
{

    [Required]
    public int ClientId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? DataReserve { get; set; }
    // public ICollection<ReserveModel>? Reserve { get; set; }
}
