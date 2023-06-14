namespace HighlightGames.GreyhoundWinners.GameEngine;

public static class Settler
{
    /* Public static methods */

    public static int SettleCatchAMatchMarket(int[] selection, int[] result)
    {
        selection = selection.OrderBy(x => x).ToArray();

        IEnumerable<IList<int>> potentialOutcomes = result.Combinations(selection.Length).Select(x => x.OrderBy(y => y).ToList()).ToList();

        int winningLines = 0;

        foreach (IEnumerable<int> potentialOutcome in potentialOutcomes)
        {
            if (selection.SequenceEqual(potentialOutcome))
            {
                winningLines++;
            }
        }

        return winningLines;
    }

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

    public static int SettlePlayYourDogsRightMarket(char[] selection, int[] result, bool insurance)
    {
        int matchingSequenceLength = 0;

        int previousTrap = -1;

        for (int i = 0; i < result.Length; i++)
        {
            bool correct;

            if (i == 0)
            {
                correct = (selection[0] == 'H' && result[0] >= 4) || (selection[0] == 'L' && result[0] <= 3);
            }
            else
            {
                correct = (selection[i] == 'H' && result[i] > previousTrap) || (selection[i] == 'L' && result[i] < previousTrap);
            }

            if (correct)
            {
                matchingSequenceLength++;
            }
            else if (insurance && ((i == 0 && (result[0] == 3 || result[0] == 4)) || result[i] == previousTrap))
            {
                insurance = false;
            }
            else
            {
                break;
            }

            previousTrap = result[i];
        }

        return matchingSequenceLength;
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

    public static string SettleTrapTotalExactMarket(int[] result) => result.Sum().ToString();

    public static string SettleTrapTotalOddEvenMarket(int[] result) => result.Sum() % 2 == 1 ? "Odd" : "Even";

    public static string SettleTrapTotalPrimeMarket(int[] result) => new[] { 7, 11, 13, 17, 19, 23, 29, 31 }.Contains(result.Sum()) ? "Yes" : "No";

    public static string SettleTrapTotalRangeMarket(int[] result) => result.Sum() switch
    {
        6 => "6",
        >= 7 and <= 16 => "7-16",
        >= 17 and <= 26 => "17-26",
        >= 27 and <= 36 => "27-36",
        _ => throw new ArgumentOutOfRangeException(nameof(result), "Trap numbers must sum to between 6 and 36"),
    };
    
    public static string SettleTrapTotalRangeReducedMarket(int[] result) => result.Sum() switch
    {
        6 => "6",
        >= 7 and <= 21 => "7-21",
        >= 22 and <= 36 => "22-36",
        _ => throw new ArgumentOutOfRangeException(nameof(result), "Trap numbers must sum to between 6 and 36"),
    };
}