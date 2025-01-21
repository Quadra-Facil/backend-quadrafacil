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

    public string? Status { get; set; }//pendente - marcado - recusado

    public string? TypeReserve { get; set; }
    public string? Observation { get; set; }

    // public ArenaModel? Arenas { get; set; }
}
