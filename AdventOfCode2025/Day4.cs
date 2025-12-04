#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var positions = ParseInput(fileName)
        .ToHashSet();
    return GetAccessiblePositions(positions)
        .Count();
}

IEnumerable<Position> GetAccessiblePositions(HashSet<Position> positions) => positions.Where(x => GetAdjacentPositions(x)
    .Count(y => positions.Contains(y)) < 4);

IEnumerable<Position> GetAdjacentPositions(Position pos)
{
    yield return new Position(pos.X - 1, pos.Y - 1);
    yield return new Position(pos.X - 1, pos.Y);
    yield return new Position(pos.X - 1, pos.Y + 1);
    yield return new Position(pos.X + 1, pos.Y - 1);
    yield return new Position(pos.X + 1, pos.Y);
    yield return new Position(pos.X + 1, pos.Y + 1);
    yield return new Position(pos.X, pos.Y - 1);
    yield return new Position(pos.X, pos.Y + 1);
}

object SolveP2(string fileName)
{
    var positions = ParseInput(fileName)
        .ToHashSet();
    var result = 0;
    var positionsToRemove = GetAccessiblePositions(positions)
        .ToArray();
    while (positionsToRemove.Length > 0)
    {
        result += positionsToRemove.Length;
        positions.ExceptWith(positionsToRemove);
        positionsToRemove = GetAccessiblePositions(positions)
            .ToArray();
    }

    return result;
}

IEnumerable<Position> ParseInput(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);
    var i = 0;
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? throw new InvalidOperationException();
        var j = 0;
        foreach (var c in line)
        {
            if (c == '@')
            {
                yield return new Position(i, j);
            }
            ++j;
        }
        ++i;
    }
}

record Position(int X, int Y);