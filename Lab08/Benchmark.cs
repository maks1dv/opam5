using System.Diagnostics;

namespace Lab08;

public static class Benchmark
{
    public static void Run(int N, Func<Student, Student, int> cmp)
    {
        Console.WriteLine("=== ЗАВДАННЯ 5: Вимірювання ===");
        int[] sizes = { 1_000, 10_000, 100_000, 1_000_000 };
        Console.WriteLine("| n | Лінійний | Бінарний | Відношення | Теорія: n / 2 і log2 n |");
        Console.WriteLine("| ---: | ---: | ---: | ---: | :--- |");

        foreach (int n in sizes)
        {
            Student[] arr = GenerateSortedStudents(n, N, cmp);
            Random queryRnd = new Random(N + n);
            long totalLin = 0, totalBin = 0;

            for (int q = 0; q < 100; q++)
            {
                int score = (q % 2 == 0) ? arr[queryRnd.Next(n)].Score : queryRnd.Next(0, 110);
                Student target = new Student("Q", score);
                Search.LinearSearch(arr, target, cmp, out int lc);
                Search.BinarySearch(arr, target, cmp, out int bc);
                totalLin += lc;
                totalBin += bc;
            }

            double avgLin = totalLin / 100.0;
            double avgBin = totalBin / 100.0;
            Console.WriteLine($"| {n:N0} | {avgLin:F1} | {avgBin:F1} | {avgLin / avgBin:F1}x | {n / 2:N0} і {Math.Ceiling(Math.Log2(n))} |");
        }

        Console.WriteLine();
        const int size100k = 100_000;
        Student[] baseUnsorted = GenerateUnsortedStudents(size100k, N);
        int[] qValues = { 1, 5, 10, 20, 30, 50, 100 };

        Random qRnd = new Random(N + 777);
        Student[] queries = new Student[100];
        for (int i = 0; i < 100; i++)
        {
            int s = (i % 2 == 0) ? baseUnsorted[qRnd.Next(size100k)].Score : qRnd.Next(0, 110);
            queries[i] = new Student("Q", s);
        }

        Student[] warm = (Student[])baseUnsorted.Clone();
        Array.Sort(warm, (x, y) => cmp(x, y));
        for (int i = 0; i < 20; i++)
        {
            Search.LinearSearch(baseUnsorted, queries[i], cmp, out _);
            Search.BinarySearch(warm, queries[i], cmp, out _);
        }

        Console.WriteLine("| q | Лінійно, мс | Сортування + бінарно, мс | Хто швидший |");
        Console.WriteLine("| ---: | ---: | ---: | :--- |");

        foreach (int q in qValues)
        {
            double bestLin = double.MaxValue, bestBin = double.MaxValue;
            for (int run = 0; run < 3; run++)
            {
                Stopwatch sw1 = Stopwatch.StartNew();
                int d1 = 0;
                for (int i = 0; i < q; i++) d1 ^= Search.LinearSearch(baseUnsorted, queries[i], cmp, out _);
                sw1.Stop();
                if (sw1.Elapsed.TotalMilliseconds < bestLin) bestLin = sw1.Elapsed.TotalMilliseconds;

                Student[] copy = (Student[])baseUnsorted.Clone();
                Stopwatch sw2 = Stopwatch.StartNew();
                Array.Sort(copy, (x, y) => cmp(x, y));
                int d2 = 0;
                for (int i = 0; i < q; i++) d2 ^= Search.BinarySearch(copy, queries[i], cmp, out _);
                sw2.Stop();
                if (sw2.Elapsed.TotalMilliseconds < bestBin) bestBin = sw2.Elapsed.TotalMilliseconds;
            }
            string win = bestBin < bestLin ? "Сортування + бінарно" : "Лінійно";
            Console.WriteLine($"| {q} | {bestLin:F3} | {bestBin:F3} | {win} |");
        }
    }

    public static Student[] GenerateUnsortedStudents(int count, int seed)
    {
        Random rnd = new Random(seed);
        Student[] arr = new Student[count];
        for (int i = 0; i < count; i++) arr[i] = new Student($"Student_{i:D6}", rnd.Next(0, 101));
        return arr;
    }

    public static Student[] GenerateSortedStudents(int count, int seed, Func<Student, Student, int> cmp)
    {
        Student[] arr = GenerateUnsortedStudents(count, seed);
        Array.Sort(arr, (x, y) => cmp(x, y));
        return arr;
    }
}
