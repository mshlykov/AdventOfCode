namespace AdventOfCode2024;

internal class Day10
{
    public static object SolveP1(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();
        var res = 0L;

        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                if (field[i][j] != 0)
                {
                    continue;
                }
                var queue = new Queue<(int, int)>([(i, j)]);
                var visited = new HashSet<(int, int)>([(i, j)]);

                while (queue.Count != 0)
                {
                    var currPoint = queue.Dequeue();

                    if (field[currPoint.Item1][currPoint.Item2] == 9)
                    {
                        ++res;
                        continue;
                    }

                    var neighbours = GetNeihgbours(field, currPoint)
                        .Where(x => !visited.Contains(x) && field[x.Item1][x.Item2] == field[currPoint.Item1][currPoint.Item2] + 1);
                    foreach (var neighbour in neighbours)
                    {
                        visited.Add(neighbour);
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        return res;
    }

    public static object SolveP2(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();
        var res = 0L;

        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                if (field[i][j] != 0)
                {
                    continue;
                }
                var queue = new Queue<(int, int)>([(i, j)]);

                while (queue.Count != 0)
                {
                    var currPoint = queue.Dequeue();

                    if (field[currPoint.Item1][currPoint.Item2] == 9)
                    {
                        ++res;
                        continue;
                    }

                    var neighbours = GetNeihgbours(field, currPoint)
                        .Where(x => field[x.Item1][x.Item2] == field[currPoint.Item1][currPoint.Item2] + 1);
                    foreach (var neighbour in neighbours)
                    {
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        return res;
    }

    private static IEnumerable<(int, int)> GetNeihgbours(int[][] field, (int, int) point)
    {
        var (i, j) = point;

        return new[]
        {
            (i - 1, j),
            (i + 1, j),
            (i, j - 1),
            (i, j + 1)
        }
        .Where(x => x.Item1 >= 0 && x.Item1 < field.Length && x.Item2 >= 0 && x.Item2 < field[x.Item1].Length);
    }

    private static IEnumerable<int[]> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return sr.ReadLine()
                .Select(x => x - '0')
                .ToArray();
        }
    }
}
