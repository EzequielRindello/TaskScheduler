using System.Text.Json;
using System.Text.Json.Serialization;
using TaskScheduler.Models;
using TaskScheduler.Services.Interface;

namespace TaskScheduler.Services;

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public JsonTaskRepository(string filePath = "tasks.json")
    {
        _filePath = filePath;
    }

    public void Save(IEnumerable<TaskItem> tasks)
    {
        try
        {
            var json = JsonSerializer.Serialize(tasks.ToList(), JsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"Failed to save tasks: {ex.Message}", ex);
        }
    }

    public List<TaskItem> Load()
    {
        if (!File.Exists(_filePath))
            return [];

        try
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json, JsonOptions) ?? [];
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[Warning] Could not parse tasks file: {ex.Message}");
            return [];
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[Warning] Could not read tasks file: {ex.Message}");
            return [];
        }
    }
}
