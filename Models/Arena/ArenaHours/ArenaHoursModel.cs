using QuadraFacil_backend.Models.Reserve;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class ArenaHoursModel
{
  [Key]
  public int Id { get; set; }

  [Required]
  public int ArenaId { get; set; } // Relation with the arena

  [Required]
  public List<int>? WeekDays { get; set; } // List of weekdays (e.g. [1, 2, 3, 4, 5])

  [Required]
  public TimeSpan StartTime { get; set; } // Start time

  [Required]
  public TimeSpan EndTime { get; set; } // End time
}


