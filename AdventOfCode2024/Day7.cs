namespace AdventOfCode2024;

internal class Day7
{
    public static object SolveP1(string fileName) => GetInput(fileName)
        .Where(x => MatchExists(x.Item1, x.Item2))
        .Sum(x => x.Item1);

    private static bool MatchExists(long expectedResult, long[] numbers)
    {
        var numOfIters = 1 << (numbers.Length - 1);

        for (var i = 0; i < numOfIters; ++i)
        {
            var div = i;
            var currResult = numbers[0];
            for (var j = 1; j < numbers.Length; ++j, div >>= 1)
            {
                var mod = div % 2;
                currResult = mod == 1 ? currResult * numbers[j] : currResult + numbers[j];
                if (currResult > expectedResult)
                {
                    break;
                }
            }

            if (currResult == expectedResult)
            {
                return true;
            }
        }

        return false;
    }

    public static object SolveP2(string fileName) => GetInput(fileName)
        .Where(x => MatchExists2(x.Item1, x.Item2))
        .Sum(x => x.Item1);

    private static bool MatchExists2(long expectedResult, long[] numbers)
    {
        var numOfIters = (long)Math.Pow(3, numbers.Length - 1);

        for (var i = 0; i < numOfIters; ++i)
        {
            var div = i;
            var currResult = numbers[0];
            for (var j = 1; j < numbers.Length; ++j, div /= 3)
            {
                var mod = div % 3;
                currResult = mod switch
                {
                    0 => currResult + numbers[j],
                    1 => currResult * numbers[j],
                    2 => long.Parse(currResult.ToString() + numbers[j].ToString()),
                    _ => throw new NotImplementedException()
                };

                if (currResult > expectedResult)
                {
                    break;
                }
            }

            if (currResult == expectedResult)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<(long, long[])> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var split1 = line.Split(": ");
            var split2 = split1[1].Split(" ");
            yield return (long.Parse(split1[0]), split2.Select(long.Parse).ToArray());
        }
    }
}
