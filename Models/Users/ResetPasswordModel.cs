using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Users;

public class ResetPasswordModel
{
    [Required]
    public string? Password { get; set; }
}
