using System.Text.Json;
using MathNet.Numerics.Random;
using HighlightGames.GreyhoundWinners.GameEngine;

if (args.Length != 1)
{
    Console.WriteLine("Usage: [<iterations>]");

    Environment.Exit(1);
}

int iterations = int.Parse(args[0]);

var random = new MersenneTwister();
var game = new Game();

int[] selectedTraps = game.CreateGame(random).ToArray();

var markets = game.GenerateMarkets();

var marketReturns = markets.ToDictionary(m => m.Name, m => m.SelectionFairPrices.Keys.ToDictionary(s => s, _ => 0m));

var playYourDogsRightSelections = new Dictionary<string, Direction[]>
{
    ["LHLHLH"] = new[] { Direction.Lower, Direction.Higher, Direction.Lower, Direction.Higher, Direction.Lower, Direction.Higher },
    ["LLHHLL"] = new[] { Direction.Lower, Direction.Lower, Direction.Higher, Direction.Higher, Direction.Lower, Direction.Lower },
    ["LLLHHH"] = new[] { Direction.Lower, Direction.Lower, Direction.Lower, Direction.Higher, Direction.Higher, Direction.Higher },
    ["LLLLHH"] = new[] { Direction.Lower, Direction.Lower, Direction.Lower, Direction.Lower, Direction.Higher, Direction.Higher },
    ["LLLLLH"] = new[] { Direction.Lower, Direction.Lower, Direction.Lower, Direction.Lower, Direction.Lower, Direction.Higher },
    ["LLLLLL"] = new[] { Direction.Lower, Direction.Lower, Direction.Lower, Direction.Lower, Direction.Lower, Direction.Lower }
};

foreach (var selection in playYourDogsRightSelections.Keys)
{
    marketReturns["GWPlayYourDogsRight"][selection] = 0m;
}

var settler = new Settler();

for (var iteration = 0; iteration < iterations; iteration++)
{
    var result = game.CreateGame(random).ToArray();

    foreach (var settlement in settler.SettleSuperMatchWinMarket(selectedTraps, result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }

    foreach (var settlement in settler.SettleSuperMatchWithABreakMarket(selectedTraps, result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }

    foreach (var settlement in settler.SettleCatchAMatchMarket(result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }

    foreach (var settlement in settler.SettleTrapNumbersWinningMostHighLowMarket(result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }

    foreach (var settlement in settler.SettleTrapNumbersWinningMostOddEvenMarket(result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }

    foreach (var settlement in settler.SettleTrapNumbersWinningMostTopTrapMarket(result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }
    
    foreach (var settlement in settler.SettleWinningTrapsTotalsTrapsTotalMarket(result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }

    foreach (var settlement in settler.SettleWinningTrapsTotalsRangeMarket(result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }

    foreach (var settlement in settler.SettleWinningTrapsTotalsSumMarket(result))
    {
        marketReturns[settlement.Market][settlement.Selection] += settlement.Dividends * settlement.UnitAmount;
    }
    
    foreach (var selection in playYourDogsRightSelections)
    {
        foreach (var settlement in settler.SettlePlayYourDogsRightMarket(selection.Value, result))
        {
            marketReturns[settlement.Market][selection.Key] += settlement.Dividends * settlement.UnitAmount;
        }
    }
}

foreach (var market in marketReturns.Keys)
{
    foreach (var selection in marketReturns[market].Keys)
    {
        marketReturns[market][selection] /= iterations;
    }
}

Console.WriteLine(JsonSerializer.Serialize(marketReturns, new JsonSerializerOptions
{
    WriteIndented = true
}));