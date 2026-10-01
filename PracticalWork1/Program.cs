using System;

class Program
{
    static void Main()
    {
        // Початкові дані (бази студентів від 0 до 100)
        int[] scores = { 85, 92, 58, 74, 95, 60, 42, 88 };

        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\nСтатистика результатів тестування");
            Console.WriteLine("1. Додати результат");
            Console.WriteLine("2. Показати всі результати");
            Console.WriteLine("3. Показати середній бал");
            Console.WriteLine("4. Показати найвищий і найнижчий бал");
            Console.WriteLine("5. Показати розподіл за категоріями");
            Console.WriteLine("0. Вихід");

            int choice = ReadInt("Ваш вибір: ", 0, 5);

            isRunning = choice switch
            {
                1 => HandleAddScore(ref scores),
                2 => HandleShowAll(scores),
                3 => HandleShowAverage(scores),
                4 => HandleShowMinMax(scores),
                5 => HandleShowDistribution(scores),
                0 => false,
                _ => true
            };
        }

        Console.WriteLine("Роботу програми завершено.");
    }

    // Обробники пунктів меню 

    static bool HandleAddScore(ref int[] scores)
    {
        int newScore = ReadInt("Введіть бал студента (0-100): ", 0, 100);
        Array.Resize(ref scores, scores.Length + 1);
        scores[^1] = newScore;
        Console.WriteLine("Результат успішно додано!");
        return true;
    }

    static bool HandleShowAll(int[] scores)
    {
        if (scores.Length == 0)
        {
            Console.WriteLine("Немає даних.");
            return true;
        }

        Console.WriteLine($"\nУсі результати ({scores.Length} шт.):");
        Console.WriteLine(string.Join(", ", scores));
        return true;
    }

    static bool HandleShowAverage(int[] scores)
    {
        if (scores.Length == 0)
        {
            Console.WriteLine("Немає даних.");
            return true;
        }

        double avg = CalculateAverage(scores);
        Console.WriteLine($"Середній бал: {avg:F2}");
        return true;
    }

    static bool HandleShowMinMax(int[] scores)
    {
        if (scores.Length == 0)
        {
            Console.WriteLine("Немає даних.");
            return true;
        }

        FindMinMax(scores, out int maxVal, out int maxIdx, out int minVal, out int minIdx);

        Console.WriteLine($"Найвищий бал: {maxVal} (позиція в масиві: {maxIdx})");
        Console.WriteLine($"Найнижчий бал: {minVal} (позиція в масиві: {minIdx})");
        return true;
    }

    static bool HandleShowDistribution(int[] scores)
    {
        if (scores.Length == 0)
        {
            Console.WriteLine("Немає даних.");
            return true;
        }

        int excellent = 0; // 90+
        int good = 0;      // 75-89
        int satisfactory = 0; // 60-74
        int unsatisfactory = 0; // <60

        foreach (int score in scores)
        {
            string category = GetCategory(score);
            switch (category)
            {
                case "відмінно": excellent++; break;
                case "добре": good++; break;
                case "задовільно": satisfactory++; break;
                case "незадовільно": unsatisfactory++; break;
            }
        }

        Console.WriteLine("\nРозподіл за категоріями:");
        Console.WriteLine($"- Відмінно (90+): {excellent}");
        Console.WriteLine($"- Добре (75-89): {good}");
        Console.WriteLine($"- Задовільно (60-74): {satisfactory}");
        Console.WriteLine($"- Незадовільно (<60): {unsatisfactory}");
        return true;
    }

    // Робочі методи обробки даних 

    static double CalculateAverage(int[] values)
    {
        int sum = 0;
        foreach (int v in values)
        {
            sum += v;
        }
        return (double)sum / values.Length;
    }

    static void FindMinMax(int[] values, out int maxVal, out int maxIdx, out int minVal, out int minIdx)
    {
        maxVal = values[0];
        maxIdx = 0;
        minVal = values[0];
        minIdx = 0;

        for (int i = 1; i < values.Length; i++)
        {
            if (values[i] > maxVal)
            {
                maxVal = values[i];
                maxIdx = i;
            }
            if (values[i] < minVal)
            {
                minVal = values[i];
                minIdx = i;
            }
        }
    }

    static string GetCategory(int score) => score switch
    {
        >= 90 => "відмінно",
        >= 75 => "добре",
        >= 60 => "задовільно",
        _ => "незадовільно"
    };

    static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Некоректне значення. Введіть ціле число від {min} до {max}.");
        }
    }
}