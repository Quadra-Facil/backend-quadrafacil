using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuadraFacil_backend.Models.Arena;

public class AdressArena
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string? State { get; set; }

    [Required]
    [MaxLength(100)]
    public string? City { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Street { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Neighborhood { get; set; }

    [Required]
    public int? Number { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Reference { get; set; }

    [ForeignKey("Arena")]
    public int ArenaId { get; set; }

    //[JsonIgnore]
    public ArenaModel? Arena { get; set; }

}
