using System.Text.Encodings.Web;
using System.Text.Json;

using HighlightGames.GreyhoundWinners.GameEngine;

var game = new Game();

IEnumerable<Market> markets = game.GenerateMarkets();

string json = JsonSerializer.Serialize(markets, new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = true
});

Console.WriteLine(json);