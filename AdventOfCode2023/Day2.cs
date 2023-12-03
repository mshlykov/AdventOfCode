namespace AdventOfCode2023;

internal class Day2
{
    public static object SolveP1(string fileName)
    {
        var cons = new Dictionary<string, long>()
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };
        return GetInput(fileName)
            .Where(x => !x.Sets.Any(y => y.Keys.Any(z => y[z] > cons[z])))
            .Sum(x => x.Id);
    }

    public static object SolveP2(string fileName)
    {
        return GetInput(fileName)
            .Select(x => {
                var cons = new Dictionary<string, long>()
                {
                    { "red", 0 },
                    { "green", 0 },
                    { "blue", 0 }
                };

                foreach(var set in x.Sets)
                {
                    foreach(var key in set.Keys)
                    {
                        cons[key] = Math.Max(cons[key], set[key]);
                    }
                }

                return cons["red"] * cons["green"] * cons["blue"];
            })
            .Sum();
    }

    private static IEnumerable<(long Id, IEnumerable<Dictionary<string, long>> Sets)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var split = line.Split(": ");
            var id = long.Parse(split[0].Substring(5));
            var sets = split[1].Split("; ").Select(x =>
            {
                var colData = x.Split(", ")
                .Select(y => {
                    var col = y.Split(" ").ToArray();
                    return (col[1], long.Parse(col[0]));
                }).ToDictionary(x => x.Item1, x => x.Item2);
                return colData;
            });
            yield return (id, sets);
        }
    }
}
