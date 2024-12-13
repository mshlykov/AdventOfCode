namespace AdventOfCode2024;

internal class Day12
{
    public static object SolveP1(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();

        var regions = new List<HashSet<(int, int)>>();
        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                if (regions.Any(x => x.Contains((i, j))))
                {
                    continue;
                }

                var region = GetRegion(field, (i, j));
                regions.Add(region);
            }
        }

        return regions.Sum(x => x.Count * GetPerimeter(x));
    }

    public static object SolveP2(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();

        var regions = new List<HashSet<(int, int)>>();
        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                if (regions.Any(x => x.Contains((i, j))))
                {
                    continue;
                }

                var region = GetRegion(field, (i, j));
                regions.Add(region);
            }
        }

        return regions.Sum(x => x.Count * GetSidesNumber(x));
    }

    private static long GetPerimeter(HashSet<(int, int)> region)
    {
        return region.Sum(x => 4 - GetNeihgbours(x).Count(y => region.Contains(y)));
    }

    private static HashSet<(int, int)> GetRegion(string[] field, (int, int) start)
    {
        var (i, j) = start;
        var regionChar = field[i][j];
        var queue = new Queue<(int, int)>([start]);
        var visited = new HashSet<(int, int)>([start]);
        while (queue.Count != 0)
        {
            var currPoint = queue.Dequeue();

            var neighbours = GetNeihgbours(currPoint)
                .Where(x => x.Item1 >= 0 && x.Item1 < field.Length && x.Item2 >= 0 && x.Item2 < field[x.Item1].Length
                && !visited.Contains(x)
                && field[x.Item1][x.Item2] == regionChar);
            foreach (var neighbour in neighbours)
            {
                visited.Add(neighbour);
                queue.Enqueue(neighbour);
            }
        }

        return visited;
    }

    // counting vertices rather than edges
    private static long GetSidesNumber(HashSet<(int, int)> region)
    {
        var adjacentPoints = region.SelectMany(x => GetNeihgbours(x))
            .Where(x => !region.Contains(x))
            .ToHashSet();

        var innerVerticesSum = adjacentPoints.Select(x =>
        {
            var (i, j) = x;
            return new[]
            {
                new[] { (i - 1, j), (i - 1, j + 1), (i, j + 1) },
                [(i + 1, j), (i + 1, j - 1), (i, j - 1)],
                [(i, j + 1), (i + 1, j + 1), (i + 1, j)],
                [(i, j - 1), (i - 1, j - 1), (i - 1, j)],
            };
        })
            .Sum(x => x.Count(y => region.IsSupersetOf(y)));
        var outerVerticesSum = region.Select(x =>
        {
            var (i, j) = x;
            return new[]
            {
                new[] { (i - 1, j), (i, j + 1) },
                [(i + 1, j), (i, j - 1)],
                [(i, j + 1), (i + 1, j)],
                [(i, j - 1), (i - 1, j)],
            };
        })
            .Sum(x => x.Count(y => adjacentPoints.IsSupersetOf(y)));

        return innerVerticesSum + outerVerticesSum;
    }

    private static IEnumerable<(int, int)> GetNeihgbours((int, int) point)
    {
        var (i, j) = point;

        return
        [
            (i - 1, j),
            (i + 1, j),
            (i, j - 1),
            (i, j + 1)
        ];
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
