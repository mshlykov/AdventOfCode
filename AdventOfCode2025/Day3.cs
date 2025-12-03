#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var numbers = ParseInput(fileName)
        .Select(x =>
        {
            var (max, index) = MaxWithIndex(x);

            if (index == x.Length - 1)
            {
                var (max2, _) = MaxWithIndex(x[..^1]);
                return max2 * 10 + max;
            }

            var (max3, _) = MaxWithIndex(x[(index + 1)..]);

            return max * 10 + max3;
        })
        .ToArray();

    return numbers.Sum();
}

object SolveP2(string fileName)
{
    var numbers = ParseInput(fileName)
        .Select(x =>
        {
            var maxArr = x.Skip(x.Length - 12).ToArray();
            var currentMax = long.Parse(string.Concat(maxArr.Select(x => x.ToString())));

            for (var i = x.Length - 13; i >= 0; --i)
            {
                var currentMaxArr = maxArr;
                for (var j = 0; j < 12; ++j)
                {
                    long[] newMaxArr = [x[i], .. currentMaxArr[..j], .. currentMaxArr[(j + 1)..]];
                    var newMax = long.Parse(string.Concat(newMaxArr.Select(x => x.ToString())));
                    if (newMax > currentMax)
                    {
                        maxArr = newMaxArr;
                        currentMax = newMax;
                    }
                }
            }

            return currentMax;
        })
        .ToArray();

    return numbers.Sum();
}

(long, int) MaxWithIndex(IEnumerable<long> numbers) => numbers.Index().Aggregate((numbers.First(), 0), (result, x) => result.Item1 < x.Item ? (x.Item, x.Index) : result);

IEnumerable<long[]> ParseInput(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);

    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? throw new InvalidOperationException();
        yield return line.Select(x => (long)(x - '0'))
            .ToArray();
    }
}