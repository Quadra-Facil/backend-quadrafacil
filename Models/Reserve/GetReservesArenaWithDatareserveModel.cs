using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Reserve;

public class GetReservesArenaWithDatareserveModel
{

    [Required]
    public int ArenaId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? DataReserve { get; set; }

    // public ICollection<ReserveModel>? Reserve { get; set; }
}
