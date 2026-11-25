using System.Diagnostics;

namespace Lab09;

public static class Benchmark
{
    public delegate void SortDelegate<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves);

    public static int[] GenerateRandomArray(int size, int seed, int maxVal)
    {
        Random rnd = new Random(seed);
        int[] arr = new int[size];
        for (int i = 0; i < size; i++)
            arr[i] = rnd.Next(0, maxVal + 1);
        return arr;
    }

    public static int[][] CreateFourInputs(int size, int seed, int maxVal, int kSwaps)
    {
        int[] random = GenerateRandomArray(size, seed, maxVal);

        int[] sorted = (int[])random.Clone();
        Array.Sort(sorted);

        int[] reversed = (int[])sorted.Clone();
        Array.Reverse(reversed);

        int[] almost = (int[])sorted.Clone();
        Random swapRnd = new Random(seed + 100);
        if (size > 1)
        {
            for (int s = 0; s < kSwaps; s++)
            {
                int i = swapRnd.Next(0, size);
                int j = swapRnd.Next(0, size);
                (almost[i], almost[j]) = (almost[j], almost[i]);
            }
        }

        return new[] { random, sorted, reversed, almost };
    }

    public static void RunTask4(int seed, int maxVal, int kSwaps)
    {
        Console.WriteLine("=== ЗАВДАННЯ 4: Порівняння / Переставляння ===");
        Func<int, int, int> cmpInt = (x, y) => x.CompareTo(y);
        int[] sizes = { 100, 1_000, 10_000 };

        (string Name, SortDelegate<int> Method)[] algos =
        {
            ("1. Бульбашка з прапорцем (a = 2)", Sorts.BubbleSort),
            ("2. Злиттям рекурсивне (b = 0)", Sorts.MergeSort),
            ("3. Сортування Шелла (c = 3)", Sorts.ShellSort)
        };

        foreach (var algo in algos)
        {
            Console.WriteLine($"\n#### {algo.Name}");
            Console.WriteLine("| n | Випадковий | Відсортований | Навпаки | Майже |");
            Console.WriteLine("| ---: | :---: | :---: | :---: | :---: |");

            foreach (int n in sizes)
            {
                int[][] inputs = CreateFourInputs(n, seed, maxVal, kSwaps);
                string[] cells = new string[4];

                for (int t = 0; t < 4; t++)
                {
                    int[] original = inputs[t];
                    int[] work = (int[])original.Clone();
                    algo.Method(work, cmpInt, out long comps, out long moves);

                    if (!Validation.IsSorted(work, cmpInt) || !Validation.IsPermutationOf(work, original, cmpInt))
                        throw new Exception($"Помилка перевірки для {algo.Name} на n={n}, тип={t}");

                    cells[t] = $"{comps} / {moves}";
                }

                Console.WriteLine($"| {n:N0} | {cells[0]} | {cells[1]} | {cells[2]} | {cells[3]} |");
            }
        }
        Console.WriteLine();
    }

    public static void RunTask5(int seed, int maxVal)
    {
        Console.WriteLine("=== ЗАВДАННЯ 5: Час проти Array.Sort (мс) ===");
        Func<int, int, int> cmpInt = (x, y) => x.CompareTo(y);
        int[] sizes = { 1_000, 10_000, 100_000, 1_000_000 };

        // Прогрівочний запуск
        int[] warm = GenerateRandomArray(2_000, seed, maxVal);
        Sorts.BubbleSort((int[])warm.Clone(), cmpInt, out _, out _);
        Sorts.MergeSort((int[])warm.Clone(), cmpInt, out _, out _);
        Sorts.ShellSort((int[])warm.Clone(), cmpInt, out _, out _);
        Array.Sort((int[])warm.Clone());

        Console.WriteLine("| n | Простий (Бульбашка) | Швидкий (Злиттям) | Додатковий (Шелла) | Array.Sort |");
        Console.WriteLine("| ---: | ---: | ---: | ---: | ---: |");

        foreach (int n in sizes)
        {
            int[] baseArr = GenerateRandomArray(n, seed, maxVal);

            string tBubble = (n <= 100_000) ? $"{MeasureBestMs(() => Sorts.BubbleSort((int[])baseArr.Clone(), cmpInt, out _, out _)):F3}" : "—";
            string tMerge = $"{MeasureBestMs(() => Sorts.MergeSort((int[])baseArr.Clone(), cmpInt, out _, out _)):F3}";
            string tShell = $"{MeasureBestMs(() => Sorts.ShellSort((int[])baseArr.Clone(), cmpInt, out _, out _)):F3}";
            string tStd = $"{MeasureBestMs(() => Array.Sort((int[])baseArr.Clone())):F3}";

            Console.WriteLine($"| {n:N0} | {tBubble} | {tMerge} | {tShell} | {tStd} |");
        }

        // Пошук межі прийнятності простого сортування (> 1000 мс)
        Console.WriteLine("\nПошук межі прийнятності бульбашкового сортування (> 1 с):");
        int testSize = 10_000;
        while (true)
        {
            int[] arr = GenerateRandomArray(testSize, seed, maxVal);
            double ms = MeasureBestMs(() => Sorts.BubbleSort((int[])arr.Clone(), cmpInt, out _, out _));
            Console.WriteLine($"   n = {testSize:N0} -> {ms:F1} мс");
            if (ms > 1000.0)
            {
                Console.WriteLine($"   -> Межа прийнятності (вперше > 1 с): n = {testSize:N0}");
                break;
            }
            testSize += 5_000;
        }
    }

    private static double MeasureBestMs(Action action)
    {
        double best = double.MaxValue;
        for (int r = 0; r < 3; r++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            if (sw.Elapsed.TotalMilliseconds < best)
                best = sw.Elapsed.TotalMilliseconds;
        }
        return best;
    }

    public static void RunStabilityCheck()
    {
        Console.WriteLine("\n=== ЗАВДАННЯ 5: Перевірка стабільності ===");
        Student[] baseStudents =
        {
            new Student("Бондаренко", "КН-3/1", 85),
            new Student("Коваленко", "КН-3/2", 90),
            new Student("Кравченко", "КН-3/1", 85),
            new Student("Мельник", "КН-3/3", 75),
            new Student("Мороз", "КН-3/2", 85),
            new Student("Олійник", "КН-3/1", 90),
            new Student("Ткаченко", "КН-3/3", 75),
            new Student("Шевчук", "КН-3/2", 85)
        };

        Func<Student, Student, int> cmpScore = (s1, s2) => s1.Score.CompareTo(s2.Score);

        CheckAlgoStability("Бульбашка з прапорцем", baseStudents, arr => Sorts.BubbleSort(arr, cmpScore, out _, out _));
        CheckAlgoStability("Сортування злиттям", baseStudents, arr => Sorts.MergeSort(arr, cmpScore, out _, out _));
        CheckAlgoStability("Сортування Шелла", baseStudents, arr => Sorts.ShellSort(arr, cmpScore, out _, out _));
        CheckAlgoStability("Array.Sort", baseStudents, arr => Array.Sort(arr, (a, b) => cmpScore(a, b)));
    }

    private static void CheckAlgoStability(string name, Student[] initial, Action<Student[]> sortAction)
    {
        Student[] copy = (Student[])initial.Clone();
        sortAction(copy);

        string counterExample = "немає (порядок збережено)";
        for (int i = 1; i < copy.Length; i++)
        {
            if (copy[i - 1].Score == copy[i].Score &&
                string.Compare(copy[i - 1].Name, copy[i].Name, StringComparison.Ordinal) > 0)
            {
                counterExample = $"Score={copy[i].Score}: '{copy[i - 1].Name}' перед '{copy[i].Name}'";
                break;
            }
        }

        Console.WriteLine($"- {name}: {(counterExample.StartsWith("немає") ? "Стабільний" : "Нестабільний")} | Контрприклад: {counterExample}");
    }
}
