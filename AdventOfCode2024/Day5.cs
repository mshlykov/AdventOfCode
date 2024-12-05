namespace AdventOfCode2024;

internal class Day5
{
    public static object SolveP1(string fileName)
    {
        var (pairs, arrs) = GetInput(fileName);
        var dict = pairs.GroupBy(x => x.Item2)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Item1).ToArray());

        return arrs.Where(x => ContainsErrors(x, dict))
            .Sum(x => x[x.Length / 2]);
    }

    private static bool ContainsErrors(long[]  arr, Dictionary<long, long[]> order)
    {
        for (var i = 0; i < arr.Length - 1; ++i)
        {
            if (order.ContainsKey(arr[i]) && arr[(i + 1)..].Intersect(order[arr[i]]).Any())
            {
                return false;
            }
        }

        return true;
    }

    public static object SolveP2(string fileName)
    {
        var (pairs, arrs) = GetInput(fileName);
        var dict = pairs.GroupBy(x => x.Item2)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Item1).ToArray());

        var arrsWithErrors = arrs.Where(x => !ContainsErrors(x, dict))
            .ToArray();

        foreach (var arr in arrsWithErrors) 
        {
            for (var i = 0; i < arr.Length - 1; ++i)
            {
                var leftover = arr[(i + 1)..];
                if (!dict.ContainsKey(arr[i]) || !leftover.Intersect(dict[arr[i]]).Any())
                {
                    continue;
                }

                var errs = leftover.Where(y => dict[arr[i]].Contains(y));
                var itemToSwap = errs.Cast<long?>().FirstOrDefault(y => !dict.ContainsKey(y!.Value) || !errs.Where(z => z != y).Intersect(dict[y.Value]).Any(), default(int?));
                if (itemToSwap == null)
                {
                    continue;
                }

                var idx = Array.IndexOf(leftover, itemToSwap.Value);
                var tmp = arr[i];
                arr[i] = arr[i + idx + 1];
                arr[i + idx + 1] = tmp;
            }
        }

        return arrsWithErrors.Sum(x => x[x.Length / 2]);
    }

    private static (IEnumerable<(long, long)>, IEnumerable<long[]>) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        var pairs = new List<(long, long)>();
        var line = sr.ReadLine();
        while (!string.IsNullOrEmpty(line))
        {
            var nums = line.Split("|");
            pairs.Add((long.Parse(nums[0]), long.Parse(nums[1])));
            line = sr.ReadLine();
        }

        var arrs = new List<long[]>();
        while (!sr.EndOfStream)
        {
            var arr = sr.ReadLine()
                .Split(",")
                .Select(long.Parse)
                .ToArray();

            arrs.Add(arr);
        }

        return (pairs, arrs);
    }
}
