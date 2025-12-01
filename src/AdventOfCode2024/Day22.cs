namespace AdventOfCode2024;

internal class Day22
{
    private const int IterationsNumber = 2000;

    public static object SolveP1(string fileName) => GetInput(fileName)
        .Select(secret =>
        {
            for (var j = 0; j < IterationsNumber; ++j)
            {
                secret = GetNextSecret(secret);
            }
            return secret;
        })
        .Sum();

    // 1100 ms with hashset + parallelized brute force
    public static object SolveP2(string fileName) => GetInput(fileName)
        .ToArray()
        .AsParallel()
        .SelectMany(GetPricePairs)
        .AggregateBy(x => x.Sequence, 0L, (result, item) => result + item.Price)
        .Max(x => x.Value);

    private static IEnumerable<((long, long, long, long) Sequence, long Price)> GetPricePairs(long secret)
    {
        var pricesList = new long[IterationsNumber + 1];
        pricesList[0] = secret % 10;
        for (var j = 0; j < pricesList.Length - 1; ++j)
        {
            secret = GetNextSecret(secret);
            pricesList[j + 1] = secret % 10;
        }

        var dict = new HashSet<(long, long, long, long)>();
        for (var j = 4; j < pricesList.Length; ++j)
        {
            var sequence = (pricesList[j - 3] - pricesList[j - 4],
            pricesList[j - 2] - pricesList[j - 3],
            pricesList[j - 1] - pricesList[j - 2],
            pricesList[j] - pricesList[j - 1]);
            if (dict.Add(sequence))
            {
                yield return (sequence, pricesList[j]);
            }
        };
    }

    private static long GetNextSecret(long secret)
    {
        secret ^= secret << 6;
        secret &= 0x0000000000FFFFFFL;
        secret ^= secret >> 5;
        secret &= 0x0000000000FFFFFFL;
        secret ^= secret << 11;
        secret &= 0x0000000000FFFFFFL;
        return secret;
    }

    private static IEnumerable<long> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return long.Parse(sr.ReadLine());
        }
    }
}
