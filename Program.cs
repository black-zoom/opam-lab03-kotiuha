using System;
using System.IO;

class Program
{
    static FileInfo? FindLargestFile(string path)
    {
        FileInfo? largest = null;
        string[] files;
        string[] directories;

        try
        {
            files = Directory.GetFiles(path);
            directories = Directory.GetDirectories(path);
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
        catch (IOException)
        {
            return null;
        }

        foreach (string file in files)
        {
            try
            {
                FileInfo info = new FileInfo(file);

                if (largest == null || info.Length > largest.Length)
                    largest = info;
            }
            catch (UnauthorizedAccessException)
            {
                continue;
            }
            catch (IOException)
            {
                continue;
            }
            catch (System.Security.SecurityException)
            {
                continue;
            }
        }

        foreach (string directory in directories)
        {
            FileInfo? candidate = FindLargestFile(directory);

            if (candidate != null &&
                (largest == null || candidate.Length > largest.Length))
            {
                largest = candidate;
            }
        }

        return largest;
    }

    static bool Solve(int[] a, int lo, int hi, ref int maxDepth,
                      int depth = 0)
    {
        if (depth > maxDepth)
            maxDepth = depth;

        if (lo == hi)
            return true;

        int mid = lo + (hi - lo) / 2;

        bool left = Solve(a, lo, mid, ref maxDepth, depth + 1);
        bool right = Solve(a, mid + 1, hi, ref maxDepth, depth + 1);

        return left && right && a[mid] == a[mid + 1];
    }

    static int FindChange(int remaining, int coinIndex,
                          int[] coins, int[] counts)
    {
        if (remaining < 0)
            return 0;

        if (coinIndex == coins.Length)
        {
            if (remaining != 0)
                return 0;

            Console.WriteLine(
                $"1 грн: {counts[0]}, 2 грн: {counts[1]}, 5 грн: {counts[2]}");

            return 1;
        }

        int total = 0;

        for (int count = 0; count <= remaining / coins[coinIndex]; count++)
        {
            counts[coinIndex] = count;

            total += FindChange(
                remaining - count * coins[coinIndex],
                coinIndex + 1,
                coins,
                counts);
        }

        counts[coinIndex] = 0;
        return total;
    }

    static bool TraceSolve(int[] a, int lo, int hi, int depth)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}enter Solve({lo}..{hi})");

        if (lo == hi)
        {
            Console.WriteLine($"{indent}  -> true (один елемент)");
            Console.WriteLine($"{indent}exit  Solve({lo}..{hi}) -> true");
            return true;
        }

        int mid = lo + (hi - lo) / 2;

        bool left = TraceSolve(a, lo, mid, depth + 1);
        bool right = TraceSolve(a, mid + 1, hi, depth + 1);

        bool result = left && right && a[mid] == a[mid + 1];

        Console.WriteLine($"{indent}exit  Solve({lo}..{hi}) -> {result}");
        return result;
    }

    static bool SolveIterative(int[] a)
    {
        if (a.Length == 0)
            return true;

        for (int i = 1; i < a.Length; i++)
        {
            if (a[i] != a[0])
                return false;
        }

        return true;
    }

    static void Task1()
    {
        Console.Write("Шлях до папки: ");
        string? path = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            Console.WriteLine("Папку не знайдено.");
            return;
        }

        FileInfo? result = FindLargestFile(path);

        if (result == null)
        {
            Console.WriteLine("Файлів не знайдено або немає доступу.");
            return;
        }

        Console.WriteLine(
            $"Найбільший файл: {result.Name}, {result.Length} байт, {result.FullName}");
    }

    static void Task2()
    {
        int[] numbers = new int[16];
        Random random = new Random(14);

        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = random.Next(-20, 21);

        int maxDepth = 0;
        bool result = Solve(numbers, 0, numbers.Length - 1, ref maxDepth);

        Console.WriteLine($"Масив: {string.Join(", ", numbers)}");
        Console.WriteLine($"Усі елементи однакові: {result}");
        Console.WriteLine($"Глибина рекурсії: {maxDepth}");
        Console.WriteLine("log2(16) = 4");
    }

    static void Task3()
    {
        int[] coins = { 1, 2, 5 };
        int[] counts = new int[coins.Length];

        int total = FindChange(9, 0, coins, counts);

        Console.WriteLine($"Кількість способів: {total}");
    }

    static void Task4()
    {
        int[] sample = { 7, -2, 15, 4 };

        Console.WriteLine("Трасування:");
        TraceSolve(sample, 0, sample.Length - 1, 0);
    }

    static void Task5()
    {
        int[] numbers = new int[16];
        Random random = new Random(14);

        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = random.Next(-20, 21);

        int maxDepth = 0;
        bool recursive = Solve(numbers, 0, numbers.Length - 1, ref maxDepth);
        bool iterative = SolveIterative(numbers);

        Console.WriteLine($"Рекурсивний результат: {recursive}");
        Console.WriteLine($"Циклічний результат: {iterative}");
        Console.WriteLine($"Результати збігаються: {recursive == iterative}");
    }

    static void Main()
    {
        Task1();
        Console.WriteLine();

        Task2();
        Console.WriteLine();

        Task3();
        Console.WriteLine();

        Task4();
        Console.WriteLine();

        Task5();
    }
}
