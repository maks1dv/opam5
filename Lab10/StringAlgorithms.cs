using System.Globalization;
using System.Text;

namespace Lab10;

public static class StringAlgorithms
{
    public static string ConcatWithPlus(int count)
    {
        string s = string.Empty;
        for (int i = 0; i < count; i++)
            s += (char)('А' + (i % 32));
        return s;
    }

    public static string ConcatWithBuilder(int count)
    {
        StringBuilder sb = new StringBuilder(count);
        for (int i = 0; i < count; i++)
            sb.Append((char)('А' + (i % 32)));
        return sb.ToString();
    }

    public static int CountEqualToLower(string[] left, string[] right)
    {
        int matches = 0;
        for (int i = 0; i < left.Length; i++)
            if (left[i].ToLower() == right[i].ToLower()) matches++;
        return matches;
    }

    public static int CountEqualOrdinalIgnoreCase(string[] left, string[] right)
    {
        int matches = 0;
        for (int i = 0; i < left.Length; i++)
            if (string.Equals(left[i], right[i], StringComparison.OrdinalIgnoreCase)) matches++;
        return matches;
    }

    public static (string[] OrdinalSorted, string[] CulturalSorted) CompareSortingWithGhe(string[] words)
    {
        string[] ord = (string[])words.Clone();
        string[] cul = (string[])words.Clone();
        Array.Sort(ord, (a, b) => string.Compare(a, b, StringComparison.Ordinal));
        CultureInfo ukCulture = CultureInfo.GetCultureInfo("uk-UA");
        Array.Sort(cul, (a, b) => string.Compare(a, b, ukCulture, CompareOptions.None));
        return (ord, cul);
    }

    public static List<string> NormalizeNaive(string text)
    {
        List<string> words = new List<string>();
        if (string.IsNullOrEmpty(text)) return words;

        string clean = text.Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ').ToLowerInvariant();
        while (clean.IndexOf("  ", StringComparison.Ordinal) >= 0)
            clean = clean.Replace("  ", " ", StringComparison.Ordinal);

        clean = clean.Trim();
        if (clean.Length == 0) return words;

        string rest = clean;
        while (rest.Length > 0)
        {
            int spaceIdx = rest.IndexOf(' ');
            if (spaceIdx < 0) { words.Add(rest); break; }
            words.Add(rest.Substring(0, spaceIdx));
            rest = rest.Substring(spaceIdx + 1);
        }
        return words;
    }

    public static List<string> NormalizeFast(string text)
    {
        List<string> words = new List<string>();
        if (string.IsNullOrEmpty(text)) return words;

        StringBuilder sb = new StringBuilder(64);
        int i = 0, n = text.Length;
        while (i < n)
        {
            while (i < n && char.IsWhiteSpace(text[i])) i++;
            if (i >= n) break;
            sb.Clear();
            while (i < n && !char.IsWhiteSpace(text[i]))
            {
                sb.Append(char.ToLowerInvariant(text[i]));
                i++;
            }
            words.Add(sb.ToString());
        }
        return words;
    }

    public static int NaiveIndexOf(string text, string pat, out long comparisons)
    {
        comparisons = 0;
        if (pat.Length == 0) return 0;
        if (pat.Length > text.Length) return -1;

        int limit = text.Length - pat.Length;
        for (int i = 0; i <= limit; i++)
        {
            int j = 0;
            while (j < pat.Length)
            {
                comparisons++;
                if (text[i + j] != pat[j]) break;
                j++;
            }
            if (j == pat.Length) return i;
        }
        return -1;
    }
}
