using System.Net;
using System.Net.Mail;
using TaskScheduler.Configuration;
using TaskScheduler.Services.Interface;

namespace TaskScheduler.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;

    public EmailService(SmtpSettings settings)
    {
        _settings = settings;
    }

    public async Task SendEmailAsync(string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(_settings.Host))
        {
            Console.WriteLine("[Email] SMTP host is not configured. Skipping email.");
            return;
        }

        try
        {
            using var client = CreateSmtpClient();
            using var message = CreateEmailMessage(subject, body);

            await client.SendMailAsync(message);
            Console.WriteLine($"[Email] Sent: {subject}");
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"[Email] Failed to send: {ex.Message}");
        }
    }

    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password)
        };
    }

    private MailMessage CreateEmailMessage(string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_settings.FromAddress),
            Subject = subject,
            Body = BuildTemplate(body),
            IsBodyHtml = true
        };

        message.To.Add(_settings.ToAddress);

        return message;
    }

    private string BuildTemplate(string content)
    {
        return $"""
        <html>
        <body style="font-family: Arial; background:#f4f4f4; padding:20px;">
            <div style="max-width:600px; margin:auto; background:white; padding:20px; border-radius:8px;">
                <h2 style="color:#333;">Task Scheduler Notification</h2>
                <div style="font-size:14px; color:#444;">
                    {content}
                </div>
                <hr/>
                <p style="font-size:12px; color:#888;">
                    Generated automatically by TaskScheduler - Do not respond
                </p>
            </div>
        </body>
        </html>
        """;
    }
}