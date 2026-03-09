using TaskScheduler.Managers;
using TaskScheduler.Models;

namespace TaskScheduler.UI;

public class ConsoleMenu
{
    private readonly TaskManager _taskManager;

    public ConsoleMenu(TaskManager taskManager)
    {
        _taskManager = taskManager;
    }

    public async Task RunAsync()
    {
        _taskManager.LoadFromFile();
        _taskManager.CheckReminders();

        while (true)
        {
            PrintMenu();
            var choice = Console.ReadLine()?.Trim();

            try
            {
                switch (choice)
                {
                    case "1":
                        await AddTaskAsync();
                        break;
                    case "2":
                        _taskManager.DisplayTasks();
                        break;
                    case "3":
                        MarkTaskComplete();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "5":
                        _taskManager.ExportToCsv();
                        break;
                    case "6":
                        Console.WriteLine("Goodbye.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please choose 1–6.");
                        break;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("══════════════════════════════");
        Console.WriteLine("   Task Scheduler & Reminders ");
        Console.WriteLine("══════════════════════════════");
        Console.WriteLine("  1. Add Task");
        Console.WriteLine("  2. View Tasks");
        Console.WriteLine("  3. Mark Task Complete");
        Console.WriteLine("  4. Delete Task");
        Console.WriteLine("  5. Export to CSV");
        Console.WriteLine("  6. Exit");
        Console.WriteLine("══════════════════════════════");
        Console.Write("Select option: ");
    }

    private async Task AddTaskAsync()
    {
        Console.WriteLine("\n── New Task ──");
        var title = InputHelper.ReadRequiredString("Title: ");
        var description = InputHelper.ReadOptionalString("Description: ");
        var dueDate = InputHelper.ReadFutureDateTime("Due Date (yyyy-MM-dd HH:mm): ");
        var priority = InputHelper.ReadEnum<PriorityLevel>("Priority:");
        var reminderHours = InputHelper.ReadInt("Reminder hours before due (0 = none): ", 0, 8760);

        var task = new TaskItem
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            Priority = priority,
            Reminder = new ReminderSettings(reminderHours)
        };

        await _taskManager.AddTaskAsync(task);
        Console.WriteLine($"Task \"{title}\" added.");
    }

    private void MarkTaskComplete()
    {
        _taskManager.DisplayTasks();
        if (_taskManager.Tasks.Count == 0) return;

        var index = InputHelper.ReadInt("Task number to complete: ", 1, _taskManager.Tasks.Count);
        _taskManager.MarkTaskComplete(index - 1);
    }

    private void DeleteTask()
    {
        _taskManager.DisplayTasks();
        if (_taskManager.Tasks.Count == 0) return;

        var index = InputHelper.ReadInt("Task number to delete: ", 1, _taskManager.Tasks.Count);
        _taskManager.DeleteTask(index - 1);
    }
}
