namespace backend_quadrafacil.Models
{
  public class PushSubscriptionModel
  {
    public int Id { get; set; }
    public string? Endpoint { get; set; }
    public string? P256DH { get; set; }
    public string? Auth { get; set; }
    public int AlunoId { get; set; }
  }
}
