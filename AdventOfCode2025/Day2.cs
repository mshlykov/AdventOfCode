#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var ranges = ParseInput(fileName)
        .ToArray();
    var numbers = new List<long>();
    foreach (var range in ranges)
    {
        var strStart = range.Start.ToString();
        var num = strStart.Length % 2 == 1 ? Math.Pow(10, strStart.Length / 2) : Math.Pow(10, strStart.Length / 2 - 1);
        var numStr = num.ToString();
        var largeNum = long.Parse(numStr + numStr);
        while (largeNum <= range.End)
        {
            if (largeNum >= range.Start)
            {
                numbers.Add(largeNum);
            }

            ++num;
            numStr = num.ToString();
            largeNum = long.Parse(numStr + numStr);
        }
        ;
    }

    return numbers.Sum();
}

object SolveP2(string fileName)
{
    return ParseInput(fileName)
        .SelectMany(r => Enumerable.Sequence(r.Start, r.End, 1L)
            .Where(x =>
            {
                var strNum = x.ToString();
                return strNum.Length > 1 && Enumerable.Sequence(1, strNum.Length / 2, 1)
                    .Any(y => strNum.Chunk(y)
                        .Select(x => new string(x))
                        .Distinct()
                        .Count() == 1);
            }))
        .Sum();
}

IEnumerable<Range> ParseInput(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);
    var input = sr.ReadLine() ?? throw new InvalidOperationException();
    return input.Split(',')
        .Select(part =>
        {
            var rangeData = part.Split('-', StringSplitOptions.RemoveEmptyEntries);
            return new Range(long.Parse(rangeData[0]), long.Parse(rangeData[1]));
        });
}

record Range(long Start, long End);