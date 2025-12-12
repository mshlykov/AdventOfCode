#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var verticesData = ParseInput(fileName)
    .ToDictionary(x => x.Vertex, x => x.Neighbours);
    var cache = new Dictionary<string, long>();
    var result = CountPathsDag("you", e => e == "out", cache, verticesData);
    return result;
}

object SolveP2(string fileName)
{
    var verticesData = ParseInput(fileName)
    .ToDictionary(x => x.Vertex, x => x.Neighbours);

    var cache = new Dictionary<string, long>();
    var result1 = CountPathsDag("svr", e => e == "fft", cache, verticesData);
    cache.Clear();
    result1 *= CountPathsDag("fft", e => e == "dac", cache, verticesData);
    cache.Clear();
    result1 *= CountPathsDag("dac", e => e == "out", cache, verticesData);
    cache.Clear();
    var result2 = CountPathsDag("svr", e => e == "dac", cache, verticesData);
    cache.Clear();
    result2 *= CountPathsDag("fac", e => e == "fft", cache, verticesData);
    cache.Clear();
    result2 *= CountPathsDag("fft", e => e == "out", cache, verticesData);
    return result1 + result2;
}

long CountPaths(string current, Func<string, HashSet<string>, bool> isEnd, HashSet<string> visited, Dictionary<string, long> cache, Dictionary<string, string[]> verticesData)
{
    var key = string.Join("|", visited.OrderBy(x => x));
    if (cache.TryGetValue(key, out long result))
    {
        return result;
    }

    if (isEnd(current, visited))
    {
        result = 1;
    }
    else if (!verticesData.TryGetValue(current, out string[]? value))
    {
        result = 0;
    }
    else
    {
        result = 0;
        foreach (var neighbor in value)
        {
            if (visited.Add(neighbor))
            {
                result += CountPaths(neighbor, isEnd, visited, cache, verticesData);
                visited.Remove(neighbor);
            }
        }
    }


    cache[key] = result;
    return result;
}

// only works because the input graph is a DAG
long CountPathsDag(string current, Func<string, bool> isEnd, Dictionary<string, long> cache, Dictionary<string, string[]> verticesData)
{
    var key = current;
    if (cache.TryGetValue(key, out long result))
    {
        return result;
    }

    if (isEnd(current))
    {
        result = 1;
    }
    else if (!verticesData.TryGetValue(current, out string[]? neighbours))
    {
        result = 0;
    }
    else
    {
        result = 0;
        foreach (var neighbor in neighbours)
        {
            result += CountPathsDag(neighbor, isEnd, cache, verticesData);
        }
    }

    cache[key] = result;
    return result;
}

IEnumerable<IncidenceList> ParseInput(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? throw new InvalidOperationException();
        var parts = line.Split(": ");
        yield return new IncidenceList(parts[0], parts[1].Split(" "));
    }
}

record IncidenceList(string Vertex, string[] Neighbours);