namespace AdventOfCode2023;

internal class Day25
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();
        var vertices = input.Aggregate(new HashSet<string>(), (set, x) =>
        {
            set.Add(x.Item1);
            set.Add(x.Item2);
            return set;
        });
        var dict = vertices.ToDictionary(x => x, x => new HashSet<string>());
        foreach (var (start, end) in input)
        {
            dict[start].Add(end);
            dict[end].Add(start);
        }
        var (item1, item2) = StoerWagnerMinCut(dict);
        return item1.Count * item2.Count;
    }
    
    private static (ICollection<string>, ICollection<string>) StoerWagnerMinCut(Dictionary<string, HashSet<string>> dict)
    {
        var vertsList = dict.Keys.Distinct()
            .Select(x => new HashSet<string>(new[] { x }))
            .ToList();
        var (connectedComponent1, connectedComponent2, cut) = (default(HashSet<string>), default(HashSet<string>), default(List<(string, string)>));
        while (vertsList.Count > 1)
        {
            var start = vertsList.First();
            var (component1, component2, cutEdges, mergedVertsList) = StoerWagnerMinCutPhase(start, vertsList, dict);
            
            if (cut == null || cutEdges.Count < cut.Count)
            {
                (connectedComponent1, connectedComponent2, cut) = (component1, component2, cutEdges);
            }

            vertsList = mergedVertsList;
        }

        return (connectedComponent1, connectedComponent2);
    }

    private static (HashSet<string>, HashSet<string>, List<(string, string)>, List<HashSet<string>> vertices) StoerWagnerMinCutPhase(HashSet<string> start, IEnumerable<HashSet<string>> vertices, Dictionary<string, HashSet<string>> dict)
    {
        var vertsSet = new HashSet<string>(start);
        var distances = vertices.Where(x => x != start).Select(x => (Vertex: x, EdgesCount: x.SelectMany(y => dict[y]).Count(y => vertsSet.Contains(y))))
            .ToDictionary(x => x.Vertex, x => x.EdgesCount);
        var preLast = start;
        while (distances.Count > 1)
        {
            var (vertex, count) = distances.MaxBy(x => x.Value);
            vertsSet.UnionWith(vertex);
            if (distances.Count == 2)
            {
                preLast = vertex;
            }

            distances.Remove(vertex);
            var neighbours = vertex.SelectMany(x => dict[x]).ToHashSet();
            foreach (var item in distances.Where(x => x.Key.Intersect(neighbours).Any()))
            {
                distances[item.Key] += 1;
            }
        }

        var last = distances.Single().Key;
        var resEdges = last.SelectMany(x => dict[x].Select(y => (y, x)))
            .Where(x => vertsSet.Contains(x.y))
            .ToList();

        var newVertex = preLast.Union(last).ToHashSet();
        var newVertices = vertices.Where(x => x != preLast && x != last).Concat(new[] { newVertex })
            .ToList();
        return (vertsSet, last, resEdges, newVertices);
    }

    private static IEnumerable<(string, string)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var parts = sr.ReadLine().Split(": ");
            foreach (var end in parts[1].Split(' '))
            {
                yield return (parts[0], end);
            }
        }
    }
}
