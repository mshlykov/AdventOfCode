namespace AdventOfCode2024;

internal class Day14
{
    public static object SolveP1(string fileName)
    {
        var data = GetInput(fileName)
            .ToArray();
        var (numX, numY) = fileName == "1.txt" ? (11L, 7L) : (101L, 103L);
        var (maxX, maxY) = (numX - 1, numY - 1);
        var numSeconds = 100;
        var normalizedData = data.Select(x => (X: NormalizeCoord(x.P.X + numSeconds * x.V.X, maxX), Y: NormalizeCoord(x.P.Y + numSeconds * x.V.Y, maxY)))
            .ToArray();

        var (firsHalfX, firstHalfY) = (numX % 2 == 1 ? numX / 2 - 1 : numX / 2, numY % 2 == 1 ? numY / 2 - 1 : numY);
        var (secondHalfX, secondHalfY) = (numX % 2 == 1 ? firsHalfX + 2 : firsHalfX + 1, numY % 2 == 1 ? firstHalfY + 2 : firstHalfY + 1);
        var quadrants = new ((long Min, long Max) X, (long Min, long Max) Y)[]
        {
            ((0, firsHalfX), (0, firstHalfY)),
            ((secondHalfX, maxX), (0, firstHalfY)),
            ((0, firsHalfX), (secondHalfY, maxY)),
            ((secondHalfX, maxX), (secondHalfY, maxY))
        };
        return quadrants.Select(x => normalizedData.LongCount(p => p.X >= x.X.Min && p.X <= x.X.Max && p.Y >= x.Y.Min && p.Y <= x.Y.Max))
            .Aggregate(1L, (res, x) => res * x);
    }

    public static object SolveP2(string fileName)
    {
        var data = GetInput(fileName)
            .ToArray();
        var (numX, numY) = (101L, 103L);
        var (maxX, maxY) = (numX - 1, numY - 1);

        for (var i = 0L; true; ++i)
        {
            var normalizedData = data.Select(x => (X: NormalizeCoord(x.P.X + i * x.V.X, maxX), Y: NormalizeCoord(x.P.Y + i * x.V.Y, maxY)))
                .ToHashSet();
            var countWithNeighbours = normalizedData.Select(p => new (long X, long Y)[]
            {
                (p.X + 1, p.Y),
                (p.X - 1, p.Y),
                (p.X, p.Y + 1),
                (p.X, p.Y - 1),
            })
                .Count(x => x.Any(y => normalizedData.Contains(y)));

            // hint from Reddit: check if more than 70% of points have neighbours, not an exact match
            if (countWithNeighbours >= 0.7 * normalizedData.Count)
            {
                for (var x = 0; x <= maxX; ++x)
                {
                    for (var y = 0; y <= maxY; ++y)
                    {
                        Console.Write(normalizedData.Contains((x, y)) ? "*" : ".");
                    }
                    Console.Write(Environment.NewLine);
                }
                Console.WriteLine(i);
                break;
            }
        }

        return 0L;
    }

    private static long NormalizeCoord(long val, long maxVal)
    {
        var numOfVals = maxVal + 1;
        switch (val)
        {
            case var val1 when val1 > maxVal:
                val %= numOfVals;
                break;
            case var val1 when val1 < 0:
                val %= numOfVals;
                val += val < 0 ? numOfVals : 0;
                break;
        }

        return val;
    }

    private static IEnumerable<((long X, long Y) P, (long X, long Y) V)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var data = line.Split(" ")
                .Select(x => x[2..].Split(",").Select(long.Parse).ToArray())
                .ToArray();
            yield return ((data[0][0], data[0][1]), (data[1][0], data[1][1]));
        }
    }
}
