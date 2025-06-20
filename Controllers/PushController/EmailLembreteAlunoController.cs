using Microsoft.AspNetCore.Mvc;
using QuadraFacil_backend.Services;
using System;
using System.Threading.Tasks;

namespace QuadraFacil_backend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LembretePagamentoController : ControllerBase
  {
    private readonly IEmailServiceLembrete _emailService;

    public LembretePagamentoController(IEmailServiceLembrete emailService)
    {
      _emailService = emailService;
    }

    [HttpPost("enviar-lembrete")]
    public async Task<IActionResult> EnviarLembrete([FromBody] LembretePagamentoDTO lembreteDto)
    {
      try
      {
        // Validar o DTO
        if (string.IsNullOrEmpty(lembreteDto.Email) ||
            string.IsNullOrEmpty(lembreteDto.Username) ||
            string.IsNullOrEmpty(lembreteDto.TipoLembrete) ||
            string.IsNullOrEmpty(lembreteDto.LinkPagamento) ||
            string.IsNullOrEmpty(lembreteDto.NomeArena) ||
            string.IsNullOrEmpty(lembreteDto.TurmaAluno) ||
            string.IsNullOrEmpty(lembreteDto.TelefoneArena) ||
            lembreteDto.ValorPagamento <= 0)
        {
          return BadRequest("Todos os campos são obrigatórios e o valor deve ser maior que zero.");
        }

        // Converter string para enum TipoLembrete
        if (!Enum.TryParse(lembreteDto.TipoLembrete, out TipoLembrete tipoLembrete))
        {
          return BadRequest("Tipo de lembrete inválido. Valores aceitos: VencendoEm5Dias, VencendoEm3Dias, VencendoHoje, Vencido");
        }

        // Enviar e-mail
        await _emailService.EnviarEmailLembrete(
            lembreteDto.Email,
            lembreteDto.Username,
            lembreteDto.LinkPagamento,
            tipoLembrete,
            lembreteDto.NomeArena,
            lembreteDto.TurmaAluno,
            lembreteDto.TelefoneArena,
            lembreteDto.ValorPagamento);

        return Ok(new { message = "E-mail de lembrete enviado com sucesso!" });
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Erro ao enviar e-mail de lembrete: {ex.Message}");
      }
    }
  }

  public class LembretePagamentoDTO
  {
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? TipoLembrete { get; set; }
    public string? LinkPagamento { get; set; }
    public string? NomeArena { get; set; }
    public string? TurmaAluno { get; set; }
    public string? TelefoneArena { get; set; }
    public decimal ValorPagamento { get; set; }
  }
}