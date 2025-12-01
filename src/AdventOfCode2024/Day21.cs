namespace AdventOfCode2024;

internal class Day21
{
    private static readonly Dictionary<char, (int X, int Y)> _keypadCoords = new()
    {
        { '<', (0, 0) },
        { 'v', (1, 0) },
        { '>', (2, 0) },
        { '^', (1, 1) },
        { 'A', (2, 1) }
    };

    private static readonly Dictionary<char, (int X, int Y)> _numpadCoords = new()
    {
        { '0', (1, 0) },
        { '1', (0, 1) },
        { '2', (1, 1) },
        { '3', (2, 1) },
        { '4', (0, 2) },
        { '5', (1, 2) },
        { '6', (2, 2) },
        { '7', (0, 3) },
        { '8', (1, 3) },
        { '9', (2, 3) },
        { 'A', (2, 0) }
    };

    public static object SolveP1(string fileName)
    {
        var (countCache, numPadCache, keyPadCache) = (
            new Dictionary<(string Instruction, int Level), long>(),
            new Dictionary<((int X, int Y), (int X, int Y)), ICollection<string>>(),
            new Dictionary<((int X, int Y), (int X, int Y)), ICollection<string>>());
        return GetInput(fileName)
            .Sum(x => long.Parse(x[0..^1]) * CountMinNumPadPaths(x, 2, countCache, numPadCache, keyPadCache));
    }

    public static object SolveP2(string fileName)
    {
        var (countCache, numPadCache, keyPadCache) = (
            new Dictionary<(string Instruction, int Level), long>(),
            new Dictionary<((int X, int Y), (int X, int Y)), ICollection<string>>(),
            new Dictionary<((int X, int Y), (int X, int Y)), ICollection<string>>());
        return GetInput(fileName)
            .Sum(x => long.Parse(x[0..^1]) * CountMinNumPadPaths(x, 25, countCache, numPadCache, keyPadCache));
    }

    private static long CountMinNumPadPaths(string str, int keyPadsNumber, IDictionary<(string X, int Y), long> keyPadCountCache, IDictionary<((int X, int Y), (int X, int Y)), ICollection<string>> numPadPathsCache, IDictionary<((int X, int Y), (int X, int Y)), ICollection<string>> keyPadPathsCache)
    {
        var result = 0L;
        var currChar = 'A';
        for (var i = 0; i < str.Length; currChar = str[i], ++i)
        {
            result += EnumerateNumPadPaths(_numpadCoords[currChar], _numpadCoords[str[i]], numPadPathsCache)
                .Select(x => x + "A")
                .Min(x => CountMinKeyPadPath(x, 0, keyPadsNumber, keyPadCountCache, keyPadPathsCache));
        }

        return result;
    }

    private static long CountMinKeyPadPath(string str, int level, int maxLevel, IDictionary<(string X, int Y), long> countCache, IDictionary<((int X, int Y), (int X, int Y)), ICollection<string>> keyPadCache)
    {
        if (countCache.TryGetValue((str, level), out var result))
        {
            return result;
        }

        if (level == maxLevel)
        {
            result = str.Length;
        }
        else
        {
            result = 0L;
            var currChar = 'A';
            for (var i = 0; i < str.Length; currChar = str[i], ++i)
            {
                result += EnumerateKeyPadPaths(_keypadCoords[currChar], _keypadCoords[str[i]], keyPadCache)
                    .Select(x => x + "A")
                    .Min(x => CountMinKeyPadPath(x, level + 1, maxLevel, countCache, keyPadCache));
            }
        }

        countCache.Add((str, level), result);
        return result;
    }

    private static ICollection<string> EnumerateNumPadPaths((int X, int Y) source, (int X, int Y) dest, IDictionary<((int X, int Y), (int X, int Y)), ICollection<string>> cache)
    {
        if (source == dest)
        {
            return [string.Empty];
        }

        if (cache.TryGetValue((source, dest), out var cachedResult))
        {
            return cachedResult;
        }

        var result = new List<string>();
        if (source.X < dest.X)
        {
            result.AddRange(EnumerateNumPadPaths((source.X + 1, source.Y), dest, cache).Select(x => ">" + x));
        }

        if (source.X > dest.X && source != (1, 0))
        {
            result.AddRange(EnumerateNumPadPaths((source.X - 1, source.Y), dest, cache).Select(x => "<" + x));
        }

        if (source.Y > dest.Y && source != (0, 1))
        {
            result.AddRange(EnumerateNumPadPaths((source.X, source.Y - 1), dest, cache).Select(x => "v" + x));
        }

        if (source.Y < dest.Y)
        {
            result.AddRange(EnumerateNumPadPaths((source.X, source.Y + 1), dest, cache).Select(x => "^" + x));
        }

        cache.TryAdd((source, dest), result);
        return result;
    }

    private static ICollection<string> EnumerateKeyPadPaths((int X, int Y) source, (int X, int Y) dest, IDictionary<((int X, int Y), (int X, int Y)), ICollection<string>> cache)
    {
        if (source == dest)
        {
            return [string.Empty];
        }

        if (cache.TryGetValue((source, dest), out var cachedResult))
        {
            return cachedResult;
        }

        var result = new List<string>();
        if (source.X < dest.X)
        {
            result.AddRange(EnumerateKeyPadPaths((source.X + 1, source.Y), dest, cache).Select(x => ">" + x));
        }

        if (source.X > dest.X && source != (1, 1))
        {
            result.AddRange(EnumerateKeyPadPaths((source.X - 1, source.Y), dest, cache).Select(x => "<" + x));
        }

        if (source.Y > dest.Y)
        {
            result.AddRange(EnumerateKeyPadPaths((source.X, source.Y - 1), dest, cache).Select(x => "v" + x));
        }

        if (source.Y < dest.Y && source != (0, 0))
        {
            result.AddRange(EnumerateKeyPadPaths((source.X, source.Y + 1), dest, cache).Select(x => "^" + x));
        }

        cache.TryAdd((source, dest), result);
        return result;
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
