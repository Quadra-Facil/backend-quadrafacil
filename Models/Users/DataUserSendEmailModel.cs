using System.ComponentModel.DataAnnotations;

namespace QuadraFacil_backend.Models.Users;

public class SendEmailRequest
{
    [Required]
    [EmailAddress]
    public string? ToEmail { get; set; }
    public string? NomeUsuario { get; set; }
    [Required]
    public string? LinkRecuperacao { get; set; }
}
