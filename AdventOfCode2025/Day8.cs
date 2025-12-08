#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt", 1000));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName, long iterations = 10)
{
    var positions = ParseInput(fileName)
    .ToArray();
    var queue = new PriorityQueue<(Position, Position), long>();
    for (var i = 0; i < positions.Length; ++i)
    {
        for (var j = i + 1; j < positions.Length; ++j)
        {
            var dist = (positions[i].X - positions[j].X) * (positions[i].X - positions[j].X)
            + (positions[i].Y - positions[j].Y) * (positions[i].Y - positions[j].Y)
            + (positions[i].Z - positions[j].Z) * (positions[i].Z - positions[j].Z);
            queue.Enqueue((positions[i], positions[j]), dist);
        }
    }

    var circuits = new List<HashSet<Position>>();
    for (var i = 0; i < iterations; ++i)
    {
        var (p1, p2) = queue.Dequeue();
        var containingCircuits = circuits.Where(x => x.Contains(p1) || x.Contains(p2))
        .ToArray();
        switch (containingCircuits.Length)
        {
            case 0:
                {
                    circuits.Add([p1, p2]);
                    break;
                }
            case 1:
                {
                    var circuit = containingCircuits[0];
                    circuit.Add(p1);
                    circuit.Add(p2);
                    break;
                }
            case 2:
                {
                    circuits.Remove(containingCircuits[1]);
                    containingCircuits[0].UnionWith(containingCircuits[1]);
                    break;
                }
            default: throw new InvalidOperationException();
        }
    }

    return circuits.Select(x => x.Count)
    .OrderByDescending(x => x)
    .Take(3)
    .Aggregate(1L, (res, x) => res * x);
}

object SolveP2(string fileName)
{
    var positions = ParseInput(fileName)
    .ToArray();
    var queue = new PriorityQueue<(Position, Position), long>();
    for (var i = 0; i < positions.Length; ++i)
    {
        for (var j = i + 1; j < positions.Length; ++j)
        {
            var dist = (positions[i].X - positions[j].X) * (positions[i].X - positions[j].X)
            + (positions[i].Y - positions[j].Y) * (positions[i].Y - positions[j].Y)
            + (positions[i].Z - positions[j].Z) * (positions[i].Z - positions[j].Z);
            queue.Enqueue((positions[i], positions[j]), dist);
        }
    }

    var circuits = new List<HashSet<Position>>();
    var result = 0L;
    for (; circuits.Count != 1 || circuits[0].Count != positions.Length;)
    {
        var (p1, p2) = queue.Dequeue();
        result = p1.X * p2.X;
        var containingCircuits = circuits.Where(x => x.Contains(p1) || x.Contains(p2))
        .ToArray();
        switch (containingCircuits.Length)
        {
            case 0:
                {
                    circuits.Add([p1, p2]);
                    break;
                }
            case 1:
                {
                    var circuit = containingCircuits[0];
                    circuit.Add(p1);
                    circuit.Add(p2);
                    break;
                }
            case 2:
                {
                    circuits.Remove(containingCircuits[1]);
                    containingCircuits[0].UnionWith(containingCircuits[1]);
                    break;
                }
            default: throw new InvalidOperationException();
        }
    }

    return result;
}

IEnumerable<Position> ParseInput(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? throw new InvalidOperationException();
        var coords = line.Split(',').Select(long.Parse).ToArray();
        yield return new Position(coords[0], coords[1], coords[2]);
    }
}

record Position(long X, long Y, long Z);