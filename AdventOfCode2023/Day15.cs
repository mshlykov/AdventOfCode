namespace AdventOfCode2023;

internal class Day15
{
    public static object SolveP1(string fileName) => GetInput(fileName)
        .Sum(GetHash);

    public static object SolveP2(string fileName)
    {
        var dict = new Dictionary<long, List<(string, long)>>();

        var input = GetInput(fileName);
        foreach (var x in input)
        {
            if (x.EndsWith("-"))
            {
                var name = x[..^1];
                var hash = GetHash(name);
                if (dict.TryGetValue(hash, out List<(string, long)>? value))
                {
                    var item = value.FirstOrDefault(x => x.Item1 == name, (null, -1));
                    if (item.Item1 != null)
                    {
                        value.Remove(item);
                    }
                }
            }
            else
            {
                var comps = x.Split("=");
                var name = comps[0];
                var val = long.Parse(comps[1]);
                var hash = GetHash(name);

                if (!dict.TryGetValue(hash, out List<(string, long)>? value))
                {
                    value = [];
                    dict[hash] = value;
                }

                var item = value.FirstOrDefault(x => x.Item1 == name, (null, -1));
                if (item.Item1 != null)
                {
                    var idx = value.IndexOf(item);
                    value[idx] = (name, val);
                }
                else
                {
                    value.Add((name, val));
                }
            }
        }

        var res = 0L;
        foreach (var pair in dict)
        {
            for (var j = 0; j < pair.Value.Count; ++j)
            {
                res += (pair.Key + 1) * (j + 1) * pair.Value[j].Item2;
            }
        }

        return res;
    }

    private static long GetHash(string s)
    {
        var curr = 0L;
        foreach (var c in s)
        {
            curr += c;
            curr *= 17;
            curr %= 256;
        }

        return curr;
    }

    private static IEnumerable<string> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        return sr.ReadToEnd().Split(",");
    }
}
