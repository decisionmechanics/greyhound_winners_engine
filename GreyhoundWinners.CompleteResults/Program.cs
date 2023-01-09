#define WRITE_RESULTS

using HighlightGames.GreyhoundWinners.GameEngine;

namespace GreyhoundWinners.CompleteResults
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if WRITE_RESULTS
            string outputFile = @"C:\ABetA\GameEngineGw\complete-results.csv";
            Directory.CreateDirectory(Path.GetDirectoryName(outputFile));

            FileStream stream = File.OpenWrite(outputFile);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine("result,highLow,oddEven,match123456_3,match333333_2,sameTrap14,sameTrap23,sameTrap36,trapMost,trapMostAny,trapTotalExact,traptotaloddeven,traptotalPrime,traptotalRange");
#endif
            int lineCount = 0;
            for (int one = 1; one <= 6; one++)
            {
                for (int two = 1; two <= 6; two++)
                {
                    for (int three = 1; three <= 6; three++)
                    {
                        for (int four = 1; four <= 6; four++)
                        {
                            for (int five = 1; five <= 6; five++)
                            {
                                for (int six = 1; six <= 6; six++)
                                {
                                    var result = new[] { one, two, three, four, five, six };
                                    string line = Settle(result);
                                    Console.Write($"{lineCount} {line} \r    ");
#if WRITE_RESULTS
                                    writer.WriteLine(line);
#endif
                                    //lineCount++;
                                    //if (lineCount > 100)
                                    //    return;
                                }
                            }
                        }
                    }
                }
            }
#if WRITE_RESULTS
            writer.Flush();
#endif
        }

        static string Settle(int[] result)
        {
            string highLow = Settler.SettleHighLowMarket(result);
            string oddEven = Settler.SettleOddEvenMarket(result);
            int match123456_3 = Settler.SettleMatchMarket(new int[] { 1, 2, 3, 4, 5, 6 }, 3, result);
            int match333333_2 = Settler.SettleMatchMarket(new int[] { 3, 3, 3, 3, 3, 3 }, 2, result);
            int sameTrap14 = Settler.SettleSameTrapMarket(1, 4, result);
            int sameTrap23 = Settler.SettleSameTrapMarket(2, 3, result);
            int sameTrap36 = Settler.SettleSameTrapMarket(3, 6, result);
            int? trapMost = Settler.SettleTrapMostMarket(result);
            string trapMostAny = Settler.SettleTrapMostAnyMarket(result);
            string trapTotalExact = Settler.SettleTrapTotalExactMarket(result);
            string traptotaloddeven = Settler.SettleTrapTotalOddEvenMarket(result);
            string traptotalPrime = Settler.SettleTrapTotalPrimeMarket(result);
            string traptotalRange = Settler.SettleTrapTotalRangeMarket(result);


            return $"{string.Join("-", result.Select(x => x.ToString()))},{highLow},{oddEven},{match123456_3},{match333333_2},{sameTrap14},{sameTrap23},{sameTrap36},{trapMost},{trapMostAny},{trapTotalExact},{traptotaloddeven},{traptotalPrime},{traptotalRange}";
        }
    }
}