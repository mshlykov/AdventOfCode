namespace AdventOfCode2024;

internal class Day20
{
    public static object SolveP1(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();
        var (start, end, obstacles) = GetFieldData(field);
        var path = GetPath(start, end, obstacles);
        var minDist = fileName == "1.txt" ? 20L : 100L;
        return GetCheats(path, obstacles, 2L).LongCount(x => x.End - x.Start - Distance(path[x.Start], path[x.End]) >= minDist);
    }

    public static object SolveP2(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();
        var (start, end, obstacles) = GetFieldData(field);
        var path = GetPath(start, end, obstacles);
        var minDist = fileName == "2.txt" ? 72L : 100L;
        return GetCheats(path, obstacles, 20L).LongCount(x => x.End - x.Start - Distance(path[x.Start], path[x.End]) >= minDist);
    }

    private static IEnumerable<(int Start, int End)> GetCheats(List<(int I, int J)> path, HashSet<(int I, int J)> obstacles, long maxCheatLength)
    {
        var (maxI, maxJ) = (obstacles.Concat(path).Max(x => x.I), obstacles.Concat(path).Max(x => x.J));
        foreach (var (startItem, start) in path.Select((x, i) => (x, i)))
        {
            var cheatEnds = new HashSet<(int I, int J)>();
            for (var p = 0; p <= maxCheatLength; ++p)
            {
                for (var t = 0; t <= maxCheatLength - p; ++t)
                {
                    var pointsToAdd = new[] {
                        (startItem.I + p, startItem.J + t),
                        (startItem.I + p, startItem.J - t),
                        (startItem.I - p, startItem.J + t),
                        (startItem.I - p, startItem.J - t),
                    }
                    .Where(x => IsInField(x, maxI, maxJ) && !obstacles.Contains(x));
                    cheatEnds.UnionWith(pointsToAdd);
                }
            }

            var reducedCheatEnds = cheatEnds
                .Select(x => path.IndexOf(x))
                .Where(x => x > start);
            foreach (var end in reducedCheatEnds)
            {
                yield return (start, end);
            }
        }
    }

    private static long Distance((int I, int J) x, (int I, int J) y) => Math.Abs(x.I - y.I) + Math.Abs(x.J - y.J);

    private static bool IsInField((int I, int J) p, int maxI, int maxJ) => p.I >= 0 && p.I <= maxI && p.J >= 0 && p.J <= maxJ;

    private static IEnumerable<(int I, int J)> GetNeighboursRaw((int I, int J) p) => new[] {
        (I: p.I + 1, J: p.J),
        (I: p.I - 1, J: p.J),
        (I: p.I, J: p.J + 1),
        (I: p.I, J: p.J - 1),
    };

    private static List<(int I, int J)> GetPath((int I, int J) start, (int I, int J) end, HashSet<(int I, int J)> obstacles)
    {
        var path = new List<(int I, int J)>();
        for (var v = start; v != (-1, -1);)
        {
            path.Add(v);
            v = GetNeighboursRaw(v)
                .FirstOrDefault(x => !path.Contains(x) && !obstacles.Contains(x), (-1, -1));
        }

        return path;
    }

    private static ((int I, int J) Start, (int I, int J) End, HashSet<(int I, int J)> Obstacles) GetFieldData(string[] field)
    {
        var (start, end, obstacles) = ((I: -1, J: -1), (I: -1, J: -1), new HashSet<(int I, int J)>());
        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                switch (field[i][j])
                {
                    case 'S':
                        start = (i, j);
                        break;
                    case 'E':
                        end = (i, j);
                        break;
                    case '#':
                        obstacles.Add((i, j));
                        break;
                }
            }
        }

        return (start, end, obstacles);
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
