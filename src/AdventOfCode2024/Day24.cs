using System.Data;
using System.Globalization;

namespace AdventOfCode2024;

internal class Day24
{
    public static object SolveP1(string fileName)
    {
        var (initVals, exprs) = GetInput(fileName);
        var values = initVals.ToDictionary(x => x.Name, x => (bool?)x.Val);
        var exprsDict = exprs.ToDictionary(x => x.Dest, x => (x.V1, x.V2, x.Op));
        var str = Evaluate(values, exprsDict);

        return long.Parse(str.Reverse().ToArray(), NumberStyles.BinaryNumber);
    }

    private static bool? EvaluateExpessionWithLoopDetection(string name, HashSet<string> path, Dictionary<string, (string V1, string V2, string Op)> exprs, Dictionary<string, bool?> values)
    {
        if (values.TryGetValue(name, out var result))
        {
            return result;
        }

        if (path.Contains(name))
        {
            return null;
        }

        path.Add(name);
        var (v1, v2, op) = exprs[name];
        var (v1Val, v2Val) = (EvaluateExpessionWithLoopDetection(v1, path, exprs, values), EvaluateExpessionWithLoopDetection(v2, path, exprs, values));
        result = v1Val != null && v2Val != null ? op switch
        {
            "AND" => v1Val.Value && v2Val.Value,
            "OR" => v1Val.Value || v2Val.Value,
            "XOR" => v1Val.Value ^ v2Val.Value,
            _ => throw new NotImplementedException()
        } : null;
        values.Add(name, result);
        return result;
    }

    private static char[]? Evaluate(Dictionary<string, bool?> values, Dictionary<string, (string V1, string V2, string Op)> exprs)
    {
        foreach (var v in exprs.Keys.Where(x => x.StartsWith('z')))
        {
            EvaluateExpessionWithLoopDetection(v, [], exprs, values);
        }

        var res = values.Where(x => x.Key.StartsWith('z'));

        if (res.Any(x => x.Value == null))
        {
            return null;
        }

        return res.OrderBy(x => x.Key)
        .Select(x => x.Value.Value ? '1' : '0')
        .ToArray();
    }

    public static object SolveP2(string fileName)
    {
        // manual solution using https://www.reddit.com/r/adventofcode/comments/1hl698z/2024_day_24_solutions/ and GraphViz
        return string.Empty;
    }

    private static (IEnumerable<(string Name, bool Val)> InitialValues, IEnumerable<(string V1, string V2, string Op, string Dest)> Exprs) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        var initialValues = new List<(string Name, bool Val)>();
        for (var line = sr.ReadLine(); !string.IsNullOrEmpty(line); line = sr.ReadLine())
        {
            var segments = line.Split(": ");
            initialValues.Add((segments[0], segments[1] == "1"));
        }

        var exprs = new List<(string V1, string V2, string Op, string Dest)>();
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var firstSplit = line.Split(" -> ");
            var op = firstSplit[0].Contains("AND") ? "AND"
                : firstSplit[0].Contains("XOR") ? "XOR"
                : "OR";
            var secondSplit = firstSplit[0].Split($" {op} ");
            exprs.Add((secondSplit[0], secondSplit[1], op, firstSplit[1]));
        }

        return (initialValues, exprs);
    }
}
