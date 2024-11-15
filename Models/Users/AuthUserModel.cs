using QuadraFacil_backend.Models.Reserve;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuadraFacil_backend.Models.Users;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(100)]
    [Required]
    [MinLength(6)]
    public string? Password { get; set; }

    [Phone]
    public string? Phone { get; set; }

    [MaxLength(100)]
    public string? Role { get; set; }    
    
    [MaxLength(10)]
    [JsonIgnore]
    public int? ArenaId { get; set; }

    public ReserveModel? Reserve { get; set; }

}