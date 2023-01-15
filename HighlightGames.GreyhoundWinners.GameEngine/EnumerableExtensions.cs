namespace HighlightGames.GreyhoundWinners.GameEngine;

public static class EnumerableExtensions
{
    public static IEnumerable<List<T>> Combinations<T>(this IEnumerable<T> sequence, int r)
    {
        sequence = sequence.ToList();

        return (r == 0
            ? new[] { Array.Empty<T>() }
            : sequence.SelectMany((item, index) =>
                sequence.Skip(index + 1).Combinations(r - 1).Select(c => (new[] { item }).Concat(c)))).Select(x => x.ToList());
    }
}