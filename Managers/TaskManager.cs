using TaskScheduler.Models;
using TaskScheduler.Services;

namespace TaskScheduler.Managers;

public class TaskManager
{
    private readonly List<TaskItem> _tasks;
    private readonly ITaskRepository _repository;
    private readonly IEmailService _emailService;
    private readonly CsvExporter _csvExporter;

    public IReadOnlyList<TaskItem> Tasks => _tasks.AsReadOnly();

    public TaskManager(ITaskRepository repository, IEmailService emailService, CsvExporter csvExporter)
    {
        _repository = repository;
        _emailService = emailService;
        _csvExporter = csvExporter;
        _tasks = [];
    }

    public void LoadFromFile()
    {
        var loaded = _repository.Load();
        _tasks.Clear();
        _tasks.AddRange(loaded);
        Console.WriteLine($"[Storage] Loaded {_tasks.Count} task(s).");
    }

    public async Task AddTaskAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);
        _tasks.Add(task);
        SaveToFile();

        await _emailService.SendEmailAsync(
            subject: $"Task Created: {task.Title}",
            body: BuildTaskEmailBody(task)
        );
    }

    public void DisplayTasks()
    {
        if (_tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
            return;
        }

        var sorted = _tasks
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToList();

        Console.WriteLine();
        Console.WriteLine($"{"#",-4} {"Title",-25} {"Due Date",-20} {"Priority",-10} {"Status",-12} {"Reminder"}");
        Console.WriteLine(new string('-', 90));

        for (int i = 0; i < sorted.Count; i++)
        {
            var task = sorted[i];
            // Use original index for commands that reference by position
            var originalIndex = _tasks.IndexOf(task);
            var status = task.IsCompleted ? "Completed" : (task.IsOverdue ? "Overdue" : "Pending");
            var color = task.IsCompleted ? ConsoleColor.Green : (task.IsOverdue ? ConsoleColor.Red : ConsoleColor.White);

            Console.ForegroundColor = color;
            Console.WriteLine($"{originalIndex + 1,-4} {task.Title,-25} {task.DueDate,-20:yyyy-MM-dd HH:mm} {task.Priority,-10} {status,-12} {task.Reminder}");
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    public void MarkTaskComplete(int taskIndex)
    {
        ValidateIndex(taskIndex);
        _tasks[taskIndex].IsCompleted = true;
        SaveToFile();
        Console.WriteLine($"Task \"{_tasks[taskIndex].Title}\" marked as complete.");
    }

    public void DeleteTask(int taskIndex)
    {
        ValidateIndex(taskIndex);
        var title = _tasks[taskIndex].Title;
        _tasks.RemoveAt(taskIndex);
        SaveToFile();
        Console.WriteLine($"Task \"{title}\" deleted.");
    }

    public void SaveToFile()
    {
        _repository.Save(_tasks);
    }

    public void ExportToCsv()
    {
        _csvExporter.Export(_tasks);
    }

    public void CheckReminders()
    {
        var due = _tasks.Where(t => t.IsReminderDue).ToList();
        if (due.Count == 0) return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[Reminders] {due.Count} task(s) upcoming:");
        foreach (var task in due)
            Console.WriteLine($"  - \"{task.Title}\" due {task.DueDate:yyyy-MM-dd HH:mm}");
        Console.ResetColor();
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= _tasks.Count)
            throw new ArgumentOutOfRangeException(nameof(index), $"Task index must be between 1 and {_tasks.Count}.");
    }

    private static string BuildTaskEmailBody(TaskItem task) =>
        $"""
        <div style="font-size:14px; line-height:1.6;">
            <p>A new task has been created in your scheduler.</p>

            <table style="width:100%; border-collapse:collapse; margin-top:10px;">
                <tr>
                    <td style="padding:6px; font-weight:bold;">Title</td>
                    <td style="padding:6px;">{task.Title}</td>
                </tr>
                <tr>
                    <td style="padding:6px; font-weight:bold;">Description</td>
                    <td style="padding:6px;">{task.Description}</td>
                </tr>
                <tr>
                    <td style="padding:6px; font-weight:bold;">Due Date</td>
                    <td style="padding:6px;">{task.DueDate:dddd, MMMM dd, yyyy HH:mm}</td>
                </tr>
                <tr>
                    <td style="padding:6px; font-weight:bold;">Priority</td>
                    <td style="padding:6px;">{task.Priority}</td>
                </tr>
                <tr>
                    <td style="padding:6px; font-weight:bold;">Reminder</td>
                    <td style="padding:6px;">{task.Reminder}</td>
                </tr>
            </table>
        </div>
        """;
}
