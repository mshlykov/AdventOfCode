namespace AdventOfCode2024;

internal class Day23
{
    public static object SolveP1(string fileName)
    {
        var edges = GetInput(fileName)
            .ToArray();
        var incidenceLists = GetIncidenceLists(edges);
        var set = new HashSet<(string, string, string)>();
        foreach (var (v1, v2) in edges.Where(x => x.V1.StartsWith("t") || x.V2.StartsWith("t")))
        {
            foreach (var v3 in incidenceLists[v1].Intersect(incidenceLists[v2]))
            {
                var arr = new[] { v1, v2, v3 };
                Array.Sort(arr);
                set.Add((arr[0], arr[1], arr[2]));
            }
        }

        return set.Count;
    }

    public static object SolveP2(string fileName)
    {
        var edges = GetInput(fileName)
            .ToArray();
        var vertLists = GetIncidenceLists(edges);
        var vertices = vertLists.Keys.ToHashSet();
        var completeSubGraphs = new List<HashSet<string>>();
        while (edges.Length != 0)
        {
            var (v1, v2) = edges.First();
            var verts = new HashSet<string>([v1, v2]);
            var intersection = verts.Aggregate(new HashSet<string>(vertices), (res, item) =>
            {
                res.IntersectWith(vertLists[item]);
                return res;
            });
            while (intersection.Count != 0)
            {
                verts.Add(intersection.First());
                intersection = verts.Aggregate(new HashSet<string>(vertices), (res, item) =>
                {
                    res.IntersectWith(vertLists[item]);
                    return res;
                });
            }

            completeSubGraphs.Add(verts);
            edges = edges.Where(x => !verts.Contains(x.V1) && !verts.Contains(x.V2))
                .ToArray();
        }

        var maxSet = completeSubGraphs.MaxBy(x => x.Count);
        return string.Join(",", maxSet.OrderBy(x => x));
    }

    private static Dictionary<string, HashSet<string>> GetIncidenceLists(IEnumerable<(string V1, string V2)> edges)
    {
        var result = new Dictionary<string, HashSet<string>>();
        foreach (var (v1, v2) in edges)
        {
            if (!result.TryGetValue(v1, out var v1List))
            {
                v1List = [];
                result[v1] = v1List;
            }

            v1List.Add(v2);

            if (!result.TryGetValue(v2, out var v2List))
            {
                v2List = [];
                result[v2] = v2List;
            }

            v2List.Add(v1);
        }

        return result;
    }

    private static IEnumerable<(string V1, string V2)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine().Split("-");
            yield return (line[0], line[1]);
        }
    }
}
