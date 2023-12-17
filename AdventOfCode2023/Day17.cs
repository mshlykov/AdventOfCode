namespace AdventOfCode2023;

internal class Day17
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();
        var starts = new[] {
            (0, 0, Dir.R, 0),
            (0, 0, Dir.B, 0),
        };

        return starts.Select(x =>
        {
            var dists = Dijkstra(x, 
                x => GetNeighbours(x)
                .Where(x => IsInField((x.i, x.j), input))
                .ToArray(),
                (x, y) => GetDist((x.i, x.j), (y.i, y.j), input));
            return dists.Where(x => x.Key.i == input.Length - 1 && x.Key.j == input[0].Length - 1)
            .Min(x => x.Value);
        })
            .Min();
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();
        var starts = new[] {
            (0, 0, Dir.R, 0),
            (0, 0, Dir.B, 0),
        };

        return starts.Select(x =>
        {
            var dists = Dijkstra(x,
                x => GetNeighboursUltra(x)
                .Where(x => IsInField((x.i, x.j), input))
                .ToArray(),
                (x, y) => GetDist((x.i, x.j), (y.i, y.j), input));
            return dists.Where(x => x.Key.i == input.Length - 1 && x.Key.j == input[0].Length - 1)
            .Min(x => x.Value);
        })
            .Min();
    }

    private static bool IsInField((int i, int j) point, long[][] input)
    {
        return point.i >= 0 && point.i < input.Length && point.j >= 0 && point.j < input[0].Length;
    }

    private static IEnumerable<(int i, int j, Dir d, int l)> GetNeighbours((int i, int j, Dir d, int l) item)
    {
        if (item.l < 3)
        {
            var (i, j) = GetNext(item.i, item.j, item.d, 1);
            yield return (i, j, item.d, item.l + 1);
        }

        foreach (var d in GetRL(item.d))
        {
            var (i, j) = GetNext(item.i, item.j, d, 1);
            yield return (i, j, d, 1);
        }
    }

    private static (int i, int j) GetNext(int i, int j, Dir d, int step)
    {
        return d switch
        {
            Dir.R => (i, j + step),
            Dir.L => (i, j - step),
            Dir.T => (i - step, j),
            Dir.B => (i + step, j),
            _ => throw new NotImplementedException(),
        };
    }

    private static IEnumerable<Dir> GetRL(Dir d)
    {
        switch (d)
        {
            case Dir.R:
            case Dir.L:
                yield return Dir.T;
                yield return Dir.B;
                break;
            case Dir.T:
            case Dir.B:
                yield return Dir.R;
                yield return Dir.L;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private static Dictionary<(int i, int j, Dir d, int l), long> Dijkstra((int i, int j, Dir d, int l) start, Func<(int i, int j, Dir d, int l), IEnumerable<(int i, int j, Dir d, int l)>> neighboursSelector, Func<(int i, int j, Dir d, int l), (int i, int j, Dir d, int l), long> distanceCalculator)
    {
        var distances = new Dictionary<(int i, int j, Dir d, int l), long>();
        var queue = new PriorityQueue<(int i, int j, Dir d, int l), long>(new[] { (start, 0L) });
        var set = new HashSet<(int i, int j, Dir d, int l)>
            {
                start
            };
        distances[start] = 0;
        while (queue.Count != 0)
        {
            var valve = queue.Dequeue();

            foreach (var neighbour in neighboursSelector(valve))
            {
                var length = distanceCalculator(valve, neighbour);
                var distance = distances.TryGetValue(neighbour, out long value) ? Math.Min(distances[valve] + length, value)
                    : distances[valve] + length;

                distances[neighbour] = distance;
                if (set.Add(neighbour))
                {
                    queue.Enqueue(neighbour, distance);
                }
            }
        }

        return distances;
    }

    private static IEnumerable<(int i, int j, Dir d, int l)> GetNeighboursUltra((int i, int j, Dir d, int l) item)
    {
        if (item.l < 4)
        {
            var step = 4 - item.l;
            var (i, j) = GetNext(item.i, item.j, item.d, step);
            yield return (i, j, item.d, item.l + step);
            yield break;
        }
        else if (item.l < 10)
        {
            var (i, j) = GetNext(item.i, item.j, item.d, 1);
            yield return (i, j, item.d, item.l + 1);
        }

        foreach (var d in GetRL(item.d))
        {
            var (i, j) = GetNext(item.i, item.j, d, 4);
            yield return (i, j, d, 4);
        }
    }

    private static long GetDist((int i, int j) first, (int i, int j) second, long[][] input)
    {
        var res = 0L;
        if (first.j == second.j)
        {
            var min = Math.Min(first.i, second.i);
            var max = Math.Max(first.i, second.i);

            if (first.i < second.i)
            {
                for (var k = min + 1; k <= max; ++k)
                {
                    res += input[k][first.j];
                }
            }
            else
            {
                for (var k = max - 1; k >= min; --k)
                {
                    res += input[k][first.j];
                }
            }
        }
        else
        {
            var min = Math.Min(first.j, second.j);
            var max = Math.Max(first.j, second.j);
            if (first.j < second.j)
            {
                for (var k = min + 1; k <= max; ++k)
                {
                    res += input[first.i][k];
                }
            }
            else
            {
                for (var k = max - 1; k >= min; --k)
                {
                    res += input[first.i][k];
                }
            }
        }

        return res;
    }

    private static IEnumerable<long[]> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return sr.ReadLine()
                .Select(x => (long)(x - '0'))
                .ToArray();
        }
    }

    private enum Dir
    {
        R,
        L,
        T,
        B
    }
}
