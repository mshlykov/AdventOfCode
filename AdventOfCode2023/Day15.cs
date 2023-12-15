﻿namespace AdventOfCode2023;

internal class Day15
{
    public static object SolveP1(string fileName) => GetInput(fileName)
        .Sum(GetHash);

    public static object SolveP2(string fileName)
    {
        var dict = new List<(string, long)>[256];
        for (var i = 0; i < dict.Length; ++i)
        {
            dict[i] = [];
        }

        var input = GetInput(fileName);
        foreach (var x in input)
        {
            if (x.EndsWith("-"))
            {
                var name = x[..^1];
                var hash = GetHash(name);
                var item = dict[hash].FirstOrDefault(x => x.Item1 == name, (null, -1));
                if (item.Item1 != null)
                {
                    dict[hash].Remove(item);
                }
            }
            else
            {
                var comps = x.Split("=");
                var name = comps[0];
                var val = long.Parse(comps[1]);
                var hash = GetHash(name);

                var item = dict[hash].FirstOrDefault(x => x.Item1 == name, (null, -1));
                if (item.Item1 != null)
                {
                    var idx = dict[hash].IndexOf(item);

                    dict[hash][idx] = (name, val);
                }
                else
                {
                    dict[hash].Add((name, val));
                }
            }
        }

        var res = 0L;
        for (var i = 0; i < dict.Length; ++i)
        {
            for (var j = 0; j < dict[i].Count; ++j)
            {
                res += (i + 1) * (j + 1) * dict[i][j].Item2;
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
