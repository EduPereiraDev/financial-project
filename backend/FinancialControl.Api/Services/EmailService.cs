using SendGrid;
using SendGrid.Helpers.Mail;

namespace FinancialControl.Api.Services;

public interface IEmailService
{
    Task SendInvitationEmailAsync(string toEmail, string toName, string inviterName, string accountName, string token, string role);
}

public class EmailService : IEmailService
{
    private readonly string _apiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly string _frontendUrl;

    public EmailService(IConfiguration configuration)
    {
        _apiKey = configuration["SendGrid:ApiKey"] ?? "";
        _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@financialcontrol.com";
        _fromName = configuration["SendGrid:FromName"] ?? "Financial Control";
        _frontendUrl = configuration["Frontend:Url"] ?? "http://localhost:5173";
    }

    public async Task SendInvitationEmailAsync(string toEmail, string toName, string inviterName, string accountName, string token, string role)
    {
        // Se não tiver API key configurada, apenas loga (para desenvolvimento)
        if (string.IsNullOrEmpty(_apiKey))
        {
            Console.WriteLine($"[EMAIL] Convite para {toEmail} - Token: {token}");
            Console.WriteLine($"[EMAIL] Link: {_frontendUrl}/accept-invitation/{token}");
            return;
        }

        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(toEmail, toName);
        var subject = $"Convite para gerenciar a conta {accountName}";

        var invitationLink = $"{_frontendUrl}/accept-invitation/{token}";

        var htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .container {{
            background-color: #ffffff;
            border-radius: 8px;
            padding: 40px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .header h1 {{
            color: #2563eb;
            margin: 0;
            font-size: 28px;
        }}
        .content {{
            margin-bottom: 30px;
        }}
        .content p {{
            margin: 15px 0;
            font-size: 16px;
        }}
        .highlight {{
            background-color: #f3f4f6;
            padding: 15px;
            border-radius: 6px;
            margin: 20px 0;
        }}
        .highlight strong {{
            color: #2563eb;
        }}
        .button {{
            display: inline-block;
            background-color: #2563eb;
            color: #ffffff !important;
            text-decoration: none;
            padding: 14px 32px;
            border-radius: 6px;
            font-weight: 600;
            text-align: center;
            margin: 20px 0;
        }}
        .button:hover {{
            background-color: #1d4ed8;
        }}
        .footer {{
            margin-top: 40px;
            padding-top: 20px;
            border-top: 1px solid #e5e7eb;
            text-align: center;
            color: #6b7280;
            font-size: 14px;
        }}
        .warning {{
            background-color: #fef3c7;
            border-left: 4px solid #f59e0b;
            padding: 12px;
            margin: 20px 0;
            font-size: 14px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📨 Convite para Conta Compartilhada</h1>
        </div>
        
        <div class='content'>
            <p>Olá{(string.IsNullOrEmpty(toName) ? "" : $" {toName}")},</p>
            
            <p><strong>{inviterName}</strong> convidou você para gerenciar a conta financeira <strong>{accountName}</strong>.</p>
            
            <div class='highlight'>
                <p><strong>Nível de acesso:</strong> {GetRoleLabel(role)}</p>
                <p style='margin: 5px 0; font-size: 14px; color: #6b7280;'>{GetRoleDescription(role)}</p>
            </div>
            
            <p>Para aceitar este convite e começar a gerenciar as finanças juntos, clique no botão abaixo:</p>
            
            <div style='text-align: center;'>
                <a href='{invitationLink}' class='button'>Aceitar Convite</a>
            </div>
            
            <div class='warning'>
                <strong>⚠️ Atenção:</strong> Este convite expira em 7 dias. Se você não aceitar dentro deste prazo, será necessário solicitar um novo convite.
            </div>
            
            <p style='font-size: 14px; color: #6b7280;'>
                Se o botão não funcionar, copie e cole este link no seu navegador:<br>
                <a href='{invitationLink}' style='color: #2563eb; word-break: break-all;'>{invitationLink}</a>
            </p>
        </div>
        
        <div class='footer'>
            <p>Este é um email automático do Financial Control.</p>
            <p>Se você não esperava este convite, pode ignorar este email com segurança.</p>
        </div>
    </div>
</body>
</html>";

        var plainTextContent = $@"
Convite para Conta Compartilhada

Olá{(string.IsNullOrEmpty(toName) ? "" : $" {toName}")},

{inviterName} convidou você para gerenciar a conta financeira {accountName}.

Nível de acesso: {GetRoleLabel(role)}
{GetRoleDescription(role)}

Para aceitar este convite, acesse o link abaixo:
{invitationLink}

⚠️ Atenção: Este convite expira em 7 dias.

---
Financial Control
";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        try
        {
            var response = await client.SendEmailAsync(msg);
            
            if (response.StatusCode != System.Net.HttpStatusCode.OK && 
                response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                var body = await response.Body.ReadAsStringAsync();
                throw new Exception($"Erro ao enviar email: {response.StatusCode} - {body}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EMAIL ERROR] Falha ao enviar email para {toEmail}: {ex.Message}");
            throw;
        }
    }

    private static string GetRoleLabel(string role)
    {
        return role switch
        {
            "Owner" => "Proprietário",
            "Editor" => "Editor",
            "Viewer" => "Visualizador",
            _ => role
        };
    }

    private static string GetRoleDescription(string role)
    {
        return role switch
        {
            "Owner" => "Controle total da conta, incluindo gerenciar membros e configurações",
            "Editor" => "Pode adicionar, editar e excluir transações",
            "Viewer" => "Pode apenas visualizar transações e relatórios",
            _ => ""
        };
    }
}
