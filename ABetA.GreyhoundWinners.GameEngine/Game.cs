namespace ABetA.GreyhoundWinners.GameEngine;

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
        CreateSyntheticRaceMarket(),
        CreateSuperMatchWinMarket(),
        CreateSuperMatchWithABreakMarket(),
        CreateCatchAMatchMarket(),
        CreateTrapNumbersWinningMostHighLowMarket(),
        CreateTrapNumbersWinningMostOddEvenMarket(),
        CreateTrapNumbersWinningMostTopTrapMarket(),
        CreateWinningTrapsTotalsTrapsTotalMarket(),
        CreateWinningTrapsTotalsRangeMarket(),
        CreateWinningTrapsTotalsSumMarket()
    }.Concat(Enumerable.Range(1, 5).Select(CreatePlayYourDogsRightMarket));

    /* Private static methods */

    private static double CalculateFairPrice(double probability) => 1 / probability;

    private static int Factorial(int n) => n > 0 ? Enumerable.Range(1, n).Aggregate((x, y) => x * y) : 1;

    private static int CalculateCombinations(int n, int r) => Factorial(n) / Factorial(n - r) / Factorial(r);

    private static Market CreateSyntheticRaceMarket() => new(
        "GWSyntheticRace",
        new Dictionary<string, double>
        {
            ["1"] = 6,
            ["2"] = 6,
            ["3"] = 6,
            ["4"] = 6,
            ["5"] = 6,
            ["6"] = 6
        });

    /* Private instance methods */

    private Market CreateCatchAMatchMarket() => new(
        "GWCatchAMatch",
        new Dictionary<string, double>
        {
            ["Crowded House"] = _outcomeCount / 720,
            ["Threesome"] = _outcomeCount / 17136,
            ["Foursome"] = _outcomeCount / 2436,
            ["Five Up"] = _outcomeCount / 186,
            ["Super Six"] = _outcomeCount / 6,
            ["Six Going Up"] = _outcomeCount,
            ["Six Coming Down"] = _outcomeCount,
            ["Full Traps"] = _outcomeCount / 456,
            ["Half Traps"] = _outcomeCount / 306,
            ["Three Two"] = _outcomeCount / 8136,
        });

    private Market CreatePlayYourDogsRightMarket(int group) => new($@"GWPlayYourDogsRightGroup{group}", group switch
    {
        1 => new Dictionary<string, double>
        {
            ["6"] = 8,
            ["5"] = 5,
            ["4"] = 3,
            ["3"] = 2
        },
        2 => new Dictionary<string, double>
        {
            ["6"] = 13,
            ["5"] = 5,
            ["4"] = 3,
            ["3"] = 2
        },
        3 => new Dictionary<string, double>
        {
            ["6"] = 40,
            ["5"] = 9,
            ["4"] = 3,
            ["3"] = 2
        },
        4 => new Dictionary<string, double>
        {
            ["6"] = 400,
            ["5"] = 30,
            ["4"] = 5,
            ["3"] = 2
        },
        5 => new Dictionary<string, double>
        {
            ["6"] = 10_000,
            ["5"] = 250,
            ["4"] = 20,
            ["3"] = 3
        },
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    });

    private Market CreateSuperMatchWinMarket() => new(
        "GWSuperMatchWin",
        new Dictionary<string, double>
        {
            ["6"] = _outcomeCount,
            ["5"] = _outcomeCount / 12,
            ["4"] = _outcomeCount / 108,
            ["3"] = _outcomeCount / 864,
            ["2"] = _outcomeCount / 6480,
        });

    private Market CreateSuperMatchWithABreakMarket() => new(
        "GWSuperMatchWithABreak",
        new Dictionary<string, double>
        {
            ["6"] = _outcomeCount / 1,
            ["5"] = _outcomeCount / 36,
            ["4"] = _outcomeCount / 540,
            ["3"] = _outcomeCount / 4320,
            ["2"] = _outcomeCount / 19440,
        });

    private Market CreateTrapNumbersWinningMostHighLowMarket()
    {
        var highOutcomeCount =
            Math.Pow(3, 6) + Math.Pow(3, 1) * Math.Pow(3, 5) * CalculateCombinations(6, 1) + Math.Pow(3, 2) * Math.Pow(3, 4) * CalculateCombinations(6, 2);

        var highProbability = highOutcomeCount / _outcomeCount;

        var lowProbability = highProbability;
        var equalProbability = 1 - highProbability - lowProbability;

        return new Market("GWTrapNumbersWinningMostHighLow", new Dictionary<string, double>
        {
            ["High"] = CalculateFairPrice(highProbability),
            ["Low"] = CalculateFairPrice(lowProbability),
            ["Equal"] = CalculateFairPrice(equalProbability)
        });
    }

    private Market CreateTrapNumbersWinningMostOddEvenMarket()
    {
        var highOutcomeCount = Math.Pow(3, 6) + Math.Pow(3, 1) * Math.Pow(3, 5) * CalculateCombinations(6, 1) + Math.Pow(3, 2) * Math.Pow(3, 4) * CalculateCombinations(6, 2);

        var oddProbability = highOutcomeCount / _outcomeCount;

        var evenProbability = oddProbability;
        var equalProbability = 1 - oddProbability - evenProbability;

        return new Market("GWTrapNumbersWinningMostOddEven", new Dictionary<string, double>
        {
            ["Odd"] = CalculateFairPrice(oddProbability),
            ["Even"] = CalculateFairPrice(evenProbability),
            ["Equal"] = CalculateFairPrice(equalProbability)
        });
    }

    private Market CreateTrapNumbersWinningMostTopTrapMarket()
    {
        var selectionFairPrices = new Dictionary<string, double>();

        var trapProbability = MostFrequentTrapOutcomeCount / _outcomeCount;

        selectionFairPrices["None"] = CalculateFairPrice(1 - trapProbability * 6);

        for (var i = 1; i <= 6; i++) selectionFairPrices[i.ToString()] = CalculateFairPrice(trapProbability);

        var anyTrapProbability = trapProbability * 6;

        selectionFairPrices["Any"] = CalculateFairPrice(anyTrapProbability);

        return new Market("GWTrapNumbersWinningMostTopTrap", selectionFairPrices);
    }

    private Market CreateWinningTrapsTotalsRangeMarket() => new(
        "GWWinningTrapsTotalsRange",
        new Dictionary<string, double>
        {
            ["6"] = CalculateFairPrice(1 / _outcomeCount),
            ["7-21"] = CalculateFairPrice(25493 / _outcomeCount),
            ["22-36"] = CalculateFairPrice(21162 / _outcomeCount)
        });

    private Market CreateWinningTrapsTotalsSumMarket() => new(
        "GWWinningTrapsTotalsSum",
        new Dictionary<string, double>
        {
            ["Odd"] = CalculateFairPrice(23328 / _outcomeCount),
            ["Even"] = CalculateFairPrice(23328 / _outcomeCount),
            ["Prime"] = CalculateFairPrice(12690 / _outcomeCount)
        });

    private Market CreateWinningTrapsTotalsTrapsTotalMarket() => new(
        "GWWinningTrapsTotalsTrapsTotal",
        new Dictionary<string, double>
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
            ["36"] = CalculateFairPrice(1 / _outcomeCount)
        });
}