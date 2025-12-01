namespace AdventOfCode2023;

internal class Day5
{
    public static object SolveP1(string fileName)
    {
        var (seeds, data) = GetInput(fileName);

        foreach (var map in data)
        {
            for (var i = 0; i < seeds.Length; ++i)
            {
                var range = map.FirstOrDefault(x => seeds[i] >= x.Source && seeds[i] <= x.Source + x.Length - 1, (-1, -1, -1));
                if (range.Item1 != -1)
                {
                    seeds[i] = range.Target + seeds[i] - range.Source;
                }
            }
        }

        return seeds.Min();
    }

    public static object SolveP2(string fileName)
    {
        var (seedsData, data) = GetInput(fileName);
        var seedsRanges = seedsData
            .Select((x, i) => (x, i))
            .GroupBy(x => x.i / 2)
            .Select(x => (Start: x.First().x, Length: x.Last().x))
            .ToList();

        foreach (var map in data)
        {
            var mappedSeedsRanges = new List<(long, long)>();
            foreach(var (target, source, length) in map)
            {
                var residualRanges = new List<(long, long)>();
                for (var i = 0; i < seedsRanges.Count; ++i)
                {
                    var range = seedsRanges[i];
                    if (range.Start < source && range.Start + range.Length - 1 >= source)
                    {
                        var newLength = source - range.Start + 1;
                        residualRanges.Add((range.Start, newLength));
                        range = (source, range.Length - newLength);
                    }

                    if (range.Start + range.Length - 1 > source + length - 1 && range.Start <= source + length - 1)
                    {
                        var newLength = range.Start + range.Length - source - length;
                        residualRanges.Add((source + length - 1, newLength));
                        range = (range.Start, range.Length - newLength);
                    }

                    if (range.Start >= source && range.Start + range.Length - 1 <= source + length - 1)
                    {
                        mappedSeedsRanges.Add((target + range.Start - source, range.Length));
                    }
                    else
                    {
                        residualRanges.Add(range);
                    }
                }
                seedsRanges = residualRanges;
            }
            seedsRanges = seedsRanges.Concat(mappedSeedsRanges).ToList();
        }

        return seedsRanges.Min(x => x.Start);
    }

    private static (long[] seeds, List<List<(long Target, long Source, long Length)>> data) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);
        var line = sr.ReadLine();
        var seeds = line.Substring(7).Split(' ').Select(x => long.Parse(x)).ToArray();

        line = sr.ReadLine();

        var data = new List<List<(long Target, long Source, long Length)>>();
        for (var i = 0; i < 7; ++i)
        {
            line = sr.ReadLine();
            line = sr.ReadLine();
            var mapping = new List<(long Target, long Source, long Length)>();
            while (line != null && line.Length != 0)
            {
                var nums = line.Split(' ').Select(x => long.Parse(x)).ToArray();
                mapping.Add((nums[0], nums[1], nums[2]));
                line = sr.ReadLine();
            }

            data.Add(mapping);
        }

        return (seeds, data);
    }
}
