namespace AdventOfCode2024;

internal class Day19
{
    public static object SolveP1(string fileName)
    {
        var (supportedPatterns, patterns) = GetInput(fileName);
        var solutions = new Dictionary<string, bool>();
        return patterns.LongCount(x => IsSupported(x, supportedPatterns, solutions));
    }

    public static object SolveP2(string fileName)
    {
        var (supportedPatterns, patterns) = GetInput(fileName);
        var solutions = new Dictionary<string, long>();
        return patterns.Sum(x => CountSupported(x, supportedPatterns, solutions));
    }

    private static bool IsSupported(string pattern, IEnumerable<string> supportedPatterns, Dictionary<string, bool> solutions)
    {
        if (solutions.TryGetValue(pattern, out var result))
        {
            return result;
        }

        if (string.IsNullOrEmpty(pattern))
        {
            return true;
        }

        result = supportedPatterns.Where(x => pattern.StartsWith(x))
            .Any(x => IsSupported(pattern[x.Length..], supportedPatterns, solutions));
        solutions.Add(pattern, result);
        return result;
    }

    private static long CountSupported(string pattern, IEnumerable<string> supportedPatterns, Dictionary<string, long> solutions)
    {
        if (solutions.TryGetValue(pattern, out var result))
        {
            return result;
        }

        if (string.IsNullOrEmpty(pattern))
        {
            return 1L;
        }

        result = supportedPatterns.Where(x => pattern.StartsWith(x))
            .Sum(x => CountSupported(pattern[x.Length..], supportedPatterns, solutions));
        solutions.Add(pattern, result);
        return result;
    }

    private static (IEnumerable<string> SupportedPatterns, IEnumerable<string> Patterns) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        var supportedPatterns = sr.ReadLine().Split(", ");

        sr.ReadLine();

        var patterns = sr.ReadToEnd().Split(Environment.NewLine);
        return (supportedPatterns, patterns);
    }
}
