namespace TaskScheduler.Models;

public class TaskItem
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public PriorityLevel Priority { get; set; }
    public ReminderSettings Reminder { get; set; }

    public bool IsOverdue => !IsCompleted && DueDate < DateTime.Now;

    public bool IsReminderDue =>
        !IsCompleted &&
        Reminder.HoursBeforeDue > 0 &&
        DateTime.Now >= Reminder.GetReminderTime(DueDate) &&
        DateTime.Now < DueDate;
}
