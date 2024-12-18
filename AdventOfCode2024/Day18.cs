namespace AdventOfCode2024;

internal class Day18
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName);
        var (maxX, maxY) = (0, 0);
        if (fileName == "1.txt")
        {
            (maxX, maxY) = (6, 6);
            input = input.Take(12);
        }
        else
        {
            (maxX, maxY) = (70, 70);
            input = input.Take(1024);
        }
        var obstacles = input.ToHashSet();
        var res = Dijkstra((X: 0, Y: 0), p =>
        {
            var (x, y) = p;
            return new[] {
                (X: x + 1, Y: y),
                (X: x - 1, Y: y),
                (X: x, Y: y + 1),
                (X: x, Y: y - 1)
            }.Where(p => p.X >= 0 && p.X <= maxX && p.Y >= 0 && p.Y <= maxY && !obstacles.Contains(p));
        }, (p1, p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y));
        var path = new List<(long, long)>();
        for (var end = res[(maxX, maxY)]; end.V != null; end = res[end.V.Value])
        {
            path.Add(end.V.Value);
        }
        return path.Count;
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();
        var (maxX, maxY) = (0, 0);
        if (fileName == "2.txt")
        {
            (maxX, maxY) = (6, 6);
        }
        else
        {
            (maxX, maxY) = (70, 70);
        }

        for (var i = 1; i <= input.Length; ++i)
        {
            var obstacles = input.Take(i)
                .ToHashSet();
            // reused Dijkstra though could be regular BFS
            var res = Dijkstra((X: 0, Y: 0), p =>
            {
                var (x, y) = p;
                return new[] {
                (X: x + 1, Y: y),
                (X: x - 1, Y: y),
                (X: x, Y: y + 1),
                (X: x, Y: y - 1)
            }.Where(p => p.X >= 0 && p.X <= maxX && p.Y >= 0 && p.Y <= maxY && !obstacles.Contains(p)).ToArray();
            }, (p1, p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y));
            if (!res.ContainsKey((maxX, maxY)))
            {
                return input[i - 1];
            }
        }

        return (-1, -1);
    }

    private static Dictionary<T, (long D, T? V)> Dijkstra<T>(T start, Func<T, IEnumerable<T>> neighboursSelector, Func<T, T, long> distanceCalculator)
        where T : struct
    {
        var distances = new Dictionary<T, (long D, T? V)>() {
            { start, (0, default(T?)) }
        };
        var queue = new PriorityQueue<T, long>([(start, 0L)]);
        var set = new HashSet<T>([start]);
        while (queue.Count != 0)
        {
            var currVertex = queue.Dequeue();

            foreach (var neighbour in neighboursSelector(currVertex))
            {
                var length = distanceCalculator(currVertex, neighbour);
                var distance = distances[currVertex].D + length;
                if (!distances.TryGetValue(neighbour, out var data) || distance < data.D)
                {
                    distances[neighbour] = (distance, currVertex);
                }

                if (set.Add(neighbour))
                {
                    queue.Enqueue(neighbour, distance);
                }
            }
        }

        return distances;
    }

    private static IEnumerable<(int X, int Y)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var tuple = sr.ReadLine().Split(",");
            yield return (int.Parse(tuple[0]), int.Parse(tuple[1]));
        }
    }
}
