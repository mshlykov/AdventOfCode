namespace AdventOfCode2024;

internal class Day25
{
    public static object SolveP1(string fileName)
    {
        var locksAndKeys = GetInput(fileName)
            .Select(x =>
            {
                var type = x[^1].All(y => y == '#') ? "KEY" : "LOCK";
                var itemToCount = type == "KEY" ? '#' : '.';
                var measurements = new int[5];
                foreach(var (index, item) in x[^1].Index())
                {
                    for (measurements[index] = 1; measurements[index] < x.Count + 1 && x[^measurements[index]][index] == itemToCount; ++measurements[index])
                    {
                    }

                    measurements[index] -= 2;
                }
                return (Type: type, Measurements: measurements);
            })
            .ToArray();

        return locksAndKeys
            .Where(x => x.Type == "LOCK")
            .Sum(@lock => locksAndKeys.Count(key => key.Type == "KEY" 
            && @lock.Measurements.Zip(key.Measurements).All(z => z.First >= z.Second)));
    }

    private static IEnumerable<List<string>> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var list = new List<string>(7);
            for (var line = sr.ReadLine(); !string.IsNullOrEmpty(line); line = sr.ReadLine())
            {
                list.Add(line);
            }

            yield return list;
        }
    }
}
