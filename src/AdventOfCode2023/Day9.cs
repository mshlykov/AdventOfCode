namespace AdventOfCode2023;

internal class Day9
{
    public static object SolveP1(string fileName)
    {
        return GetInput(fileName)
            .Select(l =>
            {
                var list = l;
                var lastEls = new List<long>();
                while (!list.All(x => x == 0))
                {
                    lastEls.Add(list[^1]);
                    list = list
                    .Skip(1)
                    .Select((x, i) => x - list[i])
                    .ToList();
                }
                return lastEls.Sum();
            })
            .Sum();
    }

    public static object SolveP2(string fileName)
    {
        return GetInput(fileName)
            .Select(l =>
            {
                var list = l;
                var lastEls = new List<long>();
                while (!list.All(x => x == 0))
                {
                    lastEls.Add(list[0]);
                    list = list
                    .Skip(1)
                    .Select((x, i) => x - list[i])
                    .ToList();
                }

                for (var i = lastEls.Count - 2; i >= 0 ; --i)
                {
                    lastEls[i] = lastEls[i] - lastEls[i + 1];
                }

                return lastEls[0];
            })
            .Sum();
    }

    private static IEnumerable<List<long>> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return sr.ReadLine()
                .Split(" ")
                .Select(x => long.Parse(x))
                .ToList();
        }
    }
}
