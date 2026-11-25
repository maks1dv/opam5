using System.Text;

namespace Lab10;

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        const int N = 8, K = 17;

        string prosePath = Path.Combine("Lab10", "prose.txt");
        if (!File.Exists(prosePath)) prosePath = "prose.txt";

        string proseText = File.ReadAllText(prosePath, Encoding.UTF8);
        Console.WriteLine($"Завантажено текст прози: {proseText.Length:N0} символів.\n");

        Console.WriteLine("=== КРАЙОВІ ВИПАДКИ ===");
        Console.WriteLine($"1) Порожній рядок (зав. 2, 3): слів = {StringAlgorithms.NormalizeFast("").Count}, індекс = {StringAlgorithms.NaiveIndexOf("", "тест", out _)}");
        Console.WriteLine($"2) Рядок з одного символу (зав. 2): [{string.Join(", ", StringAlgorithms.NormalizeFast("М"))}]");
        Console.WriteLine($"3) Усі символи однакові (зав. 2): [{string.Join(", ", StringAlgorithms.NormalizeFast("мммммм"))}]");
        Console.WriteLine($"4) Пробіли на початку, в кінці й подвійні всередині (зав. 2): [{string.Join(", ", StringAlgorithms.NormalizeFast("   Захар    Беркут   Максим   "))}]");
        Console.WriteLine($"5) Шаблон довший за текст (зав. 3): індекс = {StringAlgorithms.NaiveIndexOf("коротко", "дуже_довгий_шаблон", out _)}");
        Console.WriteLine($"6) Порожній шаблон (зав. 3): індекс = {StringAlgorithms.NaiveIndexOf("текст", "", out _)}");
        Console.WriteLine($"7) Нуль і один елемент/сума (зав. 4): сума 0 -> ({GreedyAndBruteForce.ChangeGreedy(0, new[] { 5, 4, 1 })}, {GreedyAndBruteForce.ChangeBruteForce(0, new[] { 5, 4, 1 })}); сума 1 -> ({GreedyAndBruteForce.ChangeGreedy(1, new[] { 5, 4, 1 })}, {GreedyAndBruteForce.ChangeBruteForce(1, new[] { 5, 4, 1 })})\n");

        Benchmark.RunAll(N, K, proseText);
    }
}
