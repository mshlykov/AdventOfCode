namespace AdventOfCode2023;

internal class Day1
{
    public static long SolveP1(string fileName)
    {
        return GetInput(fileName)
            .Select(x =>
            {
                var digits = x.Where(y => char.IsDigit(y))
                .Select(y => y - '0')
                .ToArray();
                return digits[0] * 10 + digits[^1];
            })
            .Sum(x => (long)x);
    }

    public static long SolveP2(string fileName)
    {
        var dict = new Dictionary<string, int>() {
            { "zero", 0 },
            { "one", 1 }, 
            { "two", 2 }, 
            { "three", 3 }, 
            { "four", 4 }, 
            { "five", 5 }, 
            { "six", 6 }, 
            { "seven", 7 },
            { "eight", 8 },
            { "nine", 9 },
        };
        return GetInput(fileName)
            .Select(x =>
            {
                var digits = x.Select((y,i) => 
                {
                    var key = dict.Keys.FirstOrDefault(k => x.Substring(i).StartsWith(k));
                    if (key != null) {
                        return dict[key];
                    }
                    else if (char.IsDigit(y)) {
                        return y - '0';
                    }
                    else return default(int?);
                })
                .Where(x => x.HasValue)
                .ToArray();
                return digits[0]!.Value * 10 + digits[^1]!.Value;
            })
            .Sum(x => (long)x);
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
