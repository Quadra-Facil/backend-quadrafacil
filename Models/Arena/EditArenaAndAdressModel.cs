using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Arena;

public class EditArenaAndAdressModel
{
    [Required]
    public int ArenaId { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required]
    [Phone]
    public string? Phone { get; set; }

    [Required]
    public int ValueHour { get; set; }

    //adress

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
}
