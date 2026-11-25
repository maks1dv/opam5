namespace Lab10;

public static class GreedyAndBruteForce
{
    public static int ChangeGreedy(int amount, int[] coinsDesc)
    {
        if (amount <= 0) return 0;
        int count = 0, rem = amount;
        for (int i = 0; i < coinsDesc.Length; i++)
        {
            if (coinsDesc[i] <= rem)
            {
                int take = rem / coinsDesc[i];
                count += take;
                rem -= take * coinsDesc[i];
            }
        }
        return rem == 0 ? count : -1;
    }

    public static int ChangeBruteForce(int amount, int[] coins)
    {
        if (amount <= 0) return 0;
        int best = int.MaxValue;
        SearchRec(amount, coins, 0, 0, ref best);
        return best == int.MaxValue ? -1 : best;
    }

    private static void SearchRec(int rem, int[] coins, int index, int currentCoins, ref int best)
    {
        if (rem == 0) { if (currentCoins < best) best = currentCoins; return; }
        if (index >= coins.Length) return;
        int coin = coins[index];
        for (int c = rem / coin; c >= 0; c--)
            SearchRec(rem - c * coin, coins, index + 1, currentCoins + c, ref best);
    }
}
