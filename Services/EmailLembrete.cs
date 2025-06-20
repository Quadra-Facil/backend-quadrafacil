using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace QuadraFacil_backend.Services
{
    public interface IEmailServiceLembrete
    {
        Task EnviarEmailLembrete(
            string toEmail,
            string nomeUsuario,
            string linkPagamento,
            TipoLembrete tipoLembrete,
            string nomeArena,
            string turmaAluno,
            string telefoneArena,
            decimal valorPagamento);
    }

    public enum TipoLembrete
    {
        VencendoEm5Dias,
        VencendoEm3Dias,
        VencendoHoje,
        Vencido
    }

    public class EmailServiceLembrete : IEmailServiceLembrete
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUser = "quadrafacilatendimento@gmail.com";
        private readonly string _smtpPassword = "zsoc jmrh lfge mxit";

        public async Task EnviarEmailLembrete(
            string toEmail,
            string nomeUsuario,
            string linkPagamento,
            TipoLembrete tipoLembrete,
            string nomeArena,
            string turmaAluno,
            string telefoneArena,
            decimal valorPagamento)
        {
            var (assunto, mensagem) = ObterConteudoEmail(
                nomeUsuario,
                tipoLembrete,
                nomeArena,
                turmaAluno,
                valorPagamento);

            var whatsappLink = $"https://wa.me/{telefoneArena}?text=Olá {nomeArena}, tenho uma dúvida sobre o pagamento da minha turma {turmaAluno}.";

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Quadra Fácil", _smtpUser));
            emailMessage.To.Add(new MailboxAddress(nomeUsuario, toEmail));
            emailMessage.Subject = assunto;

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <html lang='pt-br'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>{assunto}</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                color: #333;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}

                            .email-container {{
                                width: 100%;
                                max-width: 600px;
                                margin: 0 auto;
                                background-color: #ffffff;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                                margin-top: 5%;
                            }}

                            .header {{
                                text-align: center;
                                padding-bottom: 20px;
                            }}

                            .header h1 {{
                                color: #FF8A5B;
                                font-size: 24px;
                                margin: 0;
                            }}

                            .content {{
                                font-size: 16px;
                                line-height: 1.6;
                                color: #555;
                            }}

                            .content p {{
                                margin: 10px 0;
                            }}

                            .content p a{{
                                color: white
                            }}

                            .btn {{
                                display: inline-block;
                                background-color: #FF8A5B;
                                color: #fff !important;
                                padding: 12px 20px;
                                font-size: 16px;
                                text-decoration: none;
                                border-radius: 5px;
                                margin-top: 20px;
                            }}

                            .footer {{
                                text-align: center;
                                font-size: 12px;
                                color: #999;
                                margin-top: 20px;
                            }}

                            .footer a {{
                                color: #FF8A5B;
                                text-decoration: none;
                            }}

                            .urgente {{
                                color: #d9534f;
                                font-weight: bold;
                            }}

                            .info-box {{
                                background-color: #f8f9fa;
                                border-left: 4px solid #FF8A5B;
                                padding: 10px 15px;
                                margin: 15px 0;
                            }}

                            .info-label {{
                                font-weight: bold;
                                color: #555;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <div class='header'>
                                <h1>{assunto}</h1>
                            </div>
                            <div class='content'>
                                {mensagem}
                                
                                <div class='info-box'>
                                    <p><span class='info-label'>Arena:</span> {nomeArena}</p>
                                    <p><span class='info-label'>Turma:</span> {turmaAluno}</p>
                                    <p><span class='info-label'>Valor:</span> R$ {valorPagamento.ToString("N2")}</p>
                                </div>
                                
                                <p><a href='{linkPagamento}' class='btn'>Realizar Pagamento</a></p>
                                
                                <p>Caso já tenha efetuado o pagamento, por favor, desconsidere este e-mail.</p>
                                <p>Se precisar de ajuda ou tiver alguma dúvida, estamos à disposição:</p>
                                <p><a href='{whatsappLink}' class='btn'>Falar com a Arena no WhatsApp</a></p>
                            </div>
                            <div class='footer'>
                                <p>Atenciosamente,</p>
                                <p><strong>Equipe Quadra Fácil</strong></p>
                                <p><a href='https://api.whatsapp.com/send/?phone=5511993536138&text=Ol%C3%A1,%2520tenho%2520uma%2520d%C3%BAvida%2520sobre%2520o%2520pagamento%2520da%2520minha%2520associa%C3%A7%C3%A3o/aula.%2520Podem%2520me%2520ajudar?&type=phone_number&app_absent=0'>Central de Suporte</a></p>
                            </div>
                        </div>
                    </body>
                </html>"
            };

            emailMessage.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUser, _smtpPassword);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        private (string assunto, string mensagem) ObterConteudoEmail(
            string nomeUsuario,
            TipoLembrete tipoLembrete,
            string nomeArena,
            string turmaAluno,
            decimal valorPagamento)
        {
            var valorFormatado = valorPagamento.ToString("N2");

            switch (tipoLembrete)
            {
                case TipoLembrete.VencendoEm5Dias:
                    return (
                        "Pagamento Vencendo em 5 Dias",
                        $@"<p>Olá {nomeUsuario},</p>
                        <p>Identificamos que você possui um pagamento de <strong>R$ {valorFormatado}</strong> para a turma <strong>{turmaAluno}</strong> na arena <strong>{nomeArena}</strong> que vencerá em <strong>5 dias</strong>.</p>
                        <p>Para sua comodidade, recomendamos realizar o pagamento antecipadamente e garantir sua participação.</p>"
                    );

                case TipoLembrete.VencendoEm3Dias:
                    return (
                        "Pagamento Vencendo em 3 Dias",
                        $@"<p>Olá {nomeUsuario},</p>
                        <p>Seu pagamento de <strong>R$ {valorFormatado}</strong> para a turma <strong>{turmaAluno}</strong> na arena <strong>{nomeArena}</strong> está prestes a vencer em <strong>3 dias</strong>.</p>
                        <p>Recomendamos que realize o pagamento o quanto antes para evitar qualquer inconveniente.</p>"
                    );

                case TipoLembrete.VencendoHoje:
                    return (
                        "Pagamento Vence Hoje!",
                        $@"<p>Olá {nomeUsuario},</p>
                        <p class='urgente'>Seu pagamento de <strong>R$ {valorFormatado}</strong> para a turma <strong>{turmaAluno}</strong> na arena <strong>{nomeArena}</strong> <strong>vence hoje</strong>!</p>
                        <p>Para manter sua participação ativa, por favor, realize o pagamento imediatamente.</p>"
                    );

                case TipoLembrete.Vencido:
                    return (
                        "Pagamento Vencido",
                        $@"<p>Olá {nomeUsuario},</p>
                        <p class='urgente'>Seu pagamento de <strong>R$ {valorFormatado}</strong> para a turma <strong>{turmaAluno}</strong> na arena <strong>{nomeArena}</strong> está <strong>vencido</strong>!</p>
                        <p>Para regularizar sua situação e evitar o cancelamento da matrícula, por favor, realize o pagamento imediatamente.</p>
                        <p>Entre em contato com sua arena caso já tenha efetuado o pagamento.</p>"
                    );

                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoLembrete), tipoLembrete, null);
            }
        }
    }
}