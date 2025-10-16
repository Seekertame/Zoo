namespace Zoo.Presentation.Console.UI;

public sealed class InputReader
{
    public string ReadRequired(string prompt)
    {
        while (true)
        {
            System.Console.Write($"{prompt}: ");
            var s = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(s))
            {
                return s.Trim();
            }

            System.Console.WriteLine("Поле не может быть пустым.");
        }
    }

    public int ReadInt(string prompt)
    {
        while (true)
        {
            System.Console.Write($"{prompt}: ");
            if (int.TryParse(System.Console.ReadLine(), out var v))
            {
                return v;
            }

            System.Console.WriteLine("Введите целое число.");
        }
    }

    public int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            var v = ReadInt($"{prompt} [{min}..{max}]");
            if (v >= min && v <= max)
            {
                return v;
            }

            System.Console.WriteLine("Число вне диапазона.");
        }
    }

    public int ReadPositive(string prompt)
    {
        while (true)
        {
            var v = ReadInt(prompt);
            if (v > 0)
            {
                return v;
            }

            System.Console.WriteLine("Число должно быть > 0.");
        }
    }

    public int ReadNonNegative(string prompt)
    {
        while (true)
        {
            var v = ReadInt(prompt);
            if (v >= 0)
            {
                return v;
            }

            System.Console.WriteLine("Число должно быть ≥ 0.");
        }
    }
}
