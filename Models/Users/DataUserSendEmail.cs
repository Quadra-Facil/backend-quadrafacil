namespace QuadraFacil_backend.Models.Users;

// Modelo de dados para enviar o e-mail
public class SendEmailRequest
{
    public string? ToEmail { get; set; }
    public string? NomeUsuario { get; set; }
    public string? LinkRecuperacao { get; set; }
}
