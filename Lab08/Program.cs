using System.Text;

namespace Lab08;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        const int N = 8, K = 17, X = K + 50, MissingScore = 101;
        Func<Student, Student, int> cmpByScore = (s1, s2) => s1.Score.CompareTo(s2.Score);

        Student[] unsorted = Benchmark.GenerateUnsortedStudents(100_000, N);
        Student[] students = (Student[])unsorted.Clone();
        Array.Sort(students, (x, y) => cmpByScore(x, y));

        Student tX = new Student("TargetX", X);
        Student tFirst = students[0];
        Student tMiss = new Student("TargetMissing", MissingScore);

        Console.WriteLine("=== ЗАВДАННЯ 1: Лінійний пошук ===");
        Console.WriteLine($"1) X={X}: індекс={Search.LinearSearch(students, tX, cmpByScore, out int cL1)}, порівнянь={cL1}");
        Console.WriteLine($"2) Перший (Score={tFirst.Score}): індекс={Search.LinearSearch(students, tFirst, cmpByScore, out int cL2)}, порівнянь={cL2}");
        Console.WriteLine($"3) Відсутнє ({MissingScore}): індекс={Search.LinearSearch(students, tMiss, cmpByScore, out int cL3)}, порівнянь={cL3}\n");

        Console.WriteLine("=== ЗАВДАННЯ 2: Бінарний пошук та поломки ===");
        int iB1 = Search.BinarySearch(students, tX, cmpByScore, out int cB1);
        int iB2 = Search.BinarySearch(students, tFirst, cmpByScore, out int cB2);
        int iB3 = Search.BinarySearch(students, tMiss, cmpByScore, out int cB3);
        Console.WriteLine($"Правильний: 1) індекс={iB1}, порівнянь={cB1} | 2) індекс={iB2}, порівнянь={cB2} | 3) індекс={iB3}, порівнянь={cB3}");

        Student[] small = { new Student("A", 10), new Student("B", 20), new Student("C", 30) };
        Console.WriteLine($"Поломка 1 (lo < hi): {Broken.BinarySearch_LessInsteadOfLessOrEqual(small, new Student("C", 30), cmpByScore, out int b1c)} (порівнянь: {b1c})");
        Broken.BinarySearch_InfiniteLoop(small, new Student("C", 30), cmpByScore, 15, out _, out string tr);
        Console.WriteLine($"Поломка 2 (lo = mid): {tr}");
        int b3 = Broken.BinarySearch_OverflowMid(students, tX, cmpByScore, out int b3c);
        Console.WriteLine($"Поломка 3 ((lo+hi)/2): на 100k працює (індекс={b3}, порівнянь={b3c}), поріг={((long)int.MaxValue + 2) / 2:N0}");

        Random rnd = new Random(N);
        int notFound = 0;
        for (int i = 0; i < 100; i++)
            if (Search.BinarySearch(unsorted, unsorted[rnd.Next(unsorted.Length)], cmpByScore, out _) == -1) notFound++;
        Console.WriteLine($"Поломка 4 (невідсортований): не знайдено зі 100 = {notFound}\n");
        Console.WriteLine("=== ЗАВДАННЯ 3: Пошук межі (b = 0) ===");
        int lb = Search.FindBoundary(students, s => s.Score >= X, out int cLb);
        int ub = Search.FindBoundary(students, s => s.Score > X, out int cUb);
        Console.WriteLine($"Перше входження X={X}: індекс={lb} (порівнянь={cLb}), кількість рівних X={ub - lb} (сумарно порівнянь={cLb + cUb} проти лінійних 100000)");

        int wrongIdx = Search.FindBoundary(students, s => s.Score % 7 == 0, out _);
        int realDiv7 = -1;
        for (int i = 0; i < students.Length; i++) if (students[i].Score % 7 == 0) { realDiv7 = i; break; }
        Console.WriteLine($"Немонотонна умова (% 7 == 0): повернуло індекс={wrongIdx} (Score={(wrongIdx < students.Length ? students[wrongIdx].Score : -1)}), реальний перший={realDiv7} (Score={students[realDiv7].Score})\n");

        Console.WriteLine("=== ЗАВДАННЯ 4: Експоненційний пошук (c = 3) ===");
        Console.WriteLine($"1) X={X}: індекс={Search.ExponentialSearch(students, tX, cmpByScore, out int cE1)}, порівнянь={cE1} (бінарний={cB1})");
        Console.WriteLine($"2) Перший: індекс={Search.ExponentialSearch(students, tFirst, cmpByScore, out int cE2)}, порівнянь={cE2} (бінарний={cB2})");
        Console.WriteLine($"3) Відсутнє: індекс={Search.ExponentialSearch(students, tMiss, cmpByScore, out int cE3)}, порівнянь={cE3} (бінарний={cB3})\n");

        Console.WriteLine("=== КРАЙОВІ ВИПАДКИ ===");
        Console.WriteLine($"1) Порожній масив: {Search.BinarySearch(Array.Empty<Student>(), tX, cmpByScore, out _)}");
        Console.WriteLine($"2) Один елемент (шуканий): {Search.BinarySearch(new[] { new Student("S", 67) }, tX, cmpByScore, out _)}");
        Console.WriteLine($"3) Два елементи (другий): {Search.BinarySearch(new[] { new Student("A", 50), new Student("B", 67) }, tX, cmpByScore, out _)}");
        Console.WriteLine($"4) Менше за всі (-5): {Search.FindBoundary(students, s => s.Score >= -5, out _)}");
        Console.WriteLine($"5) Більше за всі (101): {Search.FindBoundary(students, s => s.Score >= 101, out _)}");
        Student[] eq = { new Student("1", 67), new Student("2", 67), new Student("3", 67) };
        Console.WriteLine($"6) Усі однакові: перша={Search.FindBoundary(eq, s => s.Score >= 67, out _)}, остання={Search.FindBoundary(eq, s => s.Score > 67, out _) - 1}\n");

        Benchmark.Run(N, cmpByScore);
    }
}

