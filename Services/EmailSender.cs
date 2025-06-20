using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace QuadraFacil_backend.Services
{
    public interface IEmailService
    {
        Task EnviarEmailRecuperacaoSenha(string toEmail, string nomeUsuario, string linkRecuperacao);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Servidor SMTP
        private readonly int _smtpPort = 587; // Porta SMTP
        private readonly string _smtpUser = "quadrafacilatendimento@gmail.com"; // E-mail de envio
        private readonly string _smtpPassword = "zsoc jmrh lfge mxit"; // Senha de app (senha gerada no Gmail)

        public async Task EnviarEmailRecuperacaoSenha(string toEmail, string nomeUsuario, string linkRecuperacao)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Quadra Fácil", _smtpUser));
            emailMessage.To.Add(new MailboxAddress(nomeUsuario, toEmail));
            emailMessage.Subject = "Recuperação de Senha";


            // Corpo do e-mail com HTML
            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <html lang='pt-br'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Recuperação de Senha</title>
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
                        </style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <div class='header'>
                                <h1>Recuperação de Senha</h1>
                            </div>
                            <div class='content'>
                                <p>Olá,</p>
                                <p>Recebemos uma solicitação para a recuperação de sua senha. Se você não foi quem solicitou, por favor, ignore este e-mail.</p>
                                <p>Caso tenha feito a solicitação, clique no botão abaixo para criar uma nova senha:</p>
                                <p><a href='{linkRecuperacao}' class='btn'>Recuperar Senha</a></p>
                                <p>Se você tiver algum problema ou dúvida, não hesite em nos contatar.</p>
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

            // Envio do e-mail usando o MailKit
            emailMessage.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                // conectar com STARTTLS (o Gmail exige STARTTLS na porta 587)
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
    }
}
