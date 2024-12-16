namespace AdventOfCode2024;

internal class Day16
{
    public static object SolveP1(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();
        var start = ((field.Length - 2, 1), Dir.R);
        var ends = new[] {
            ((1, field[1].Length - 2), Dir.T),
            ((1, field[1].Length - 2), Dir.R),
        };
        var res = Dijkstra(start, x => GetNeighbours(x, field), (x, y) => GetDist(x, y));
        return ends.Where(x => res.ContainsKey(x)).Min(x => res[x]);
    }

    public static object SolveP2(string fileName)
    {
        var field = GetInput(fileName)
                .ToArray();
        var start = ((field.Length - 2, 1), Dir.R);
        var ends = new[] {
            ((1, field[1].Length - 2), Dir.T),
            ((1, field[1].Length - 2), Dir.R),
        };
        var res = DijkstraWithBackTracking(start, x => GetNeighbours(x, field), (x, y) => GetDist(x, y));
        var minPathEnd = ends.MinBy(x => res.TryGetValue(x, out var value) ? value.D : long.MaxValue);
        var pathsItems = new HashSet<(int i, int j)>();
        for (var verts = new[] { minPathEnd }; verts.Length != 0;)
        {
            pathsItems.UnionWith(verts.Select(x => x.Item1));
            verts = verts.SelectMany(x => res.TryGetValue(x, out var value) ? value.V : Enumerable.Empty<((int i, int j) P, Dir D)>())
                .ToArray();
        }

        return pathsItems.Count;
    }

    private static ((int i, int j) P, Dir D)[] GetNeighbours(((int i, int j) P, Dir D) item, string[] field)
    {
        var neighbours = item.D switch
        {
            Dir.R => new ((int i, int j) P, Dir D)[]
            {
                ((item.P.i + 1, item.P.j), Dir.B),
                ((item.P.i - 1, item.P.j), Dir.T),
                ((item.P.i, item.P.j + 1), Dir.R),
            },
            Dir.L =>
            [
                ((item.P.i + 1, item.P.j), Dir.B),
                ((item.P.i - 1, item.P.j), Dir.T),
                ((item.P.i, item.P.j - 1), Dir.L),
            ],
            Dir.T =>
            [
                ((item.P.i - 1, item.P.j), Dir.T),
                ((item.P.i, item.P.j + 1), Dir.R),
                ((item.P.i, item.P.j - 1), Dir.L),
            ],
            Dir.B =>
            [
                ((item.P.i + 1, item.P.j), Dir.B),
                ((item.P.i, item.P.j + 1), Dir.R),
                ((item.P.i, item.P.j - 1), Dir.L),
            ],
            _ => throw new NotImplementedException()
        };

        return neighbours.Where(x => IsInField(x.P, field) && field[x.P.i][x.P.j] != '#')
            .ToArray();
    }

    private static long GetDist(((int i, int j) P, Dir D) item1, ((int i, int j) P, Dir D) item2) => Math.Abs(item1.P.i - item2.P.i) + Math.Abs(item1.P.j - item2.P.j) + (item1.D != item2.D ? 1000L : 0L);

    private static bool IsInField((int i, int j) p, string[] field) => p.i >= 0 && p.i < field.Length && p.j >= 0 && p.j < field[p.i].Length;

    private static Dictionary<T, long> Dijkstra<T>(T start, Func<T, IEnumerable<T>> neighboursSelector, Func<T, T, long> distanceCalculator)
        where T : notnull
    {
        var distances = new Dictionary<T, long>() {
            { start, 0 }
        };
        var queue = new PriorityQueue<T, long>([(start, 0L)]);
        var set = new HashSet<T>([start]);
        while (queue.Count != 0)
        {
            var currVertex = queue.Dequeue();

            foreach (var neighbour in neighboursSelector(currVertex))
            {
                var length = distanceCalculator(currVertex, neighbour);
                var distance = distances.TryGetValue(neighbour, out var value) ? Math.Min(distances[currVertex] + length, value)
                    : distances[currVertex] + length;

                distances[neighbour] = distance;
                if (set.Add(neighbour))
                {
                    queue.Enqueue(neighbour, distance);
                }
            }
        }

        return distances;
    }

    private static Dictionary<T, (long D, HashSet<T> V)> DijkstraWithBackTracking<T>(T start, Func<T, IEnumerable<T>> neighboursSelector, Func<T, T, long> distanceCalculator)
        where T : notnull
    {
        var distances = new Dictionary<T, (long D, HashSet<T> V)>() {
            { start, (0, new HashSet<T>())}
        };
        var queue = new PriorityQueue<T, long>([(start, 0L)]);
        var set = new HashSet<T>([start]);
        while (queue.Count != 0)
        {
            var currVertex = queue.Dequeue();

            foreach (var neighbour in neighboursSelector(currVertex))
            {
                var length = distanceCalculator(currVertex, neighbour);
                var newItem = !distances.TryGetValue(neighbour, out var data);
                var distance = !newItem ? Math.Min(distances[currVertex].D + length, data.D)
                    : distances[currVertex].D + length;
                distances[neighbour] = (distance, newItem ? new HashSet<T>([currVertex]) : data.V);
                if (!newItem && distances[currVertex].D + length == data.D)
                {
                    data.V.Add(currVertex);
                }

                if (set.Add(neighbour))
                {
                    queue.Enqueue(neighbour, distances[neighbour].D);
                }
            }
        }

        return distances;
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

    private enum Dir
    {
        R,
        L,
        B,
        T
    }
}
