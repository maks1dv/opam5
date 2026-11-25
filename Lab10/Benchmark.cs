using System.Diagnostics;
using System.Text;

namespace Lab10;

public static class Benchmark
{
    public static void RunAll(int N, int K, string proseText)
    {
        RunTask1(proseText);
        RunTask2(proseText);
        RunTask3(K, proseText);
        RunTask4(N, K);
        RunTaskC(proseText);
    }

    private static void RunTask1(string proseText)
    {
        Console.WriteLine("=== ЗАВДАННЯ 1: Три пастки ===");
        StringAlgorithms.ConcatWithPlus(500);
        StringAlgorithms.ConcatWithBuilder(500);

        double tPlus10k = MeasureBestMs(() => StringAlgorithms.ConcatWithPlus(10_000));
        double tSb10k = MeasureBestMs(() => StringAlgorithms.ConcatWithBuilder(10_000));
        double tPlus100k = MeasureBestMs(() => StringAlgorithms.ConcatWithPlus(100_000));
        double tSb100k = MeasureBestMs(() => StringAlgorithms.ConcatWithBuilder(100_000));

        Console.WriteLine($"Пастка 1 (10 000): s += c -> {tPlus10k:F3} мс | StringBuilder -> {tSb10k:F3} мс");
        Console.WriteLine($"Пастка 1 (100 000): s += c -> {tPlus100k:F3} мс | StringBuilder -> {tSb100k:F3} мс");

        List<string> words = StringAlgorithms.NormalizeFast(proseText);
        string[] left = new string[100_000];
        string[] right = new string[100_000];
        for (int i = 0; i < 100_000; i++)
        {
            string w = words[i % words.Count];
            left[i] = w;
            right[i] = (i % 2 == 0) ? w.ToUpperInvariant() : w;
        }

        StringAlgorithms.CountEqualToLower(left, right);
        StringAlgorithms.CountEqualOrdinalIgnoreCase(left, right);

        double tToLower = MeasureBestMs(() => StringAlgorithms.CountEqualToLower(left, right));
        double tOrdinal = MeasureBestMs(() => StringAlgorithms.CountEqualOrdinalIgnoreCase(left, right));
        Console.WriteLine($"Пастка 2 (100 000 пар): ToLower() == ToLower() -> {tToLower:F3} мс | OrdinalIgnoreCase -> {tOrdinal:F3} мс");

        string[] gheWords =
        {
            "ґанок", "ганок", "ґудзик", "гудзик", "ґрунт",
            "грунт", "ґрати", "грати", "ґедзь", "гедзь",
            "ґава", "гава", "ґніт", "гніт", "ґринджоли",
            "гринджоли", "аґрус", "агрус", "дзиґа", "дзига"
        };
        var (ordSorted, culSorted) = StringAlgorithms.CompareSortingWithGhe(gheWords);
        Console.WriteLine("Пастка 3 (сортування 20 слів із 'ґ'):");
        Console.WriteLine($"  Ordinal : {string.Join(", ", ordSorted)}");
        Console.WriteLine($"  uk-UA   : {string.Join(", ", culSorted)}\n");
    }

    private static void RunTask2(string proseText)
    {
        Console.WriteLine("=== ЗАВДАННЯ 2 (a = 2): Нормалізація тексту ===");
        int[] lengths = { 1_000, 10_000, 50_000 };
        string warmSlice = proseText.Substring(0, 500);
        StringAlgorithms.NormalizeNaive(warmSlice);
        StringAlgorithms.NormalizeFast(warmSlice);

        Console.WriteLine("| Символів | Наївно, мс | Швидко, мс | У скільки разів |");
        Console.WriteLine("| ---: | ---: | ---: | ---: |");

        foreach (int len in lengths)
        {
            string slice = proseText.Substring(0, len);
            List<string> r1 = StringAlgorithms.NormalizeNaive(slice);
            List<string> r2 = StringAlgorithms.NormalizeFast(slice);
            if (r1.Count != r2.Count) throw new Exception("Mismatch!");

            double tNaive = MeasureBestMs(() => StringAlgorithms.NormalizeNaive(slice));
            double tFast = MeasureBestMs(() => StringAlgorithms.NormalizeFast(slice));
            Console.WriteLine($"| {len:N0} | {tNaive:F3} | {tFast:F3} | {tNaive / tFast:F1}x |");
        }
        Console.WriteLine();
    }

    private static void RunTask3(int K, string proseText)
    {
        Console.WriteLine("=== ЗАВДАННЯ 3: Пошук підрядка ===");
        string prose50k = proseText.Substring(0, 50_000);
        string patInProse = "світанком";
        string patMissing = "синхрофазотрон";
        string repeats10k = new string('М', 10_000);
        string patRepeats = new string('М', 900 + 10 * K - 1) + "Н";

        StringAlgorithms.NaiveIndexOf(prose50k, patInProse, out _);
        _ = prose50k.IndexOf(patInProse, StringComparison.Ordinal);

        Console.WriteLine("| Вхід | Шаблон | Порівнянь | Час, мс | string.IndexOf, мс |");
        Console.WriteLine("| :--- | :--- | ---: | ---: | ---: |");

        MeasureSearchRow("проза, 50 тис. символів", $"«{patInProse}»", prose50k, patInProse);
        MeasureSearchRow("проза, 50 тис. символів", $"«{patMissing}» (відсутнє)", prose50k, patMissing);
        MeasureSearchRow("повтори, 10 тис. літер 'М'", $"1069×'М' + 'Н' ({patRepeats.Length} симв.)", repeats10k, patRepeats);
        Console.WriteLine();
    }

    private static void MeasureSearchRow(string inputDesc, string patDesc, string text, string pat)
    {
        StringAlgorithms.NaiveIndexOf(text, pat, out long comps);
        double tNaive = MeasureBestMs(() => StringAlgorithms.NaiveIndexOf(text, pat, out _));
        double tStd = MeasureBestMs(() => text.IndexOf(pat, StringComparison.Ordinal));
        Console.WriteLine($"| {inputDesc} | {patDesc} | {comps:N0} | {tNaive:F3} | {tStd:F4} |");
    }

    private static void RunTask4(int N, int K)
    {
        Console.WriteLine("=== ЗАВДАННЯ 4 (b = 0): Решта монетами 1, 4, 5 ===");
        int[] coinsDesc = { (K % 4) + 4, (K % 4) + 3, 1 };
        int maxSum = 20 + K;
        Random rnd = new Random(N);
        int optimalCount = 0;
        double worstLagPercent = 0.0;
        string smallestCounterExample = "немає";
        int smallestAmount = int.MaxValue;

        for (int t = 0; t < 100; t++)
        {
            int amount = rnd.Next(1, maxSum + 1);
            int greedy = GreedyAndBruteForce.ChangeGreedy(amount, coinsDesc);
            int exact = GreedyAndBruteForce.ChangeBruteForce(amount, coinsDesc);
            if (greedy == exact) optimalCount++;
            else
            {
                double lag = ((double)(greedy - exact) / exact) * 100.0;
                if (lag > worstLagPercent) worstLagPercent = lag;
                if (amount < smallestAmount)
                {
                    smallestAmount = amount;
                    smallestCounterExample = $"сума = {amount} (жадібно: {greedy} монет [5+1+1+1], оптимум: {exact} монети [4+4])";
                }
            }
        }

        GreedyAndBruteForce.ChangeBruteForce(10, coinsDesc);
        double tBrute15 = MeasureBestMs(() => GreedyAndBruteForce.ChangeBruteForce(15, coinsDesc));
        double tBrute20 = MeasureBestMs(() => GreedyAndBruteForce.ChangeBruteForce(20, coinsDesc));

        Console.WriteLine("| Показник | Значення |");
        Console.WriteLine("| :--- | :--- |");
        Console.WriteLine($"| Тестів, де жадібне рішення дало оптимум | {optimalCount} зі 100 |");
        Console.WriteLine($"| Найгірше відставання, у відсотках | {worstLagPercent:F1}% |");
        Console.WriteLine($"| Найменший контрприклад, який ви знайшли | {smallestCounterExample} |");
        Console.WriteLine($"| Час перебору на 15 і на 20 елементах | 15: {tBrute15:F4} мс; 20: {tBrute20:F4} мс |\n");
    }

    private static void RunTaskC(string proseText)
    {
        Console.WriteLine("=== ДОДАТКОВО (c = 3): Split усього файлу проти StreamReader (100 000 рядків) ===");
        string bigFilePath = Path.Combine("Lab10", "temp_100k.txt");
        using (StreamWriter sw = new StreamWriter(bigFilePath, false, Encoding.UTF8))
        {
            string sampleLine = proseText.Substring(0, 80).Replace('\r', ' ').Replace('\n', ' ');
            for (int i = 0; i < 100_000; i++) sw.WriteLine($"{i}: {sampleLine}");
        }

        _ = File.ReadAllText(bigFilePath).Split('\n').Length;
        using (StreamReader sr = new StreamReader(bigFilePath)) { while (sr.ReadLine() != null) { } }

        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        long memBeforeSplit = GC.GetTotalMemory(true);
        int linesSplit = 0;
        double tSplit = MeasureBestMs(() =>
        {
            string all = File.ReadAllText(bigFilePath, Encoding.UTF8);
            linesSplit = all.Split('\n').Length;
        });
        long memAfterSplit = GC.GetTotalMemory(false);
        double deltaSplitMb = Math.Max(0, (memAfterSplit - memBeforeSplit) / (1024.0 * 1024.0));

        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        long memBeforeReader = GC.GetTotalMemory(true);
        int linesReader = 0;
        double tReader = MeasureBestMs(() =>
        {
            int count = 0;
            using StreamReader sr = new StreamReader(bigFilePath, Encoding.UTF8);
            while (sr.ReadLine() != null) count++;
            linesReader = count;
        });
        long memAfterReader = GC.GetTotalMemory(false);
        double deltaReaderMb = Math.Max(0, (memAfterReader - memBeforeReader) / (1024.0 * 1024.0));

        File.Delete(bigFilePath);
        Console.WriteLine($"- ReadAllText + Split : час = {tSplit:F3} мс, приріст керованої пам'яті = {deltaSplitMb:F2} МБ (рядків: {linesSplit})");
        Console.WriteLine($"- StreamReader        : час = {tReader:F3} мс, приріст керованої пам'яті = {deltaReaderMb:F2} МБ (рядків: {linesReader})\n");
    }

    private static double MeasureBestMs(Action action)
    {
        double best = double.MaxValue;
        for (int r = 0; r < 3; r++)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            if (sw.Elapsed.TotalMilliseconds < best) best = sw.Elapsed.TotalMilliseconds;
        }
        return best;
    }
}
