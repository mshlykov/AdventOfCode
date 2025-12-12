#:sdk Microsoft.Net.Sdk
#:package LpSolveDotNet.Native.win-x64@4.1.0

using LpSolveDotNet;

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var input = ParseInput(fileName);
    var results = new List<long>();

    foreach (var machine in input)
    {
        var start = string.Join(string.Empty, Enumerable.Repeat('.', machine.Target.Length));
        var distances = Dijkstra(
            start,
            x => GetNeighbours(x, machine.Switches),
            (_, __) => 1L);

        results.Add(distances[machine.Target]);
    }

    return results.Sum();
}

object SolveP2(string fileName)
{
    var input = ParseInput(fileName);
    var results = new List<double>();
    LpSolve.Init();

    foreach (var machine in input)
    {
        // Setup integer linear programming problem
        using var lp = LpSolve.make_lp(3, machine.Switches.Length);
        lp.set_minim();
        lp.set_obj_fn([.. new[] { 0.0 }, .. Enumerable.Repeat(1.0, machine.Switches.Length)]);
        for (var i = 1; i <= machine.Switches.Length; i++)
        {
            lp.set_int(i, true);
        }

        lp.set_add_rowmode(true);
        for (var i = 0; i < machine.Joltage.Length; ++i)
        {
            lp.add_constraint([.. Enumerable.Repeat(0.0, 1), .. machine.Switches.Select(x => x.Contains(i) ? 1.0 : 0.0)], lpsolve_constr_types.EQ, machine.Joltage[i]);
        }
        lp.set_add_rowmode(false);

        lp.set_verbose(lpsolve_verbosity.CRITICAL);
        var ret = lp.solve();

        results.Add(lp.get_objective());
    }

    return results.Sum();
}

IEnumerable<string> GetNeighbours(string state, int[][] switches)
{
    return switches.Select(s =>
    {
        var arr = state.ToCharArray();
        foreach (var index in s)
        {
            arr[index] = arr[index] == '.' ? '#' : '.';
        }

        return new string(arr);
    })
    .ToArray();
}

Dictionary<T, long> Dijkstra<T>(T start, Func<T, IEnumerable<T>> neighboursSelector, Func<T, T, long> distanceCalculator)
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

IEnumerable<Machine> ParseInput(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? throw new InvalidOperationException();
        var parts = line.Split(" ");
        var target = parts[0].TrimStart('[')
        .TrimEnd(']');
        var switches = parts[1..^1].Select(
            s => s.TrimStart('(')
            .TrimEnd(')')
            .Split(',')
            .Select(int.Parse)
            .ToArray())
        .ToArray();
        var joltage = parts[^1].TrimStart('{')
        .TrimEnd('}')
        .Split(',')
        .Select(int.Parse)
        .ToArray();
        yield return new Machine(target, switches, joltage);
    }
}

record Machine(string Target, int[][] Switches, int[] Joltage);