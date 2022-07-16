using UnityEngine;

public class Dice
{
    readonly public static int D4 = 4;
    readonly public static int D6 = 6;
    readonly public static int D8 = 8;
    readonly public static int D10 = 10;
    readonly public static int D12 = 12;
    readonly public static int D20 = 20;

    public int Roll(int n, int d)
    {
        int result = 0;
        for (int i = 0; i < n; i++)
        {
            result += Random.Range(1, d + 1);
        }

        return result;
    }
}
