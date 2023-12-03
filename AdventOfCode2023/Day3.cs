namespace AdventOfCode2023;

internal class Day3
{
    public static object SolveP1(string fileName) => GetNumbersData(fileName)
        .Where(x => x.Probes.Any(y => !char.IsDigit(y.Char) && y.Char != '.'))
        .Sum(x => x.Number);

    public static object SolveP2(string fileName)
    {
        var numbersData = GetNumbersData(fileName)
            .ToArray();
        return numbersData.SelectMany(x => x.Probes)
            .Where(x => x.Char == '*')
            .DistinctBy(x => x.Coords)
            .Select(x =>
            {
                var data = numbersData
                    .Where(y => y.Probes.Any(z => z.Coords == x.Coords))
                    .ToArray();

                return data.Length == 2 ? data[0].Number * data[1].Number : 0;
            })
            .Sum();
    }

    private static IEnumerable<(long Number, IEnumerable<(char Char, (int X, int Y) Coords)> Probes)> GetNumbersData(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();
        var (maxX, maxY) = (input[0].Length - 1, input.Length - 1);
        var probesDeltas = new[] {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1), (0, 0), (0, 1),
            (1, -1), (1, 0), (1, 1)
        };

        for (var i = 0; i <= maxY; ++i)
        {
            for (var j = 0; j <= maxX; ++j)
            {
                if (!char.IsDigit(input[i][j]))
                {
                    continue;
                }

                var charsCoords = new List<(int X, int Y)>();
                for (; j <= maxX && char.IsDigit(input[i][j]); ++j)
                {
                    charsCoords.Add((j, i));
                }

                var number = long.Parse(input[i].AsSpan(charsCoords[0].X, charsCoords.Count));
                var probes = charsCoords.SelectMany(y => probesDeltas.Select(p => (X: y.X + p.Item1, Y: y.Y + p.Item2))
                    .Where(p => p.Item1 >= 0 && p.Item1 <= maxX && p.Item2 >= 0 && p.Item2 <= maxY))
                    .Distinct()
                    .Select(x => (input[x.Y][x.X], x))
                    .ToArray();

                yield return (number, probes);
            }
        }
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