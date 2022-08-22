namespace HighlightGames.GreyhoundWinners.GameEngine;

public class Game
{
    /* Private fields */

    private const int MostFrequentTrapOutcomeCount = 4606;
    
    private readonly double _outcomeCount = Math.Pow(6, 6);

    /* Public instance methods */

    public IEnumerable<int> CreateGame(Random random) => Enumerable.Range(0, 6).Select(_ => random.Next(1, 7));

    public IEnumerable<Market> GenerateMarkets() => new[]
    {
        CreateRaceMarket(),
        CreateMatchMarket(),
        CreateSameTrapMarket(),
        CreateHighLowMarket(),
        CreateOddEvenMarket(),
        CreateTrapMostMarket(),
        CreateTrapMostAnyMarket()
    };

    /* Private static methods */

    private static int Factorial(int n) => Enumerable.Range(1, n).Aggregate((x, y) => x * y);

    private static int CalculateCombinations(int n, int r) => Factorial(n) / Factorial(n - r) / Factorial(r);
    
    /* Private instance methods */

    private Market CreateHighLowMarket()
    {
        double highOutcomeCount = Math.Pow(3, 6) + (Math.Pow(3, 1) * Math.Pow(3, 5)) * CalculateCombinations(6, 1) +
                                  (Math.Pow(3, 2) * Math.Pow(3, 4)) * CalculateCombinations(6, 2);
        
        double highProbability = highOutcomeCount  / _outcomeCount;
        
        double lowProbability = highProbability;
        double equalProbability = 1 - highProbability - lowProbability;
        
        return new Market("GWHighLow", new Dictionary<string, double>
        {
            ["High"] = highProbability,
            ["Low"] = lowProbability,
            ["Equal"] = equalProbability,
        });
    }

    private Market CreateMatchMarket() => new Market("GWMatch", new Dictionary<string, double>
    {
        ["6"] = 1 / _outcomeCount,
        ["5"] = 10 / _outcomeCount,
        ["4"] = 85 / _outcomeCount,
        ["3"] = 660 / _outcomeCount,
        ["2"] = 4860 / _outcomeCount,
        ["1"] = 34560 / _outcomeCount,
    });

    private Market CreateOddEvenMarket()
    {
        double highOutcomeCount = Math.Pow(3, 6) + (Math.Pow(3, 1) * Math.Pow(3, 5)) * CalculateCombinations(6, 1) +
                                  (Math.Pow(3, 2) * Math.Pow(3, 4)) * CalculateCombinations(6, 2);
        
        double oddProbability = highOutcomeCount  / _outcomeCount;
        
        double evenProbability = oddProbability;
        double equalProbability = 1 - oddProbability - evenProbability;
        
        return new Market("GWOddEven", new Dictionary<string, double>
        {
            ["Odd"] = oddProbability,
            ["Even"] = evenProbability,
            ["Equal"] = equalProbability,
        });
    }

    private Market CreateRaceMarket()
    {
        double probability = 1.0 / 6.0;
        
        return new Market("GWRace", new Dictionary<string, double>
        {
            ["1"] = probability,
            ["2"] = probability,
            ["3"] = probability,
            ["4"] = probability,
            ["5"] = probability,
            ["6"] = probability,
        });
    }

    private Market CreateSameTrapMarket() => new Market("GWSameTrap", new Dictionary<string, double>
    {
        ["5+"] = (Math.Pow(6, 0) * 1 + Math.Pow(6, 1) * 2) / _outcomeCount,
        ["4"] = Math.Pow(6, 2) * 3 / _outcomeCount,
        ["3"] = Math.Pow(6, 3) * 4 / _outcomeCount,
        ["2"] = Math.Pow(6, 4) * 5 / _outcomeCount,
        ["1"] = Math.Pow(5, 6) / _outcomeCount,
    });

    private Market CreateTrapMostAnyMarket()
    {
        double trapProbability = MostFrequentTrapOutcomeCount / _outcomeCount;
        double anyTrapProbability = trapProbability * 6;

        return new Market("GWTrapMostAny", new Dictionary<string, double>
        {
            ["None"] = 1 - anyTrapProbability,
            ["Any"] = anyTrapProbability,
        });
    }

    private Market CreateTrapMostMarket()
    {
        var selectionProbabilities = new Dictionary<string, double>();
        
        double trapProbability = MostFrequentTrapOutcomeCount / _outcomeCount;

        selectionProbabilities["None"] = 1 - trapProbability * 6;
        
        for (int i = 1; i <= 6; i++)
        {
            selectionProbabilities[i.ToString()] = trapProbability;
        }
        
        return new Market("GWTrapMost", selectionProbabilities);
    }
}