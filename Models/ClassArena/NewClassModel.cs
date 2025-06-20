using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.ClassArena;

public class ClassArenaModel
{
  [Key]
  public int Id { get; set; }

  [Required]
  public string? NameClass { get; set; }

  public string? Teacher { get; set; }

  public string? PhoneTeacher { get; set; }
  public string? CreateClass { get; set; }

  public int? ArenaId { get; set; }
}