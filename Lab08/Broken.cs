namespace Lab08;

public static class Broken
{
    public static int BinarySearch_LessInsteadOfLessOrEqual<T>(
        T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0, hi = a.Length - 1;
        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int c = cmp(a[mid], target);
            if (c == 0) return mid;
            if (c < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }

    public static int BinarySearch_InfiniteLoop<T>(
        T[] a, T target, Func<T, T, int> cmp, int maxIterations, out int iterations, out string trace)
    {
        iterations = 0;
        trace = "";
        int lo = 0, hi = a.Length - 1;
        while (lo <= hi)
        {
            iterations++;
            int mid = lo + (hi - lo) / 2;
            if (iterations <= 5) trace += $"[крок {iterations}: lo={lo}, hi={hi}, mid={mid}] ";
            if (iterations >= maxIterations)
            {
                trace += "... ЗУПИНЕНО (нескінченний цикл)";
                return -2;
            }
            int c = cmp(a[mid], target);
            if (c == 0) return mid;
            if (c < 0) lo = mid;
            else hi = mid - 1;
        }
        return -1;
    }

    public static int BinarySearch_OverflowMid<T>(
        T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0, hi = a.Length - 1;
        while (lo <= hi)
        {
            int mid = (lo + hi) / 2;
            comparisons++;
            int c = cmp(a[mid], target);
            if (c == 0) return mid;
            if (c < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }
}
