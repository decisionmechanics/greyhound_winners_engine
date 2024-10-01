namespace ABetA.GreyhoundWinners.GameEngine;

using System.Collections;

public enum Direction {
    Higher,
    Lower
}

public class Settler
{
    /* Private fields */

    private readonly IDictionary<string, double> _catchAMatchMarketPrices;
    private readonly IDictionary<string, double> _superMatchWinMarketPrices;
    private readonly IDictionary<string, double> _superMatchWithABreakMarketPrices;
    private readonly IDictionary<string, double> _trapNumbersWinningMostHighLowMarketPrices;
    private readonly IDictionary<string, double> _trapNumbersWinningMostOddEvenMarketPrices;
    private readonly IDictionary<string, double> _trapNumbersWinningMostTopTrapMarketPrices;
    private readonly IDictionary<string, double> _winningTrapsTotalsTrapsTotalMarketPrices;
    private readonly IDictionary<string, double> _winningTrapsTotalsRangeMarketPrices;
    private readonly IDictionary<string, double> _winningTrapsTotalsSumMarketPrices;
    
    /* Constructors */

    public Settler()
    {
        Game game = new();

        IEnumerable<Market> markets = game.GenerateMarkets().ToList();
        
        _superMatchWinMarketPrices = markets.Single(m => m.Name == "GWSuperMatchWin").SelectionFairPrices;
        _superMatchWithABreakMarketPrices = markets.Single(m => m.Name == "GWSuperMatchWithABreak").SelectionFairPrices;
        _catchAMatchMarketPrices = markets.Single(m => m.Name == "GWCatchAMatch").SelectionFairPrices;
        _trapNumbersWinningMostHighLowMarketPrices = markets.Single(m => m.Name == "GWTrapNumbersWinningMostHighLow").SelectionFairPrices;
        _trapNumbersWinningMostOddEvenMarketPrices = markets.Single(m => m.Name == "GWTrapNumbersWinningMostOddEven").SelectionFairPrices;
        _trapNumbersWinningMostTopTrapMarketPrices = markets.Single(m => m.Name == "GWTrapNumbersWinningMostTopTrap").SelectionFairPrices;
        _winningTrapsTotalsTrapsTotalMarketPrices = markets.Single(m => m.Name == "GWWinningTrapsTotalsTrapsTotal").SelectionFairPrices;
        _winningTrapsTotalsRangeMarketPrices = markets.Single(m => m.Name == "GWWinningTrapsTotalsRange").SelectionFairPrices;
        _winningTrapsTotalsSumMarketPrices = markets.Single(m => m.Name == "GWWinningTrapsTotalsSum").SelectionFairPrices;
    }

    /* Public instance methods */
    
    public IEnumerable<Settlement> SettleCatchAMatchMarket(int[] result)
    {
        var trapCounts = result.GroupBy(t => t).Select(g => g.Count()).OrderByDescending(c => c).ToArray();

        ICollection<Settlement> settlements = new List<Settlement>();

        if (result.Zip(Enumerable.Range(1, 6)).All(x => x.First == x.Second))
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Six Going Up", 1, (decimal)_catchAMatchMarketPrices["Six Going Up"]));
        }

        if (result.Zip(Enumerable.Range(1, 6).Reverse()).All(x => x.First == x.Second))
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Six Coming Down", 1, (decimal)_catchAMatchMarketPrices["Six Coming Down"]));
        }
        
        if (trapCounts.Length == 6)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Crowded House", 1, (decimal)_catchAMatchMarketPrices["Crowded House"]));
        }

        if (trapCounts[0] == 4 && trapCounts[1] == 2)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Full Traps", 1, (decimal)_catchAMatchMarketPrices["Full Traps"]));
        }

        if (trapCounts[0] == 3 && trapCounts[1] == 3)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Half Traps", 1, (decimal)_catchAMatchMarketPrices["Half Traps"]));
        }

        if (trapCounts[0] == 3 && trapCounts[1] == 2)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Three Two", 1, (decimal)_catchAMatchMarketPrices["Three Two"]));
        }

        if (trapCounts[0] == 6)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Super Six", 1, (decimal)_catchAMatchMarketPrices["Super Six"]));
        }
        
        if (trapCounts[0] == 5)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Five Up", 1, (decimal)_catchAMatchMarketPrices["Five Up"]));
        }
        
        if (trapCounts[0] == 4)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Foursome", 1, (decimal)_catchAMatchMarketPrices["Foursome"]));
        }
        
        if (trapCounts[0] == 3)
        {
            settlements.Add(new Settlement("GWCatchAMatch", "Threesome", 1, (decimal)_catchAMatchMarketPrices["Threesome"]));
        }

        return settlements.Take(1).ToList();
    }

    public IEnumerable<Settlement> SettlePlayYourDogsRightMarket(Direction[] selection, int[] result)
    {
        var betGroup = GetPlayYourDogsRightBetGroup(selection);
        var run = CalculatePlayYourDogRightRun(selection, result);

        var dividend = (betGroup, run) switch
        {
            (1, 6) => 8m,
            (1, 5) => 5m,
            (1, 4) => 3m,
            (1, 3) => 2m,
            (2, 6) => 13m,
            (2, 5) => 5m,
            (2, 4) => 3m,
            (2, 3) => 2m,
            (3, 6) => 40m,
            (3, 5) => 9m,
            (3, 4) => 3m,
            (3, 3) => 2m,
            (4, 6) => 400m,
            (4, 5) => 30m,
            (4, 4) => 5m,
            (4, 3) => 2m,
            (5, 6) => 10_000m,
            (5, 5) => 250m,
            (5, 4) => 20m,
            (5, 3) => 3m,
            _ => 0m
        };
            
        return new[] { new Settlement("GWPlayYourDogsRight", "", 1, dividend) };
    }
    
    public IEnumerable<Settlement> SettleSuperMatchWinMarket(int[] selection, int[] result) =>
        Enumerable.Range(2, 5).Select(l => SettleSuperMatchWinMarket(l, selection, result)).Where(s => s.Dividends > 0).ToList();

    public IEnumerable<Settlement> SettleSuperMatchWithABreakMarket(int[] selection, int[] result) =>
        Enumerable.Range(2, 5).Select(l => SettleSuperMatchWithABreakMarket(l, selection, result)).Where(s => s.Dividends > 0).ToList();

    public IEnumerable<Settlement> SettleSyntheticRaceMarket(int selection) => [new Settlement("GWSyntheticRace", selection.ToString(), 1, 6)];
    
    public IEnumerable<Settlement> SettleTrapNumbersWinningMostHighLowMarket(int[] result)
    {
        int highTrapCount = result.Count(t => t >= 4);

        if (highTrapCount > 3)
        {
            return new[] { new Settlement("GWTrapNumbersWinningMostHighLow", "High", 1, (decimal)_trapNumbersWinningMostHighLowMarketPrices["High"]) };
        }
        else if (highTrapCount < 3)
        {
            return new[] { new Settlement("GWTrapNumbersWinningMostHighLow", "Low", 1, (decimal)_trapNumbersWinningMostHighLowMarketPrices["Low"]) };
        }
        else
        {
            return new[] { new Settlement("GWTrapNumbersWinningMostHighLow", "Equal", 1, (decimal)_trapNumbersWinningMostHighLowMarketPrices["Equal"]) };
        }
    }
    
    public IEnumerable<Settlement> SettleTrapNumbersWinningMostOddEvenMarket(int[] result)
    {
        int oddTrapCount = result.Count(t => t % 2 == 1);
        
        if (oddTrapCount > 3)
        {
            return new[] { new Settlement("GWTrapNumbersWinningMostOddEven", "Odd", 1, (decimal)_trapNumbersWinningMostOddEvenMarketPrices["Odd"]) };
        }
        else if (oddTrapCount < 3)
        {
            return new[] { new Settlement("GWTrapNumbersWinningMostOddEven", "Even", 1, (decimal)_trapNumbersWinningMostOddEvenMarketPrices["Even"]) };
        }
        else
        {
            return new[] { new Settlement("GWTrapNumbersWinningMostOddEven", "Equal", 1, (decimal)_trapNumbersWinningMostOddEvenMarketPrices["Equal"]) };
        }
    }

    public IEnumerable<Settlement> SettleTrapNumbersWinningMostTopTrapMarket(int[] result)
    {
        var trapCounts = result.GroupBy(t => t).Select(g => new { Trap = g.Key, Count = g.Count() }).OrderByDescending(c => c.Count).Take(2).ToArray();
        
        ICollection<Settlement> settlements = new List<Settlement>();
        
        if (trapCounts.Length == 1 || trapCounts[0].Count > trapCounts[1].Count)
        {
            var topTrap = trapCounts[0].Trap.ToString();

            settlements.Add(new Settlement("GWTrapNumbersWinningMostTopTrap", topTrap, 1, (decimal)_trapNumbersWinningMostTopTrapMarketPrices[topTrap]));
            settlements.Add(new Settlement("GWTrapNumbersWinningMostTopTrap", "Any", 1, (decimal)_trapNumbersWinningMostTopTrapMarketPrices["Any"]));            
        }
        else
        {
            settlements.Add(new Settlement("GWTrapNumbersWinningMostTopTrap", "None", 1, (decimal)_trapNumbersWinningMostTopTrapMarketPrices["None"]));
        }

        return settlements;
    }

    public IEnumerable<Settlement> SettleWinningTrapsTotalsRangeMarket(int[] result)
    {
        var trapsTotalRange = result.Sum() switch
        {
            6 => "6",
            >= 7 and <= 21 => "7-21",
            <= 36 => "22-36",
            _ => throw new ArgumentOutOfRangeException(nameof(result), "Trap numbers must sum to between 6 and 36"),
        };
        
        return new[] { new Settlement("GWWinningTrapsTotalsRange", trapsTotalRange, 1, (decimal)_winningTrapsTotalsRangeMarketPrices[trapsTotalRange]) };
    }

    public IEnumerable<Settlement> SettleWinningTrapsTotalsSumMarket(int[] result)
    {
        var trapsTotal = result.Sum();

        ICollection<Settlement> settlements = new List<Settlement>();

        settlements.Add(trapsTotal % 2 == 1
            ? new Settlement("GWWinningTrapsTotalsSum", "Odd", 1, (decimal)_winningTrapsTotalsSumMarketPrices["Odd"])
            : new Settlement("GWWinningTrapsTotalsSum", "Even", 1, (decimal)_winningTrapsTotalsSumMarketPrices["Even"]));

        if (new[] { 7, 11, 13, 17, 19, 23, 29, 31 }.Contains(trapsTotal))
        {
            settlements.Add(new Settlement("GWWinningTrapsTotalsSum", "Prime", 1, (decimal)_winningTrapsTotalsSumMarketPrices["Prime"]));
        }

        return settlements;
    }

    public IEnumerable<Settlement> SettleWinningTrapsTotalsTrapsTotalMarket(int[] result)
    {
        var trapsTotal = result.Sum().ToString();
        
        return new[] { new Settlement("GWWinningTrapsTotalsTrapsTotal", trapsTotal, 1, (decimal)_winningTrapsTotalsTrapsTotalMarketPrices[trapsTotal]) };
    }
    
    /* Private static methods */

    private static int CalculatePlayYourDogRightRun(Direction[] selection, int[] result)
    {
        var previousTrap = 3.5;

        int run = 0;
        
        for (var i = 0; i < 6; i++)
        {
            if ((selection[i] == Direction.Lower && result[i] < previousTrap) || (selection[i] == Direction.Higher && result[i] > previousTrap))
            {
                run++;
            }
            else if (i > 0)
            {
                break;
            }
            
            previousTrap = result[i];
        }

        return run;
    }
    
    private static int GetPlayYourDogsRightBetGroup(Direction[] selection)
    {
        Direction? previousDirection = null;

        var longestRun = 0;
        var currentRun = 0;

        foreach (Direction direction in selection)
        {
            if (previousDirection == null || direction == previousDirection)
            {
                currentRun++;
            }
            else
            {
                if (currentRun > longestRun)
                {
                    longestRun = currentRun;
                }

                currentRun = 0;
            }

            previousDirection = direction;
        }
        
        return Math.Min(longestRun, 5);
    }
    
    /* Private instance methods */
    
    private Settlement SettleSuperMatchWinMarket(int matchLength, int[] selection, int[] result)
    {
        var matches = selection.Zip(result).Select(x => x.First == x.Second).ToArray();
        
        var dividends = 0;
        
        for (var i = 0; i < 7 - matchLength; i++)
        {
            var runFound = true;
            
            for (var j = 0; j < matchLength; j++)
            {
                if (!matches[i + j])
                {
                    runFound = false;
                    
                    break;
                }
            }

            if (runFound)
            {
                dividends++;
            }
        }

        var winningSelection = matchLength.ToString();
        
        return new Settlement("GWSuperMatchWin", winningSelection, dividends, (decimal)_superMatchWinMarketPrices[winningSelection]);
    }

    private Settlement SettleSuperMatchWithABreakMarket(int matchLength, int[] selection, int[] result)
    {
        var matches = selection.Zip(result).Select(x => x.First == x.Second).ToArray();
        
        var dividends = 0;

        foreach (var sequenceIndexes in Combinations.Calculate(Enumerable.Range(0, 6).ToArray(), matchLength))
        {
            if (sequenceIndexes.All(i => matches[i]))
            {
                dividends++;
            }
        }
        
        var winningSelection = matchLength.ToString();
        
        return new Settlement("GWSuperMatchWithABreak", winningSelection, dividends, (decimal)_superMatchWithABreakMarketPrices[winningSelection]);
    }
}