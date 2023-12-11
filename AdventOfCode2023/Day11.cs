namespace AdventOfCode2023;

internal class Day11
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();

        return GetDistances(input, 2L).Sum();
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName)
           .ToArray();

        return GetDistances(input, 1000000L).Sum();
    }

    private static List<int> GetEmptyRows(string[] input)
    {
        return input.Select((x, i) => (x, i))
            .Where(x => x.x.All(y => y == '.'))
            .Select(x => x.i)
            .ToList();
    }

    private static List<int> GetEmptyCols(string[] input)
    {
        var res = new List<int>();
        for (var i = 0; i < input[0].Length; ++i)
        {
            var empty = true;
            for (var j = 0; j < input.Length; ++j)
            {
                if (input[j][i] != '.')
                {
                    empty = false;
                }
            }


            if (empty)
            {
                res.Add(i);
            }
        }

        return res;
    }

    private static List<(int i, int j)> GetGalaxies(string[] input)
    {
        var res = new List<(int, int)>();
        for (var i = 0; i < input.Length; ++i)
        {
            for (var j = 0; j < input[0].Length; ++j)
                if (input[i][j] == '#')
                {
                    res.Add((i, j));
                }
        }

        return res;
    }

    public static IEnumerable<long> GetDistances(string[] input, long multiplier)
    {
        var (rows, cols) = (GetEmptyRows(input), GetEmptyCols(input));

        var galaxies = GetGalaxies(input);
        for (var i = 0; i < galaxies.Count; ++i)
        {
            for (var j = i + 1; j < galaxies.Count; ++j)
            {
                var (first, second) = (galaxies[i], galaxies[j]);
                var (minI, maxI) = (Math.Min(first.i, second.i), Math.Max(first.i, second.i));
                var rowsBCount = rows.LongCount(x => x > minI && x < maxI);

                var (minJ, maxJ) = (Math.Min(first.j, second.j), Math.Max(first.j, second.j));
                var colsBCount = cols.LongCount(x => x > minJ && x < maxJ);

                yield return maxI - minI + maxJ - minJ + colsBCount * (multiplier - 1) + rowsBCount * (multiplier - 1);
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
