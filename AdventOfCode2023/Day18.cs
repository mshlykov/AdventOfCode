namespace AdventOfCode2023;

internal class Day18
{
    public static object SolveP1(string fileName)
    {
        var data = GetInput(fileName)
            .Select(x => (x.d, x.l));
        var edges = GetEdges(data)
            .ToArray();

        return GetArea(edges);
    }

    public static object SolveP2(string fileName)
    {
        var data = GetInput(fileName)
            .Select(x =>
            {
                var d = x.c[^1] switch
                {
                    '0' => "R",
                    '1' => "D",
                    '2' => "L",
                    '3' => "U",
                    _ => throw new Exception()
                };

                var l = Convert.ToInt64(x.c[..^1], 16);

                return (d, l);
            });
        var edges = GetEdges(data)
            .ToArray();

        return GetArea(edges);
    }

    private static IEnumerable<((long i, long j) p1, (long i, long j) p2)> GetEdges(IEnumerable<(string d, long l)> input)
    {
        var curr = (i: 0L, j: 0L);
        var prev = curr;
        foreach (var (d, l) in input)
        {
            curr = d switch
            {
                "R" => (curr.i, j: curr.j + l),
                "L" => (curr.i, j: curr.j - l),
                "D" => (i: curr.i + l, curr.j),
                "U" => (i: curr.i - l, curr.j),
                _ => throw new Exception()
            };

            yield return (prev, curr);

            prev = curr;
        }
    }

    private static long GetThickness(long j, IEnumerable<((long i, long j) p1, (long i, long j) p2)> edges)
    {
        var crossingEdges = edges.Where(x => x.p1.i == x.p2.i && j >= Math.Min(x.p1.j, x.p2.j) && j <= Math.Max(x.p1.j, x.p2.j) ||
        x.p1.j == x.p2.j && x.p1.j == j)
                .Select(x => (Min: Math.Min(x.p1.i, x.p2.i), Max: Math.Max(x.p1.i, x.p2.i), Edge: x))
                .OrderBy(x => x)
                .ToArray();
        var sum = 1L;
        var isInside = true;
        for (var i = 1; i < crossingEdges.Length;)
        {
            if (crossingEdges[i].Min != crossingEdges[i].Max)
            {
                sum += crossingEdges[i].Max - crossingEdges[i].Min - 1;
            }
            else
            {
                if (isInside && crossingEdges[i].Min != crossingEdges[i - 1].Max)
                {
                    sum += crossingEdges[i].Min - crossingEdges[i - 1].Max - 1;
                }

                sum += 1;

                if (crossingEdges[i - 1].Min != crossingEdges[i - 1].Max)
                {
                    var j1 = crossingEdges[i - 2].Edge.p1.j != j ? crossingEdges[i - 2].Edge.p1.j : crossingEdges[i - 2].Edge.p2.j;
                    var j2 = crossingEdges[i].Edge.p1.j != j ? crossingEdges[i].Edge.p1.j : crossingEdges[i].Edge.p2.j;

                    if (j1 > j == j2 > j)
                    {
                        isInside = !isInside;
                    }
                }
                else
                {
                    isInside = !isInside;
                }
            }

            ++i;
        }

        return sum;
    }

    private static long GetArea(IEnumerable<((long i, long j) p1, (long i, long j) p2)> edges)
    {
        var jList = edges
            .Where(x => x.p1.j == x.p2.j)
            .Select(x => x.p1.j)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();
        var res = GetThickness(jList[0], edges);
        for (var t = 1; t < jList.Length; ++t)
        {
            if (jList[t] - jList[t - 1] - 1 != 0)
            {
                res += GetThickness(jList[t] - 1, edges) * (jList[t] - jList[t - 1] - 1);
            }

            res += GetThickness(jList[t], edges);
        }

        return res;
    }

    private static IEnumerable<(string d, long l, string c)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine().Split(" ");
            yield return (line[0], long.Parse(line[1]), line[2].Trim('(', ')')[1..]);
        }
    }
}
