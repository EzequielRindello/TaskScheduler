using System.Text;
using TaskScheduler.Models;

namespace TaskScheduler.Services;

public class CsvExporter
{
    private readonly string _outputPath;

    public CsvExporter(string outputPath = "tasks_export.csv")
    {
        _outputPath = outputPath;
    }

    public void Export(IEnumerable<TaskItem> tasks)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Title,Description,DueDate,IsCompleted,Priority,ReminderHoursBeforeDue");

        foreach (var task in tasks)
        {
            sb.AppendLine(string.Join(",",
                EscapeCsv(task.Title),
                EscapeCsv(task.Description),
                task.DueDate.ToString("yyyy-MM-dd HH:mm"),
                task.IsCompleted,
                task.Priority,
                task.Reminder.HoursBeforeDue
            ));
        }

        try
        {
            File.WriteAllText(_outputPath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine($"[Export] Tasks exported to {_outputPath}");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Failed to export CSV: {ex.Message}", ex);
        }
    }

    private static string EscapeCsv(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";

        return value;
    }
}
