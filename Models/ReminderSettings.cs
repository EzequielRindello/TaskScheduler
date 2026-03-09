namespace TaskScheduler.Models;

public struct ReminderSettings
{
    public int HoursBeforeDue { get; set; }

    public ReminderSettings(int hoursBeforeDue)
    {
        if (hoursBeforeDue < 0)
            throw new ArgumentOutOfRangeException(nameof(hoursBeforeDue), "Hours must be non-negative.");

        HoursBeforeDue = hoursBeforeDue;
    }

    public readonly DateTime GetReminderTime(DateTime dueDate) =>
        dueDate.AddHours(-HoursBeforeDue);

    public override readonly string ToString() =>
        HoursBeforeDue == 0 ? "No reminder" : $"{HoursBeforeDue}h before due";
}
