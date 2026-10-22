using System;

namespace Lab05_StackQueue
{
    public class IntStack
    {
        private int[] _array;
        private int _count;

        public int Count => _count;
        public int Capacity => _array.Length;

        public IntStack(int initialCapacity)
        {
            if (initialCapacity <= 0) initialCapacity = 3;
            _array = new int[initialCapacity];
            _count = 0;
        }

        public void Push(int x)
        {
            if (_count == _array.Length)
            {
                Array.Resize(ref _array, _array.Length * 2);
            }
            _array[_count++] = x;
        }

        public int Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній! Неможливо виконати Pop().");
            }
            return _array[--_count];
        }

        public int Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній! Неможливо виконати Peek().");
            }
            return _array[_count - 1];
        }
    }

    public class CustomStack<T>
    {
        private T[] _array;
        private int _count;

        public int Count => _count;
        public int Capacity => _array.Length;

        public CustomStack(int initialCapacity)
        {
            if (initialCapacity <= 0) initialCapacity = 3;
            _array = new T[initialCapacity];
            _count = 0;
        }

        public void Push(T item)
        {
            if (_count == _array.Length)
            {
                Array.Resize(ref _array, _array.Length * 2);
            }
            _array[_count++] = item;
        }

        public T Pop()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній!");
            }
            return _array[--_count];
        }

        public T Peek()
        {
            if (_count == 0)
            {
                throw new InvalidOperationException("Стек порожній!");
            }
            return _array[_count - 1];
        }
    }
}