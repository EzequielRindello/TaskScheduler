namespace TaskScheduler.UI;

public static class InputHelper
{
    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(value))
                return value;

            Console.WriteLine("  This field is required.");
        }
    }

    public static string ReadOptionalString(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public static DateTime ReadFutureDateTime(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

            if (DateTime.TryParse(input, out var dt))
            {
                if (dt > DateTime.Now)
                    return dt;

                Console.WriteLine("  Due date must be in the future.");
            }
            else
            {
                Console.WriteLine("  Invalid date. Use format: yyyy-MM-dd HH:mm");
            }
        }
    }

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"  Please enter a number between {min} and {max}.");
        }
    }

    public static T ReadEnum<T>(string prompt) where T : struct, Enum
    {
        var names = Enum.GetNames<T>();
        var values = Enum.GetValues<T>();

        Console.WriteLine(prompt);
        for (int i = 0; i < names.Length; i++)
            Console.WriteLine($"  {i + 1}. {names[i]}");

        var choice = ReadInt("  Choice: ", 1, names.Length);
        return values[choice - 1];
    }
}
