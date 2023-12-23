namespace AdventOfCode2023;

internal class Day23
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();
        var dict = BuildGraph(input, true);
        ReduceGraph(dict);
        var paths = DFS((0, 1), (input.Length - 1, input[0].Length - 2), x => dict[x].Keys);
        return paths.Max(x => Enumerable.Range(1, x.Count - 1).Sum(y => dict[x[y - 1]][x[y]]));
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();
        var dict = BuildGraph(input, false);
        ReduceGraph(dict);
        var paths = DFS((0, 1), (input.Length - 1, input[0].Length - 2), x => dict[x].Keys);
        return paths.Max(x => Enumerable.Range(1, x.Count - 1).Sum(y => dict[x[y - 1]][x[y]]));
    }

    private static Dictionary<(int i, int j), Dictionary<(int i, int j), long>> BuildGraph(string[] input, bool oriented)
    {
        var vertices = new List<(int i, int j)>(new[] {
            (0, 1),
            (input.Length - 1, input[0].Length - 2)
        });
        var probes = new[] {
            (i: 0, j: 1),
            (i: 0, j: -1),
            (i: 1, j: 0),
            (i: -1, j: 0)
        };
        for (var i = 1; i < input.Length - 1; ++i)
        {
            for (var j = 0; j < input[0].Length - 1; ++j)
            {
                if (input[i][j] != '.')
                {
                    continue;
                }

                var neighboursProbes = probes
                    .Where(x => IsInField((i + x.i, j + x.j), input) && input[i + x.i][j + x.j] != '#')
                    .ToArray();
                if (neighboursProbes.Length == 1 || neighboursProbes.Max(x => Math.Abs(x.i)) == 1 && neighboursProbes.Max(x => Math.Abs(x.j)) == 1)
                {
                    vertices.Add((i, j));
                }
            }
        }

        var dict = vertices.ToDictionary(x => x, x => new Dictionary<(int i, int j), long>());
        for (var i = 0; i < vertices.Count; ++i)
        {
            for (var j = i + 1; j < vertices.Count; ++j)
            {
                var (first, second) = (vertices[i], vertices[j]);
                if (first.i != second.i && first.j != second.j)
                {
                    continue;
                }

                var firstToSecond = true;
                var secondToFirst = true;
                if (first.j != second.j)
                {
                    var (min, max) = (Math.Min(first.j, second.j), Math.Max(first.j, second.j));
                    for (var t = min + 1; t < max; ++t)
                    {
                        if (input[first.i][t] == '#' || vertices.Contains((first.i, t)))
                        {
                            firstToSecond = false;
                            secondToFirst = false;
                            break;
                        }

                        if (oriented && input[first.i][t] == '>')
                        {
                            firstToSecond = min == first.j;
                            secondToFirst = min == second.j;
                        }
                        if (oriented && input[first.i][t] == '<')
                        {
                            firstToSecond = max == first.j;
                            secondToFirst = max == second.j;
                        }
                    }
                }
                else
                {
                    var (min, max) = (Math.Min(first.i, second.i), Math.Max(first.i, second.i));
                    for (var t = min + 1; t < max; ++t)
                    {
                        if (input[t][first.j] == '#' || vertices.Contains((t, first.j)))
                        {
                            firstToSecond = false;
                            secondToFirst = false;
                            break;
                        }

                        if (oriented && input[t][first.j] == 'v')
                        {
                            firstToSecond = min == first.i;
                            secondToFirst = min == second.i;
                        }
                        if (oriented && input[t][first.j] == '^')
                        {
                            firstToSecond = max == first.i;
                            secondToFirst = max == second.i;
                        }
                    }
                }

                if (firstToSecond)
                {
                    dict[first][second] = GetDistance(first, second);
                }
                if (secondToFirst)
                {
                    dict[second][first] = GetDistance(first, second);
                }
            }
        }

        return dict;
    }

    private static void ReduceGraph(Dictionary<(int i, int j), Dictionary<(int i, int j), long>> dict)
    {
        var vertices = dict.Keys.ToArray();
        foreach (var vertex in vertices)
        {
            var incoming = dict.Where(x => x.Value.ContainsKey(vertex)).Select(x => x.Key).ToHashSet();
            var outgoing = dict[vertex].Keys.ToHashSet();

            if (incoming.Count == 1 && dict[vertex].Count == 1 && !incoming.SetEquals(outgoing))
            {
                var incomingVert = incoming.Single();
                var outgoingVert = outgoing.Single();
                var distance = dict[incomingVert][vertex] + dict[vertex][outgoingVert];
                dict[incomingVert].Remove(vertex);
                dict[incomingVert][outgoingVert] = distance;
                dict.Remove(vertex);
            }
            else if (incoming.Count == 2 && incoming.SetEquals(outgoing))
            {
                var vert1 = incoming.First();
                var vert2 = incoming.Last();
                var distance = dict[vert1][vertex] + dict[vertex][vert2];
                dict[vert1].Remove(vertex);
                dict[vert1][vert2] = distance;
                dict[vert2].Remove(vertex);
                dict[vert2][vert1] = distance;
                dict.Remove(vertex);
            }
        }
    }

    private static bool IsInField((int i, int j) point, string[] input)
    {
        return point.i >= 0 && point.i < input.Length && point.j >= 0 && point.j < input[0].Length;
    }

    private static long GetDistance((int i, int j) p1, (int i, int j) p2)
    {
        return Math.Abs(p1.i - p2.i) + Math.Abs(p1.j - p2.j);
    }

    private static IEnumerable<IList<(int i, int j)>> DFS((int i, int j) start, (int i, int j) end, Func<(int i, int j), IEnumerable<(int i, int j)>> neighboursSelector)
    {
        var stack = new Stack<((int i, int j) Point, Dictionary<(int i, int j), bool>)>();
        stack.Push((start, neighboursSelector(start).ToDictionary(x => x, x => false)));

        while (stack.Count != 0)
        {
            var (point, neighbours) = stack.Peek();

            if (neighbours.All(x => x.Value))
            {
                stack.Pop();
                if (stack.Count != 0)
                {
                    var (prevPoint, prevNeighbours) = stack.Peek();
                    prevNeighbours[point] = true;
                }
            }
            else
            {
                var firstNotChecked = neighbours.First(x => !x.Value);

                var notCheckedNeighbours = neighboursSelector(firstNotChecked.Key)
                    .Where(x => !stack.Any(y => y.Point == x))
                    .ToDictionary(x => x, x => false);

                if (notCheckedNeighbours.Count == 0)
                {
                    neighbours[firstNotChecked.Key] = true;
                }
                else
                {
                    var firstNotCheckedNeighbour = notCheckedNeighbours.First();
                    if (firstNotCheckedNeighbour.Key == end)
                    {
                        neighbours[firstNotChecked.Key] = true;
                        yield return stack.Select(x => x.Point).Reverse().Concat(new[] { firstNotChecked.Key, firstNotCheckedNeighbour.Key })
                            .ToArray();
                    }
                    else
                    {
                        stack.Push((firstNotChecked.Key, notCheckedNeighbours));
                    }
                }
            }
        }
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
