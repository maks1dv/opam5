namespace Lab09;

public static class Sorts
{
    // Завдання 1 (a = 2): Сортування бульбашкою з прапорцем
    public static void BubbleSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;

        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - 1 - i; j++)
            {
                comparisons++;
                if (cmp(a[j], a[j + 1]) > 0)
                {
                    T temp = a[j];
                    a[j] = a[j + 1];
                    a[j + 1] = temp;
                    moves += 2; // 2 записи у масив при обміні
                    swapped = true;
                }
            }
            if (!swapped)
                break;
        }
    }

    // Завдання 2 (b = 0): Рекурсивне сортування злиттям (один буфер зовні)
    public static void MergeSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
    {
        comparisons = 0;
        moves = 0;
        if (a.Length <= 1)
            return;

        T[] buffer = new T[a.Length];
        MergeSortRec(a, buffer, 0, a.Length - 1, cmp, ref comparisons, ref moves);
    }

    private static void MergeSortRec<T>(
        T[] a, T[] buffer, int left, int right, Func<T, T, int> cmp, ref long comparisons, ref long moves)
    {
        if (left >= right)
            return;

        int mid = left + (right - left) / 2;
        MergeSortRec(a, buffer, left, mid, cmp, ref comparisons, ref moves);
        MergeSortRec(a, buffer, mid + 1, right, cmp, ref comparisons, ref moves);
        Merge(a, buffer, left, mid, right, cmp, ref comparisons, ref moves);
    }

    private static void Merge<T>(
        T[] a, T[] buffer, int left, int mid, int right, Func<T, T, int> cmp, ref long comparisons, ref long moves)
    {
        int i = left;
        int j = mid + 1;
        int k = left;

        while (i <= mid && j <= right)
        {
            comparisons++;
            // <= гарантує стабільність сортування
            if (cmp(a[i], a[j]) <= 0)
            {
                buffer[k++] = a[i++];
            }
            else
            {
                buffer[k++] = a[j++];
            }
            moves++;
        }

        while (i <= mid)
        {
            buffer[k++] = a[i++];
            moves++;
        }

        while (j <= right)
        {
            buffer[k++] = a[j++];
            moves++;
        }

        for (int idx = left; idx <= right; idx++)
        {
            a[idx] = buffer[idx];
            moves++;
        }
    }

    // Завдання 3 (c = 3): Сортування Шелла (послідовність Кнута або ділення на 2)
    public static void ShellSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;

        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i++)
            {
                T temp = a[i];
                int j = i;
                bool shifted = false;

                while (j >= gap)
                {
                    comparisons++;
                    if (cmp(a[j - gap], temp) > 0)
                    {
                        a[j] = a[j - gap];
                        moves++;
                        j -= gap;
                        shifted = true;
                    }
                    else
                    {
                        break;
                    }
                }

                if (shifted)
                {
                    a[j] = temp;
                    moves++;
                }
            }
        }
    }
}
