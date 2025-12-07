#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var (splittersList, start, boundary) = ParseInput(fileName);
    var splitters = splittersList.ToHashSet();
    var beams = new HashSet<Position>([start]);
    var result = 0L;
    while (beams.Count != 0)
    {
        var newBeams = beams.Select(x => x with { Y = x.Y + 1 })
            .Where(x => x.Y < boundary.Y);

        var beamsToSplit = newBeams.Where(x => splitters.Contains(x))
        .ToArray();
        result += beamsToSplit.Length;
        var splitBeams = beamsToSplit.SelectMany(x => new[] {
            x with { X = x.X - 1 },
            x with { X = x.X + 1 }
        }).ToArray();
        beams = [.. newBeams.Except(beamsToSplit).Concat(splitBeams).Where(x => x.X >= 0 && x.X < boundary.X)];
    }

    return result;
}

object SolveP2(string fileName)
{
    var (splittersList, start, boundary) = ParseInput(fileName);
    var splitters = splittersList.ToHashSet();
    var resultsCache = new Dictionary<Position, long>();
    var result = CountTimelines(start, boundary, splitters, resultsCache);

    return result;
}

long CountTimelines(Position start, Position boundary, HashSet<Position> splitters, Dictionary<Position, long> resultsCache)
{
    if (resultsCache.TryGetValue(start, out long result))
    {
        return result;
    }

    if (start.Y >= boundary.Y || start.X < 0 || start.X >= boundary.X)
    {
        result = 1;
    }
    else
    {
        var splitter = splitters.Where(x => x.X == start.X && x.Y > start.Y)
        .MinBy(x => x.Y);

        if (splitter == null)
        {
            result = 1;
        }
        else
        {
            result = CountTimelines(start with { Y = splitter.Y, X = splitter.X - 1 }, boundary, splitters, resultsCache) +
            CountTimelines(splitter with { Y = splitter.Y, X = splitter.X + 1 }, boundary, splitters, resultsCache);
        }
    }

    resultsCache[start] = result;
    return result;
}

(IEnumerable<Position>, Position, Position) ParseInput(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var start = new Position(0, 0);
    var splitters = new List<Position>();
    for (var i = 0; i < lines.Length; ++i)
    {
        var line = lines[i];
        for (var j = 0; j < line.Length; ++j)
        {
            if (line[j] == 'S')
            {
                start = new Position(j, i);
            }
            else if (line[j] == '^')
            {
                splitters.Add(new Position(j, i));
            }
        }
    }

    return (splitters, start, new Position(lines[0].Length, lines.Length));
}

record Position(long X, long Y);