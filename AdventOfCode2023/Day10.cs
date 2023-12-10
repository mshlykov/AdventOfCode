namespace AdventOfCode2023;

internal class Day10
{
    public static object SolveP1(string fileName)
    {
        var matrix = GetInput(fileName).ToArray();
        var start = GetStart(matrix);

        return GetLoop(matrix, start).Count / 2;
    }

    private static (int, int) GetStart(string[] matrix)
    {
        for (var i = 0; i < matrix.Length; ++i)
            for (var j = 0; j < matrix[i].Length; ++j)
                if (matrix[i][j] == 'S')
                {
                    return (i, j);
                }

        throw new Exception();
    }

    private static List<(int, int)> GetLoop(string[] matrix, (int, int) start)
    {
        var startProbes = new[] {
            (start.Item1 + 1, start.Item2),
            (start.Item1 - 1, start.Item2),
            (start.Item1, start.Item2 + 1),
            (start.Item1, start.Item2 - 1),
        }
        .Where(x => x.Item1 >= 0 && x.Item1 < matrix.Length && x.Item2 >= 0 && x.Item2 < matrix[x.Item1].Length);
        foreach (var probe in startProbes)
        {
            var prev = start;
            var curr = probe;
            var found = false;
            var list = new List<(int, int)>();
            while (true)
            {
                var (i, j) = curr;
                var next = matrix[i][j] switch
                {
                    '|' => prev != (i - 1, j) ? (i - 1, j) : (i + 1, j),
                    '-' => prev != (i, j - 1) ? (i, j - 1) : (i, j + 1),
                    'F' => prev != (i, j + 1) ? (i, j + 1) : (i + 1, j),
                    '7' => prev != (i, j - 1) ? (i, j - 1) : (i + 1, j),
                    'J' => prev != (i, j - 1) ? (i, j - 1) : (i - 1, j),
                    'L' => prev != (i, j + 1) ? (i, j + 1) : (i - 1, j),
                    _ => default((int, int)?),
                };

                if (next == null || !(next.Value.Item1 >= 0 && next.Value.Item1 < matrix.Length && next.Value.Item2 >= 0 && next.Value.Item2 < matrix[next.Value.Item1].Length))
                {
                    break;
                }

                list.Add(curr);

                if (matrix[next.Value.Item1][next.Value.Item2] == 'S')
                {
                    list.Add(next.Value);
                    found = true;
                    break;
                }

                prev = curr;
                curr = next.Value;
            }

            if (found)
            {
                return list;
            }
        }

        throw new Exception();
    }

    public static object SolveP2(string fileName)
    {
        var matrix = GetInput(fileName).ToArray();
        var start = GetStart(matrix);
        var visitedList = GetLoop(matrix, start);
        var points = matrix.SelectMany((x, i) => x.Select((y, j) => (i, j)))
            .Where(x => !visitedList.Contains(x))
            .ToArray();

        var count = points
            .Where(x =>
            {
                var rPoints = visitedList.Where(y => y.Item1 == x.Item1 && y.Item2 > x.Item2)
                .OrderBy(x => x.Item2)
                .ToArray();

                var rCount = 0;
                for (var i = 0; i < rPoints.Length; ++i)
                {
                    var currItem = rPoints[i];
                    var curr = matrix[currItem.Item1][currItem.Item2];
                    if (curr == '|')
                    {
                        rCount++;
                    }
                    else
                    {
                        ++i;
                        while (matrix[rPoints[i].Item1][rPoints[i].Item2] == '-')
                        {
                            ++i;
                        }

                        var idx1 = visitedList.IndexOf(currItem);
                        var idx2 = visitedList.IndexOf((rPoints[i].Item1, rPoints[i].Item2));
                        var sgn = Math.Sign(idx1 - idx2);
                        var idx11 = idx1 + sgn < 0 ? visitedList.Count - 1 : (idx1 + sgn) % visitedList.Count;
                        var idx21 = idx2 - sgn < 0 ? visitedList.Count - 1 : (idx2 - sgn) % visitedList.Count;
                        if (visitedList[idx11].Item1 > x.i != visitedList[idx21].Item1 > x.i)
                        {
                            rCount++;
                        }
                    }
                }


                return rCount % 2 == 1;
            })
            .ToArray();


        return count.Length;
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
