namespace HighlightGames.GreyhoundWinners.GameEngine;

public class Game
{
    /* Private fields */

    private const int MostFrequentTrapOutcomeCount = 4606;

    private readonly double _outcomeCount = Math.Pow(6, 6);

    /* Public static methods */

    public static double CalculatePrice(double fairPrice, double overRound) => fairPrice / overRound;

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
        CreateTrapMostAnyMarket(),
    }.Concat(CreateTrapTotalMarkets());

    /* Private static methods */

    private static double CalculateFairPrice(double probability) => 1 / probability;

    private static int Factorial(int n) => Enumerable.Range(1, n).Aggregate((x, y) => x * y);

    private static int CalculateCombinations(int n, int r) => Factorial(n) / Factorial(n - r) / Factorial(r);

    /* Private instance methods */

    private Market CreateHighLowMarket()
    {
        double highOutcomeCount = Math.Pow(3, 6) + (Math.Pow(3, 1) * Math.Pow(3, 5)) * CalculateCombinations(6, 1) +
                                  (Math.Pow(3, 2) * Math.Pow(3, 4)) * CalculateCombinations(6, 2);

        double highProbability = highOutcomeCount / _outcomeCount;

        double lowProbability = highProbability;
        double equalProbability = 1 - highProbability - lowProbability;

        return new Market("GWHighLow", new Dictionary<string, double>
        {
            ["High"] = CalculateFairPrice(highProbability),
            ["Low"] = CalculateFairPrice(lowProbability),
            ["Equal"] = CalculateFairPrice(equalProbability),
        });
    }

    private Market CreateMatchMarket() => new Market("GWMatch", new Dictionary<string, double>
    {
        ["6"] = CalculateFairPrice((Math.Pow(6, 0) * 1) / _outcomeCount),
        ["5"] = CalculateFairPrice((Math.Pow(6, 1) * 2) / _outcomeCount),
        ["4"] = CalculateFairPrice((Math.Pow(6, 2) * 3) / _outcomeCount),
        ["3"] = CalculateFairPrice((Math.Pow(6, 3) * 4) / _outcomeCount),
        ["2"] = CalculateFairPrice((Math.Pow(6, 4) * 5) / _outcomeCount),
        ["1"] = CalculateFairPrice((Math.Pow(6, 5) * 6) / _outcomeCount),
    });

    private Market CreateOddEvenMarket()
    {
        double highOutcomeCount = Math.Pow(3, 6) + (Math.Pow(3, 1) * Math.Pow(3, 5)) * CalculateCombinations(6, 1) +
                                  (Math.Pow(3, 2) * Math.Pow(3, 4)) * CalculateCombinations(6, 2);

        double oddProbability = highOutcomeCount / _outcomeCount;

        double evenProbability = oddProbability;
        double equalProbability = 1 - oddProbability - evenProbability;

        return new Market("GWOddEven", new Dictionary<string, double>
        {
            ["Odd"] = CalculateFairPrice(oddProbability),
            ["Even"] = CalculateFairPrice(evenProbability),
            ["Equal"] = CalculateFairPrice(equalProbability),
        });
    }

    private Market CreateRaceMarket()
    {
        double probability = 1.0 / 6;
        double fairPrice = CalculateFairPrice(probability);

        return new Market("GWRace", new Dictionary<string, double>
        {
            ["1"] = fairPrice,
            ["2"] = fairPrice,
            ["3"] = fairPrice,
            ["4"] = fairPrice,
            ["5"] = fairPrice,
            ["6"] = fairPrice,
        });
    }

    private Market CreateSameTrapMarket() => new Market("GWSameTrap", new Dictionary<string, double>
    {
        ["5+"] = CalculateFairPrice((Math.Pow(6, 0) * 1 + Math.Pow(6, 1) * 2) / _outcomeCount),
        ["4"] = CalculateFairPrice(Math.Pow(6, 2) * 3 / _outcomeCount),
        ["3"] = CalculateFairPrice(Math.Pow(6, 3) * 4 / _outcomeCount),
        ["2"] = CalculateFairPrice(Math.Pow(6, 4) * 5 / _outcomeCount),
    });

    private Market CreateTrapMostAnyMarket()
    {
        double trapProbability = MostFrequentTrapOutcomeCount / _outcomeCount;
        double anyTrapProbability = trapProbability * 6;

        return new Market("GWTrapMostAny", new Dictionary<string, double>
        {
            ["None"] = CalculateFairPrice(1 - anyTrapProbability),
            ["Any"] = CalculateFairPrice(anyTrapProbability),
        });
    }

    private Market CreateTrapMostMarket()
    {
        var selectionFairPrices = new Dictionary<string, double>();

        double trapProbability = MostFrequentTrapOutcomeCount / _outcomeCount;

        selectionFairPrices["None"] = CalculateFairPrice(1 - trapProbability * 6);

        for (int i = 1; i <= 6; i++)
        {
            selectionFairPrices[i.ToString()] = CalculateFairPrice(trapProbability);
        }

        return new Market("GWTrapMost", selectionFairPrices);
    }

    private IEnumerable<Market> CreateTrapTotalMarkets()
    {
        var exactMarket = new Market("GWTrapTotalExact", new Dictionary<string, double>
        {
            ["6"] = CalculateFairPrice(1 / _outcomeCount),
            ["7"] = CalculateFairPrice(6 / _outcomeCount),
            ["8"] = CalculateFairPrice(21 / _outcomeCount),
            ["9"] = CalculateFairPrice(56 / _outcomeCount),
            ["10"] = CalculateFairPrice(126 / _outcomeCount),
            ["11"] = CalculateFairPrice(252 / _outcomeCount),
            ["12"] = CalculateFairPrice(456 / _outcomeCount),
            ["13"] = CalculateFairPrice(756 / _outcomeCount),
            ["14"] = CalculateFairPrice(1161 / _outcomeCount),
            ["15"] = CalculateFairPrice(1666 / _outcomeCount),
            ["16"] = CalculateFairPrice(2247 / _outcomeCount),
            ["17"] = CalculateFairPrice(2856 / _outcomeCount),
            ["18"] = CalculateFairPrice(3431 / _outcomeCount),
            ["19"] = CalculateFairPrice(3906 / _outcomeCount),
            ["20"] = CalculateFairPrice(4221 / _outcomeCount),
            ["21"] = CalculateFairPrice(4332 / _outcomeCount),
            ["22"] = CalculateFairPrice(4221 / _outcomeCount),
            ["23"] = CalculateFairPrice(3906 / _outcomeCount),
            ["24"] = CalculateFairPrice(3431 / _outcomeCount),
            ["25"] = CalculateFairPrice(2856 / _outcomeCount),
            ["26"] = CalculateFairPrice(2247 / _outcomeCount),
            ["27"] = CalculateFairPrice(1666 / _outcomeCount),
            ["28"] = CalculateFairPrice(1161 / _outcomeCount),
            ["29"] = CalculateFairPrice(756 / _outcomeCount),
            ["30"] = CalculateFairPrice(456 / _outcomeCount),
            ["31"] = CalculateFairPrice(252 / _outcomeCount),
            ["32"] = CalculateFairPrice(126 / _outcomeCount),
            ["33"] = CalculateFairPrice(56 / _outcomeCount),
            ["34"] = CalculateFairPrice(21 / _outcomeCount),
            ["35"] = CalculateFairPrice(6 / _outcomeCount),
            ["36"] = CalculateFairPrice(1 / _outcomeCount),
        });

        var oddEvenMarket = new Market("GWTrapTotalOddEven", new Dictionary<string, double>
        {
            ["Odd"] = CalculateFairPrice(23328 / _outcomeCount),
            ["Even"] = CalculateFairPrice(23328 / _outcomeCount),
        });

        var primeMarket = new Market("GWTrapTotalPrime", new Dictionary<string, double>
        {
            ["Yes"] = CalculateFairPrice(12690 / _outcomeCount),
            ["No"] = CalculateFairPrice(33966 / _outcomeCount),
        });

        var rangeMarket = new Market("GWTrapTotalRange", new Dictionary<string, double>
        {
            ["6"] = CalculateFairPrice(1 / _outcomeCount),
            ["7-16"] = CalculateFairPrice(6747 / _outcomeCount),
            ["17-26"] = CalculateFairPrice(35407 / _outcomeCount),
            ["27-36"] = CalculateFairPrice(4501 / _outcomeCount),
        });

        return new[] { exactMarket, oddEvenMarket, primeMarket, rangeMarket };
    }
}