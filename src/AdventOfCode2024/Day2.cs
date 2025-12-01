namespace AdventOfCode2024;

internal class Day2
{
    public static object SolveP1(string fileName) => GetInput(fileName)
        .Count(IsSafe);

    public static object SolveP2(string fileName) => GetInput(fileName)
        .Count(data => IsSafe(data) || Enumerable.Range(0, data.Length).Any(y => IsSafe(data.Take(y).Concat(data.Skip(y + 1)).ToArray())));

    private static bool IsSafe(long[] data)
    {
        var reportSign = Math.Sign(data[1] - data[0]);
        if (reportSign == 0 || Math.Abs(data[1] - data[0]) > 3)
        {
            return false;
        }

        return !Enumerable.Range(2, data.Length - 2).Any(x => Math.Sign(data[x] - data[x - 1]) != reportSign || Math.Abs(data[x] - data[x - 1]) > 3);
    }

    private static IEnumerable<long[]> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return sr.ReadLine()
                .Split(" ")
                .Select(long.Parse)
                .ToArray();
        }
    }
}
