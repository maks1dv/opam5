using System;
using System.Collections.Generic;

namespace Lab05_StackQueue
{
    public static class Task1_BracketBalance
    {
        // a = 2: ( ) [ ] { }, дужки в лапках "..." ігноруються
        public static (bool IsBalanced, int ErrorIndex) CheckBracketsStandard(string input)
        {
            if (string.IsNullOrEmpty(input))
                return (true, -1);

            Stack<char> stack = new Stack<char>();
            bool inQuotes = false;

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (inQuotes)
                    continue;

                if (IsOpening(ch))
                {
                    stack.Push(ch);
                }
                else if (IsClosing(ch))
                {
                    if (stack.Count == 0)
                    {
                        return (false, i);
                    }

                    char open = stack.Pop();
                    if (!IsMatchingPair(open, ch))
                    {
                        return (false, i);
                    }
                }
            }

            if (stack.Count > 0)
            {
                return (false, input.Length);
            }

            return (true, -1);
        }

        public static (bool IsBalanced, int ErrorIndex) CheckBracketsCustom(string input)
        {
            if (string.IsNullOrEmpty(input))
                return (true, -1);

            CustomStack<char> stack = new CustomStack<char>(3);
            bool inQuotes = false;

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (inQuotes)
                    continue;

                if (IsOpening(ch))
                {
                    stack.Push(ch);
                }
                else if (IsClosing(ch))
                {
                    if (stack.Count == 0)
                    {
                        return (false, i);
                    }

                    char open = stack.Pop();
                    if (!IsMatchingPair(open, ch))
                    {
                        return (false, i);
                    }
                }
            }

            if (stack.Count > 0)
            {
                return (false, input.Length);
            }

            return (true, -1);
        }

        private static bool IsOpening(char c) => c == '(' || c == '[' || c == '{';
        private static bool IsClosing(char c) => c == ')' || c == ']' || c == '}';

        private static bool IsMatchingPair(char open, char close)
        {
            return (open == '(' && close == ')') ||
                   (open == '[' && close == ']') ||
                   (open == '{' && close == '}');
        }
    }
}