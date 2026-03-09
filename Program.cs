using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskScheduler.Configuration;
using TaskScheduler.Managers;
using TaskScheduler.Services;
using TaskScheduler.UI;

DotEnv.Load(new DotEnvOptions(probeForEnv: true));

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var smtpSettings = configuration.GetSection("Smtp").Get<SmtpSettings>() ?? new SmtpSettings();

Console.WriteLine($"SMTP USER: {smtpSettings.Username}");
Console.WriteLine($"SMTP TO: {smtpSettings.ToAddress}");

var services = new ServiceCollection();

services.AddSingleton(smtpSettings);
services.AddSingleton<IEmailService, EmailService>();
services.AddSingleton<ITaskRepository>(new JsonTaskRepository("tasks.json"));
services.AddSingleton(new CsvExporter("tasks_export.csv"));
services.AddSingleton<TaskManager>();
services.AddSingleton<ConsoleMenu>();

var provider = services.BuildServiceProvider();

var menu = provider.GetRequiredService<ConsoleMenu>();

try
{
    await menu.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"[Fatal] Unexpected error: {ex.Message}");
    Environment.Exit(1);
}