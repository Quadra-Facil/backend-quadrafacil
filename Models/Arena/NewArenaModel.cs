using QuadraFacil_backend.Models.Arena.Space;
using QuadraFacil_backend.Models.Reserve;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuadraFacil_backend.Models.Arena;

public class ArenaModel
{
    [Key]
    public int Id { get; set; } 

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [Phone]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Status { get; set; }

    [Required]
    public int ValueHour { get; set; }

    public ReserveModel? Reserve { get; set; }

    //[JsonIgnore]
    public ICollection<AdressArena>? AdressArenas { get; set; }

    public ICollection<SpaceModel>? Spaces { get; set; }

}
