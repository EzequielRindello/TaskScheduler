using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskScheduler.Configuration;
using TaskScheduler.Managers;
using TaskScheduler.Services;
using TaskScheduler.Services.Interface;
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
services.AddSingleton(new CsvExporter(@"C:\Users\your user \Downloads\tasks_export.csv")); // add here where you want your file
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