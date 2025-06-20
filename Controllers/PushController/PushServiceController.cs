using backend_quadrafacil.Models;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Data;
using System.Text.Json;
using WebPush;
using WP = WebPush.PushSubscription;

namespace backend_quadrafacil.Controllers.Notification
{
  [ApiController]
  [Route("api/[controller]")]
  public class NotificationsController : ControllerBase
  {
    private readonly AppDbContext _appDbContext;
    private readonly VapidDetails _vapidDetails;

    public NotificationsController(AppDbContext appDbContext, IConfiguration configuration)
    {
      _appDbContext = appDbContext;

      _vapidDetails = new VapidDetails(
          "mailto:quadrafacilatendimento@gmail.com",
          configuration["Vapid:PublicKey"],
          configuration["Vapid:PrivateKey"]
      );
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> Subscribe([FromBody] PushSubscriptionModel subscriptionDto)
    {
      try
      {
        // Verifica se já existe uma subscription para este aluno
        var existingSubscription = await _appDbContext.PushSubscriptions
            .FirstOrDefaultAsync(s => s.AlunoId == subscriptionDto.AlunoId);

        if (existingSubscription != null)
        {
          // Atualiza a subscription existente
          existingSubscription.Endpoint = subscriptionDto.Endpoint;
          existingSubscription.P256DH = subscriptionDto.P256DH;
          existingSubscription.Auth = subscriptionDto.Auth;
        }
        else
        {
          // Cria uma nova subscription
          var newSubscription = new PushSubscriptionModel
          {
            Endpoint = subscriptionDto.Endpoint,
            P256DH = subscriptionDto.P256DH,
            Auth = subscriptionDto.Auth,
            AlunoId = subscriptionDto.AlunoId
          };
          _appDbContext.PushSubscriptions.Add(newSubscription);
        }

        await _appDbContext.SaveChangesAsync();
        return Ok("Subscription salva com sucesso.");
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Erro ao salvar subscription: {ex.Message}");
      }
    }


    [Authorize]
    [HttpPost("send-payment-notification")]
    public async Task<IActionResult> SendPaymentNotification([FromBody] PaymentNotificationDto dto)
    {
      try
      {
        var subscription = await _appDbContext.PushSubscriptions
            .FirstOrDefaultAsync(s => s.AlunoId == dto.IdAluno);

        if (subscription == null)
          return NotFound("Inscrição de push não encontrada para este aluno.");

        var pushSubscription = new WP(subscription.Endpoint, subscription.P256DH, subscription.Auth);

        var payload = JsonSerializer.Serialize(new
        {
          title = dto.Title,
          body = dto.Body,
          link = dto.Link,
          pix = dto.Pix,
          id_aluno = dto.IdAluno
        });

        var webPushClient = new WebPushClient();
        webPushClient.SendNotification(pushSubscription, payload, _vapidDetails);

        return Ok("Notificação enviada com sucesso.");
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Erro ao enviar notificação: {ex.Message}");
      }
    }
  }

  public class PaymentNotificationDto
  {
    public string? Title { get; set; }
    public string? Body { get; set; }
    public string? Link { get; set; }
    public string? Pix { get; set; }
    public int IdAluno { get; set; }
  }
}
