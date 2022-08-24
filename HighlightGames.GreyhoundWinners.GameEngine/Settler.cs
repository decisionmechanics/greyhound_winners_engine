namespace HighlightGames.GreyhoundWinners.GameEngine;

public static class Settler
{
    /* Public static methods */

    public static string SettleHighLowMarket(int[] result)
    {
        int highTrapCount = result.Count(x => x >= 4);

        if (highTrapCount > 3)
        {
            return "High";
        }
        else if (highTrapCount < 3)
        {
            return "Low";
        }
        else
        {
            return "Equal";
        }
    }

    public static string SettleOddEvenMarket(int[] result)
    {
        int oddTrapCount = result.Count(x => x % 2 == 1);

        if (oddTrapCount > 3)
        {
            return "Odd";
        }
        else if (oddTrapCount < 3)
        {
            return "Even";
        }
        else
        {
            return "Equal";
        }
    }

    public static int SettleMatchMarket(int[] selectedTraps, int selection, int[] result)
    {
        int matches = 0;

        for (int startingIndex = 0; startingIndex <= result.Length - selection; startingIndex++)
        {
            bool match = true;

            for (int trapIndex = startingIndex; trapIndex < startingIndex + selection; trapIndex++)
            {
                if (selectedTraps[trapIndex] != result[trapIndex])
                {
                    match = false;

                    break;
                }
            }

            if (match)
            {
                matches++;
            }
        }

        return matches;
    }

    public static int SettleSameTrapMarket(int trap, int selection, int[] result)
    {
        int matches = 0;

        for (int startingIndex = 0; startingIndex <= result.Length - selection; startingIndex++)
        {
            bool match = result.Skip(startingIndex).Take(selection).Count(x => x == trap) == selection;

            if (match)
            {
                matches++;
            }
        }

        return matches;
    }

    public static int? SettleTrapMostMarket(int[] result)
    {
        var trapCounts = result.GroupBy(x => x).Select(x => new { Trap = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count).ToList();

        if (trapCounts.Count == 1)
        {
            return trapCounts.Single().Trap;
        }

        if (trapCounts[0].Count > trapCounts[1].Count)
        {
            return trapCounts.First().Trap;
        }

        return null;
    }

    public static string SettleTrapMostAnyMarket(int[] result) => SettleTrapMostMarket(result) == null ? "None" : "Any";
}