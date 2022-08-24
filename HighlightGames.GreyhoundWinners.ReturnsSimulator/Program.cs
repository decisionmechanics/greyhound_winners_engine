using System.Text.Json;

using MathNet.Numerics.Random;

using HighlightGames.GreyhoundWinners.GameEngine;

const int iterations = 10_000_000;

Console.WriteLine($"{iterations} iterations");

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
}

IEnumerable<Market> markets = game.GenerateMarkets().ToList();

Market matchMarket = markets.Single(x => x.name == "GWMatch");
Market sameTrapMarket = markets.Single(x => x.name == "GWSameTrap");
Market highLowMarket = markets.Single(x => x.name == "GWHighLow");
Market oddEvenMarket = markets.Single(x => x.name == "GWOddEven");
Market trapMostMarket = markets.Single(x => x.name == "GWTrapMost");
Market trapMostAnyMarket = markets.Single(x => x.name == "GWTrapMostAny");

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
