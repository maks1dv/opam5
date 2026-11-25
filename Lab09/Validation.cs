namespace Lab09;

public static class Validation
{
    public static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
    {
        for (int i = 1; i < a.Length; i++)
        {
            if (cmp(a[i - 1], a[i]) > 0)
                return false;
        }
        return true;
    }

    public static bool IsPermutationOf<T>(T[] actual, T[] expected, Func<T, T, int> cmp)
    {
        if (actual.Length != expected.Length)
            return false;

        T[] copyActual = (T[])actual.Clone();
        T[] copyExpected = (T[])expected.Clone();

        Array.Sort(copyActual, (x, y) => cmp(x, y));
        Array.Sort(copyExpected, (x, y) => cmp(x, y));

        for (int i = 0; i < copyActual.Length; i++)
        {
            if (cmp(copyActual[i], copyExpected[i]) != 0)
                return false;
        }
        return true;
    }
}
