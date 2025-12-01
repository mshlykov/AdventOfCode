namespace AdventOfCode2023;

internal class Day12
{
    public static object SolveP1(string fileName)
    {
        var dict = new Dictionary<(string, string), long>();
        return GetInput(fileName)
            .Select((x, i) => CountArranges(dict, x.Str, x.Nums))
            .Sum();
    }

    public static object SolveP2(string fileName)
    {
        var dict = new Dictionary<(string, string), long>();
        return GetInput(fileName)
            .Select(x => CountArranges(dict, string.Join("?", x.Str, x.Str, x.Str, x.Str, x.Str), string.Join(",", x.Nums, x.Nums, x.Nums, x.Nums, x.Nums)))
            .Sum();
    }

    private static long CountArranges(Dictionary<(string, string), long> cache, string str, string nums)
    {
        if (cache.ContainsKey((str, nums)))
        {
            return cache[(str, nums)];
        }

        if (str.All(x => x == '.' || x == '?') && string.IsNullOrEmpty(nums))
        {
            return 1L;
        }

        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(nums))
        {
            return 0L;
        }

        var arr = nums.Split(",")
            .Select(int.Parse)
            .ToArray();
        var res = 0L;
        switch (str[0])
        {
            case '#':
                {
                    var newStr = CropMatch(str, arr[0]);
                    if (newStr != null)
                    {
                        res += CountArranges(cache, newStr, string.Join(",", arr[1..]));
                    }
                }
                break;
            case '?':
                {
                    var newStr = CropMatch(str, arr[0]);
                    if (newStr != null)
                    {
                        res += CountArranges(cache, newStr, string.Join(",", arr[1..]));
                    }

                    res += CountArranges(cache, str[1..], nums);
                }
                break;
            case '.':
                res += CountArranges(cache, str[1..], nums);
                break;
            default:
                throw new Exception();
        }

        cache.Add((str, nums), res);
        return res;
    }

    private static string CropMatch(string str, int num)
    {
        if (str.Length < num || !str[..num].All(x => x == '#' || x == '?') || str.Length != num && str[num] == '#')
        {
            return null;
        }

        return str.Length == num || str.Length == num + 1 ? string.Empty : str[(num + 1)..];
    }

    private static IEnumerable<(string Str, string Nums)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var parts = sr.ReadLine().Split(" ");
            yield return (parts[0], parts[1]);
        }
    }
}
