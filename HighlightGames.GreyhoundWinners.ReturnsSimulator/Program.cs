using System.Text.Json;

using MathNet.Numerics.Random;

using HighlightGames.GreyhoundWinners.GameEngine;
using MoreLinq.Extensions;

if (args.Length != 1)
{
    Console.WriteLine("Usage: [<iterations>]");
    
    Environment.Exit(1);
}

int iterations = int.Parse(args[0]);

var random = new MersenneTwister();
var game = new Game();

int[] selectedTraps = game.CreateGame(random).ToArray();
int selectedTrap = 1;

IDictionary<string, decimal> matchMarketReturns = Enumerable.Range(1, 6).ToDictionary(x => x.ToString(), _ => 0m);

IDictionary<string, decimal> sameTrapMarketReturns = new Dictionary<string, decimal>
{
    ["5+"] = 0,
    ["4"] = 0,
    ["3"] = 0,
    ["2"] = 0,
};

IDictionary<string, decimal> highLowMarketReturns = new Dictionary<string, decimal>
{
    ["High"] = 0,
    ["Low"] = 0,
    ["Equal"] = 0,
};

IDictionary<string, decimal> oddEvenMarketReturns = new Dictionary<string, decimal>
{
    ["Odd"] = 0,
    ["Even"] = 0,
    ["Equal"] = 0,
};

IDictionary<string, decimal> trapMostMarketReturns = Enumerable.Range(0, 7).ToDictionary(x => x.ToString(), _ => 0m);

IDictionary<string, decimal> trapMostAnyMarketReturns = new Dictionary<string, decimal>
{
    ["Any"] = 0,
    ["None"] = 0,
};

IDictionary<string, decimal> trapTotalExactMarketReturns = new Dictionary<string, decimal>
{
    ["6"] = 0,
    ["7"] = 0,
    ["8"] = 0,
    ["9"] = 0,
    ["10"] = 0,
    ["11"] = 0,
    ["12"] = 0,
    ["13"] = 0,
    ["14"] = 0,
    ["15"] = 0,
    ["16"] = 0,
    ["17"] = 0,
    ["18"] = 0,
    ["19"] = 0,
    ["20"] = 0,
    ["21"] = 0,
    ["22"] = 0,
    ["23"] = 0,
    ["24"] = 0,
    ["25"] = 0,
    ["26"] = 0,
    ["27"] = 0,
    ["28"] = 0,
    ["29"] = 0,
    ["30"] = 0,
    ["31"] = 0,
    ["32"] = 0,
    ["33"] = 0,
    ["34"] = 0,
    ["35"] = 0,
    ["36"] = 0,
};

IDictionary<string, decimal> trapTotalOddEvenMarketReturns = new Dictionary<string, decimal>
{
    ["Odd"] = 0,
    ["Even"] = 0,
};

IDictionary<string, decimal> trapTotalPrimeMarketReturns = new Dictionary<string, decimal>
{
    ["Yes"] = 0,
    ["No"] = 0,
};

IDictionary<string, decimal> trapTotalRangeMarketReturns = new Dictionary<string, decimal>
{
    ["6"] = 0,
    ["7-16"] = 0,
    ["17-26"] = 0,
    ["27-36"] = 0,
};

IDictionary<string, decimal> catchAMatchMarketReturns = Enumerable.Range(2, 5).ToDictionary(x => x.ToString(), _ => 0m);

IDictionary<string, decimal> playYourDogsRightWithoutInsuranceMarketReturns = new Dictionary<string, decimal>
{
    ["0"] = 0,
    ["1"] = 0,
    ["2"] = 0,
    ["3"] = 0,
    ["4"] = 0,
    ["5"] = 0,
    ["6"] = 0,
};

for (int iteration = 0; iteration < iterations; iteration++)
{
    int[] result = game.CreateGame(random).ToArray();

    for (int matchLength = 1; matchLength <= 6; matchLength++)
    {
        matchMarketReturns[matchLength.ToString()] += Settler.SettleMatchMarket(selectedTraps, matchLength, result);
    }

    for (int matchLength = 2; matchLength <= 6; matchLength++)
    {
        string selection = matchLength >= 5 ? "5+" : matchLength.ToString(); 
        
        sameTrapMarketReturns[selection] += Settler.SettleSameTrapMarket(selectedTrap, matchLength, result);
    }
    
    highLowMarketReturns[Settler.SettleHighLowMarket(result)] += 1;
    oddEvenMarketReturns[Settler.SettleOddEvenMarket(result)] += 1;
    trapMostMarketReturns[Settler.SettleTrapMostMarket(result)?.ToString() ?? "0"] += 1;
    trapMostAnyMarketReturns[Settler.SettleTrapMostAnyMarket(result)] += 1;
    
    trapTotalExactMarketReturns[Settler.SettleTrapTotalExactMarket(result)] += 1;
    trapTotalOddEvenMarketReturns[Settler.SettleTrapTotalOddEvenMarket(result)] += 1;
    trapTotalPrimeMarketReturns[Settler.SettleTrapTotalPrimeMarket(result)] += 1;
    trapTotalRangeMarketReturns[Settler.SettleTrapTotalRangeMarket(result)] += 1;
    
    for (int matchLength = 2; matchLength <= 6; matchLength++)
    {
        catchAMatchMarketReturns[matchLength.ToString()] += Settler.SettleCatchAMatchMarket(new[] { 1, 2, 3, 4, 5, 6}.Shuffle().Take(matchLength).ToArray(), result);
    }

    char[] playYourDogsRightSelection = Enumerable.Range(0, 6).Select(_ => random.NextInt64(2) == 0 ? 'H' : 'L').ToArray();
    playYourDogsRightWithoutInsuranceMarketReturns[Settler.SettlePlayYourDogsRightMarket(playYourDogsRightSelection, result, false).ToString()] += 1;
}

IEnumerable<Market> markets = game.GenerateMarkets().ToList();

Market matchMarket = markets.Single(x => x.name == "GWMatch");
Market sameTrapMarket = markets.Single(x => x.name == "GWSameTrap");
Market highLowMarket = markets.Single(x => x.name == "GWHighLow");
Market oddEvenMarket = markets.Single(x => x.name == "GWOddEven");
Market trapMostMarket = markets.Single(x => x.name == "GWTrapMost");
Market trapMostAnyMarket = markets.Single(x => x.name == "GWTrapMostAny");
Market trapTotalExactMarket = markets.Single(x => x.name == "GWTrapTotalExact");
Market trapTotalOddEvenMarket = markets.Single(x => x.name == "GWTrapTotalOddEven");
Market trapTotalPrimeMarket = markets.Single(x => x.name == "GWTrapTotalPrime");
Market trapTotalRangeMarket = markets.Single(x => x.name == "GWTrapTotalRange");
IDictionary<string, Market> catchAMatchMarkets = Enumerable.Range(2, 5).ToDictionary(
    x => x.ToString(),
    x => markets.Single(y => y.name == $"GWCatchAMatch{x}")
);
Market playYourDogsRightWithoutInsuranceMarket = markets.Single(x => x.name == "GWPlayYourDogsRightWithoutInsurance");


Console.WriteLine(JsonSerializer.Serialize(new { iterations}));

foreach (string selection in matchMarketReturns.Keys)
{
    matchMarketReturns[selection] *= (decimal)matchMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(matchMarketReturns));

foreach (string selection in sameTrapMarketReturns.Keys)
{
    sameTrapMarketReturns[selection] *= (decimal)sameTrapMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(sameTrapMarketReturns));

foreach (string selection in highLowMarketReturns.Keys)
{
    highLowMarketReturns[selection] *= (decimal)highLowMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(highLowMarketReturns));

foreach (string selection in oddEvenMarketReturns.Keys)
{
    oddEvenMarketReturns[selection] *= (decimal)oddEvenMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(oddEvenMarketReturns));

foreach (string selection in trapMostMarketReturns.Keys)
{
    trapMostMarketReturns[selection] *= (decimal)trapMostMarket.selectionFairPrices[selection == "0" ? "None" : selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(trapMostMarketReturns));

foreach (string selection in trapMostAnyMarketReturns.Keys)
{
    trapMostAnyMarketReturns[selection] *= (decimal)trapMostAnyMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(trapMostAnyMarketReturns));

foreach (string selection in trapTotalExactMarketReturns.Keys)
{
    trapTotalExactMarketReturns[selection] *= (decimal)trapTotalExactMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(trapTotalExactMarketReturns));

foreach (string selection in trapTotalOddEvenMarketReturns.Keys)
{
    trapTotalOddEvenMarketReturns[selection] *= (decimal)trapTotalOddEvenMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(trapTotalOddEvenMarketReturns));

foreach (string selection in trapTotalPrimeMarketReturns.Keys)
{
    trapTotalPrimeMarketReturns[selection] *= (decimal)trapTotalPrimeMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(trapTotalPrimeMarketReturns));

foreach (string selection in trapTotalRangeMarketReturns.Keys)
{
    trapTotalRangeMarketReturns[selection] *= (decimal)trapTotalRangeMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(trapTotalRangeMarketReturns));

foreach (string selection in catchAMatchMarketReturns.Keys)
{
    catchAMatchMarketReturns[selection] *= (decimal)catchAMatchMarkets[selection].selectionFairPrices["1"] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(catchAMatchMarketReturns));

foreach (string selection in playYourDogsRightWithoutInsuranceMarketReturns.Keys)
{
    playYourDogsRightWithoutInsuranceMarketReturns[selection] *= (decimal)playYourDogsRightWithoutInsuranceMarket.selectionFairPrices[selection] / iterations;
}

Console.WriteLine(JsonSerializer.Serialize(playYourDogsRightWithoutInsuranceMarketReturns));