using TaskScheduler.Models;

namespace TaskScheduler.Services;

public interface ITaskRepository
{
    void Save(IEnumerable<TaskItem> tasks);
    List<TaskItem> Load();
}
