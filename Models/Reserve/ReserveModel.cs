using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QuadraFacil_backend.Models.Arena;
using QuadraFacil_backend.Models.Arena.Space;
using QuadraFacil_backend.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuadraFacil_backend.Models.Reserve;

public class ReserveModel
{
    [Key]
    public int Id_reserve { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ArenaId { get; set; }

    [Required]
    public int SpaceId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? DataReserve { get; set; }

    [Required]
    public TimeSpan? TimeInitial { get; set; }

    [Required]
    public TimeSpan? TimeFinal { get; set; }

    [Required]
    public string? Status { get; set; }//pendente - marcado - recusado

    [Required]
    public string? Observation { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [ForeignKey("ArenaId")]
    public ArenaModel? Arena { get; set; }

    [ForeignKey("SpaceId")]
    public SpaceModel? Space { get; set; }
}
