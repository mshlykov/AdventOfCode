#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var (ranges, ids) = ParseInput(fileName);

    return ids.Count(id => ranges.Any(r => id >= r.Start && id <= r.End));
}

object SolveP2(string fileName)
{
    var ranges = ParseInput(fileName).Item1.ToList();
    var rangeGroups = new List<List<Range>>();
    foreach (var range in ranges)
    {
        var groups = rangeGroups.Where(g => g.Any(r => HaveIntersection(r, range)))
            .ToArray();

        var newGroup = groups.Length switch
        {
            0 => [],
            1 => groups[0],
            _ => [.. groups.SelectMany(g => g)],
        };

        rangeGroups.RemoveAll(g => groups.Contains(g));
        newGroup.Add(range);
        rangeGroups.Add(newGroup);
    }

    return rangeGroups.Sum(g => g.Max(r => r.End) - g.Min(r => r.Start) + 1);
}

bool HaveIntersection(Range r1, Range r2)
{
    return r1.Start <= r2.End && r1.Start >= r2.Start
        || r2.Start <= r1.End && r2.Start >= r1.Start;
}

(IEnumerable<Range>, IEnumerable<long>) ParseInput(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);

    var ranges = new List<Range>();
    for (var line = sr.ReadLine(); !string.IsNullOrEmpty(line); line = sr.ReadLine())
    {
        var bounds = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
        ranges.Add(new Range(long.Parse(bounds[0]), long.Parse(bounds[1])));
    }

    var ids = new List<long>();
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? throw new InvalidOperationException();
        ids.Add(long.Parse(line));
    }

    return (ranges, ids);
}

record Range(long Start, long End);