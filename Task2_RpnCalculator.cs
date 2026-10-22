using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lab05_StackQueue
{
    public static class Task2_RpnCalculator
    {
        // b = 0: +, -, *, /
        public static (bool Success, double Value, string ErrorMessage) Evaluate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return (false, 0, "Помилка: порожній вираз.");
            }

            string[] tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Stack<double> stack = new Stack<double>();

            foreach (string token in tokens)
            {
                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    stack.Push(number);
                }
                else if (token == "+" || token == "-" || token == "*" || token == "/")
                {
                    if (stack.Count < 2)
                    {
                        return (false, 0, $"Помилка: забракло чисел для операції '{token}'.");
                    }

                    double b = stack.Pop();
                    double a = stack.Pop();

                    if (token == "+") stack.Push(a + b);
                    else if (token == "-") stack.Push(a - b);
                    else if (token == "*") stack.Push(a * b);
                    else if (token == "/")
                    {
                        if (Math.Abs(b) < 1e-12)
                        {
                            return (false, 0, "Помилка: ділення на нуль.");
                        }
                        stack.Push(a / b);
                    }
                }
                else
                {
                    return (false, 0, $"Помилка: невідомий токен '{token}'.");
                }
            }

            if (stack.Count == 0)
            {
                return (false, 0, "Помилка: порожній вираз.");
            }

            if (stack.Count > 1)
            {
                return (false, 0, "Помилка: забагато чисел у виразі.");
            }

            return (true, stack.Pop(), "Успішно");
        }
    }
}