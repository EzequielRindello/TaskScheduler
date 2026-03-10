namespace TaskScheduler.Services.Interface;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string body);
}
