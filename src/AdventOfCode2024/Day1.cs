namespace AdventOfCode2024;

internal class Day1
{
    public static object SolveP1(string fileName) {
        var input = GetInput(fileName)
            .Select(x => x.Split("   "))
            .Select(x => (long.Parse(x[0]), long.Parse(x[1])))
            .ToArray();

        return input.Select(x => x.Item1)
            .OrderBy(x => x)
            .Zip(input.Select(x => x.Item2).OrderBy(x => x), (x, y) => Math.Abs(x - y))
            .Sum();
    }

    public static object SolveP2(string fileName) 
    {
        var input = GetInput(fileName)
            .Select(x => x.Split("   "))
            .Select(x => (long.Parse(x[0]), long.Parse(x[1])))
            .ToArray();

        var dict = input.Select(x => x.Item2)
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.LongCount());

        return input.Sum(x => x.Item1 * (dict.ContainsKey(x.Item1) ? dict[x.Item1] : 0L));
    }

    private static IEnumerable<string> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return sr.ReadLine();
        }
    }
}
