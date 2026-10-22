using System;

namespace Lab05_StackQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 8;
            int K = 17;

            Console.WriteLine("==================================================");
            Console.WriteLine($" Лабораторна робота 5 | Варіант: N={N}, K={K}");
            Console.WriteLine("==================================================");

            // --- Завдання 1 ---
            Console.WriteLine("\n=== Завдання 1: Збалансованість дужок (Стандартний Stack) ===");
            string[] testStrings = new string[]
            {
                "(a[b]{c})",
                "([)]",
                ")(",
                "((",
                "",
                new string('(', 17) + new string(')', 16)
            };

            foreach (var test in testStrings)
            {
                var (isBalanced, errorIdx) = Task1_BracketBalance.CheckBracketsStandard(test);
                Console.WriteLine($"Рядок: \"{test}\" -> Збалансований: {isBalanced}, Індекс помилки: {errorIdx}");
            }

            // --- Завдання 2 ---
            Console.WriteLine("\n=== Завдання 2: Зворотний польський запис ===");
            string[] rpnTests = new string[]
            {
                "3 4 + 2 *",
                "5 3 -",
                "8 2 /",
                "2 3 4 * +",
                "5 0 /",
                "10 5 + 4 2 / +"
            };

            foreach (var expr in rpnTests)
            {
                var (success, val, msg) = Task2_RpnCalculator.Evaluate(expr);
                if (success)
                    Console.WriteLine($"Вираз: \"{expr}\" -> Результат: {val}");
                else
                    Console.WriteLine($"Вираз: \"{expr}\" -> {msg}");
            }

            // --- Завдання 3 ---
            Task3_QueueService.RunSimulation(N, K);

            // --- Завдання 4 ---
            Console.WriteLine("\n=== Завдання 4: Власний стек поверх масиву ===");
            int initialCap = (K % 4) + 2; // 3
            int pushCount = (K % 4) + 3;  // 4
            IntStack myStack = new IntStack(initialCap);

            Console.WriteLine($"Початкова місткість: {myStack.Capacity}");
            for (int i = 1; i <= pushCount; i++)
            {
                myStack.Push(i * 10);
                Console.WriteLine($"Push({i * 10}) -> Count: {myStack.Count}, Capacity: {myStack.Capacity}");
            }

            // Перевірка Завдання 1 на CustomStack
            Console.WriteLine("\n--- Перевірка Завдання 1 на CustomStack ---");
            foreach (var test in testStrings)
            {
                var (isBalanced, errorIdx) = Task1_BracketBalance.CheckBracketsCustom(test);
                Console.WriteLine($"[CustomStack] Рядок: \"{test}\" -> Збалансований: {isBalanced}, Індекс помилки: {errorIdx}");
            }

            Console.WriteLine("\nУсі тести успішно виконано!");
        }
    }
}