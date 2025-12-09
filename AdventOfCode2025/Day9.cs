#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var input = ParseInput(fileName)
    .ToArray();
    var result = Enumerable.Range(0, input.Length)
    .SelectMany(i => Enumerable.Range(i + 1, input.Length - i - 1).Select(j => (input[i], input[j])))
    .Aggregate(0L, (res, pair) => Math.Max(res, (Math.Abs(pair.Item1.X - pair.Item2.X) + 1) * (Math.Abs(pair.Item1.Y - pair.Item2.Y) + 1)));
    return result;
}

object SolveP2(string fileName)
{
    var input = ParseInput(fileName)
    .ToArray();
    var result = Enumerable.Range(0, input.Length)
    .SelectMany(i => Enumerable.Range(i + 1, input.Length - i - 1).Select(j => (input[i], input[j])))
    .Where(pair =>
    {
        var isInsidePolygon = true;
        var (minX, maxX) = (Math.Min(pair.Item1.X, pair.Item2.X), Math.Max(pair.Item1.X, pair.Item2.X));
        var (minY, maxY) = (Math.Min(pair.Item1.Y, pair.Item2.Y), Math.Max(pair.Item1.Y, pair.Item2.Y));
        for (var i = 0; i < input.Length; ++i)
        {
            var (p1, p2) = (input[i % input.Length], input[(i + 1) % input.Length]);

            if (p1.X == p2.X && p1.X > minX && p1.X < maxX && Math.Min(p1.Y, p2.Y) < maxY && Math.Max(p1.Y, p2.Y) > minY)
            {
                isInsidePolygon = false;
                break;
            }

            if (p1.Y == p2.Y && p1.Y > minY && p1.Y < maxY && Math.Min(p1.X, p2.X) < maxX && Math.Max(p1.X, p2.X) > minX)
            {
                isInsidePolygon = false;
                break;
            }
        }
        return isInsidePolygon;
    })
    .Aggregate(0L, (res, pair) => Math.Max(res, (Math.Abs(pair.Item1.X - pair.Item2.X) + 1) * (Math.Abs(pair.Item1.Y - pair.Item2.Y) + 1)));

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
        yield return new Position(coords[0], coords[1]);
    }
}

record Position(long X, long Y);