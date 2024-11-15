using Microsoft.AspNetCore.Mvc;
using QuadraFacil_backend.Models.Users;
using QuadraFacil_backend.Services;
using System.Threading.Tasks;

namespace QuadraFacil_backend.Controllers.Users
{
    [Route("api/")]
    [ApiController]
    public class SendEmailController(IEmailService emailService) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;

        [HttpPost("email-send")]
        public async Task<IActionResult> SendRecoveryEmail([FromBody] SendEmailRequest request)
        {
            if (string.IsNullOrEmpty(request.ToEmail) || string.IsNullOrEmpty(request.NomeUsuario) || string.IsNullOrEmpty(request.LinkRecuperacao))
            {
                return BadRequest("Por favor, forneça todos os dados necessários.");
            }

            try
            {
                // Envia o e-mail de recuperação com o HTML predefinido
                await _emailService.EnviarEmailRecuperacaoSenha(request.ToEmail, request.NomeUsuario, request.LinkRecuperacao);
                return Ok($"E-mail para {request.ToEmail} enviado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}
