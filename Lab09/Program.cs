using System.Text;

namespace Lab09;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        const int N = 8;
        const int K = 17;
        const int MaxVal = 20 * K; // 340
        const int TraceLen = (K % 5) + 6; // 8

        Func<int, int, int> cmpInt = (x, y) => x.CompareTo(y);

        // --- ЗАВДАННЯ 1: Трасування бульбашкового сортування на перших 8 елементах ---
        int[] fullRandom = Benchmark.GenerateRandomArray(100, N, MaxVal);
        int[] traceArr = new int[TraceLen];
        Array.Copy(fullRandom, traceArr, TraceLen);

        Console.WriteLine("=== ЗАВДАННЯ 1: Трасування бульбашки з прапорцем ===");
        Console.WriteLine($"Початковий масив ({TraceLen} елементів): [{string.Join(", ", traceArr)}]");

        int[] stepArr = (int[])traceArr.Clone();
        int totalComps = 0, totalMoves = 0;
        for (int i = 0; i < stepArr.Length - 1; i++)
        {
            bool swapped = false;
            int passComps = 0, passSwaps = 0;
            for (int j = 0; j < stepArr.Length - 1 - i; j++)
            {
                passComps++;
                totalComps++;
                if (stepArr[j] > stepArr[j + 1])
                {
                    (stepArr[j], stepArr[j + 1]) = (stepArr[j + 1], stepArr[j]);
                    passSwaps++;
                    totalMoves += 2;
                    swapped = true;
                }
            }
            Console.WriteLine($"Прохід {i + 1}: [{string.Join(", ", stepArr)}] | порівнянь: {passComps}, обмінів: {passSwaps}");
            if (!swapped)
            {
                Console.WriteLine("   -> Обмінів не було, дострокова зупинка за прапорцем!");
                break;
            }
        }
        Console.WriteLine($"Разом: порівнянь = {totalComps}, записів (moves) = {totalMoves}\n");

        // --- КРАЙОВІ ВИПАДКИ ---
        Console.WriteLine("=== КРАЙОВІ ВИПАДКИ ===");
        (string Label, int[] Data)[] edgeCases =
        {
            ("Порожній масив", Array.Empty<int>()),
            ("Один елемент", new[] { 42 }),
            ("Два елементи в порядку", new[] { 10, 20 }),
            ("Два елементи навпаки", new[] { 20, 10 }),
            ("Усі елементи однакові", new[] { 7, 7, 7, 7, 7, 7 }),
            ("Два різних значення по пів масиву", new[] { 5, 1, 5, 1, 5, 1 })
        };

        foreach (var ec in edgeCases)
        {
            int[] a1 = (int[])ec.Data.Clone();
            int[] a2 = (int[])ec.Data.Clone();
            int[] a3 = (int[])ec.Data.Clone();

            Sorts.BubbleSort(a1, cmpInt, out _, out _);
            Sorts.MergeSort(a2, cmpInt, out _, out _);
            Sorts.ShellSort(a3, cmpInt, out _, out _);

            bool ok = Validation.IsSorted(a1, cmpInt) && Validation.IsPermutationOf(a1, ec.Data, cmpInt) &&
                      Validation.IsSorted(a2, cmpInt) && Validation.IsPermutationOf(a2, ec.Data, cmpInt) &&
                      Validation.IsSorted(a3, cmpInt) && Validation.IsPermutationOf(a3, ec.Data, cmpInt);

            Console.WriteLine($"- {ec.Label}: [{string.Join(", ", a2)}] -> OK={ok}");
        }
        Console.WriteLine();

        // --- ЗАВДАННЯ 4 і 5 ---
        Benchmark.RunTask4(N, MaxVal, K);
        Benchmark.RunTask5(N, MaxVal);
        Benchmark.RunStabilityCheck();
    }
}
