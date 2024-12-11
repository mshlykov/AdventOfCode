namespace AdventOfCode2024;

internal class Day11
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName);
        var numsList = new LinkedList<long>(input);
        for (var i = 0; i < 25; ++i)
        {
            numsList = new LinkedList<long>(numsList.SelectMany(PerformBlink));
        }

        return numsList.Count;
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName);
        var numsWithCounts = new LinkedList<(long Num, long Count)>(input.GroupBy(x => x).Select(x => (x.Key, x.LongCount())));
        for (var i = 0; i < 75; ++i)
        {
            var newNumsWithCounts = numsWithCounts.SelectMany(x => PerformBlink(x.Num)
                    .GroupBy(y => y)
                    .Select(y => (Num: y.Key, Count: y.LongCount() * x.Count))
                )
                .GroupBy(x => x.Num, x => x.Count)
                .Select(x => (x.Key, x.Sum()));
            numsWithCounts = new LinkedList<(long Num, long Count)>(newNumsWithCounts);
        }

        return numsWithCounts.Sum(x => x.Count);
    }

    public static IEnumerable<long> PerformBlink(long data)
    {
        switch (data)
        {
            case 0:
                {
                    yield return 1;
                    break;
                }
            case var x when x.ToString().Length % 2 == 0:
                {
                    var str = x.ToString();
                    yield return long.Parse(str[..(str.Length / 2)]);
                    yield return long.Parse(str[(str.Length / 2)..]);
                    break;
                }
            default:
                {
                    yield return data * 2024;
                    break;
                }
        }
    }

    private static long[] GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        return sr.ReadLine()
            .Split(" ")
            .Select(long.Parse)
            .ToArray();

    }
}
