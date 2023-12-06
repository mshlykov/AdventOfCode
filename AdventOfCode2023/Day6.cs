namespace AdventOfCode2023;

internal class Day6
{
    public static object SolveP1(string fileName)
    {
        var (times, records) = GetInput(fileName);
        var res = 1L;
        for (var i = 0; i < times.Length; ++i)
        {
            var acc = 0L;
            var accidx = new List<long>();
            for (var j = 0; j <= times[i]; ++j)
            {
                if (j * (times[i] - j) > records[i])
                {
                    acc++;
                    accidx.Add(j);
                }
            }
            res *= acc;
        }
        return res;
    }

    public static object SolveP2(string fileName)
    {
        var (time, record) = GetInput2(fileName);
        var acc = 0L;
        for (var j = 0; j <= time; ++j)
        {
            if (j * (time - j) > record)
            {
                acc++;
            }
        }

        return acc;
    }

    private static (long Time, long Record) GetInput2(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        var times1 = sr.ReadLine().Split(": ")[^1]
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var time = long.Parse(string.Join("", times1));

        var record1 = sr.ReadLine().Split(": ")[^1]
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var record = long.Parse(string.Join("", record1));

        return (time, record);
    }

    private static (long[] Times, long[] Records) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        var times = sr.ReadLine().Split(": ")[^1]
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => long.Parse(x))
            .ToArray();

        var records = sr.ReadLine().Split(": ")[^1]
            .Trim()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => long.Parse(x))
            .ToArray();

        return (times, records);
    }
}
