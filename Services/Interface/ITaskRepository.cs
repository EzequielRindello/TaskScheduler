using TaskScheduler.Models;

namespace TaskScheduler.Services.Interface;

public interface ITaskRepository
{
    void Save(IEnumerable<TaskItem> tasks);
    List<TaskItem> Load();
}
