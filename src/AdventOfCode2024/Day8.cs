namespace AdventOfCode2024;

internal class Day8
{
    public static object SolveP1(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();

        var antennas = Enumerable.Range(0, field.Length)
            .SelectMany(i => Enumerable.Range(0, field[i].Length).Select(j => (i, j)))
            .Where(x => field[x.i][x.j] != '.')
            .GroupBy(x => field[x.i][x.j])
            .ToDictionary(x => x.Key, x => x.ToArray());

        var res = 0L;

        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                res += IsAntinode((i, j), antennas) ? 1 : 0;
            }
        }

        return res;
    }

    private static bool IsAntinode((int, int) point, Dictionary<char, (int, int)[]> antennas)
    {
        var (i, j) = point;
        foreach (var key in antennas.Keys)
        {
            var antennasData = antennas[key].Where(x => x.Item1 != i || x.Item2 != j)
                .ToArray();

            foreach (var data in antennasData)
            {
                var d1 = data.Item1 - i;
                var d2 = data.Item2 - j;

                if (antennasData.Any(x => x.Item1 == i + 2 * d1 && x.Item2 == j + 2 * d2))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static object SolveP2(string fileName)
    {
        var field = GetInput(fileName)
            .ToArray();

        var antennas = Enumerable.Range(0, field.Length)
            .SelectMany(i => Enumerable.Range(0, field[i].Length).Select(j => (i, j)))
            .Where(x => field[x.i][x.j] != '.')
            .GroupBy(x => field[x.i][x.j])
            .ToDictionary(x => x.Key, x => x.ToArray());

        return GetAntinodes(antennas, field.Length - 1, field[0].Length - 1)
            .Distinct()
            .Count();
    }

    private static IEnumerable<(int, int)> GetAntinodes(Dictionary<char, (int, int)[]> antennas, long maxI, long maxJ)
    {
        foreach (var key in antennas.Keys)
        {
            var antennasData = antennas[key];
            for (var i = 0; i < antennasData.Length; ++i)
            {
                for (var j = i + 1; j < antennasData.Length; ++j)
                {
                    var (antenna1, antenna2) = (antennasData[i], antennasData[j]);
                    var (d1, d2) = (antenna2.Item1 - antenna1.Item1, antenna2.Item2 - antenna1.Item2);
                    var divisor = gcd(Math.Abs(d1), Math.Abs(d2));

                    (d1, d2) = (d1 / divisor, d2 / divisor);
                    for (var point = antenna1;
                        point.Item1 >= 0 && point.Item1 <= maxI && point.Item2 >= 0 && point.Item2 <= maxJ;
                        point = (point.Item1 + d1, point.Item2 + d2))
                    {
                        yield return point;
                    }

                    (d1, d2) = (-d1, -d2);
                    for (var point = antenna1;
                        point.Item1 >= 0 && point.Item1 <= maxI && point.Item2 >= 0 && point.Item2 <= maxJ;
                        point = (point.Item1 + d1, point.Item2 + d2))
                    {
                        yield return point;
                    }
                }
            }
        }
    }

    static int gcd(int a, int b)
    {
        while (b != 0L)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static IEnumerable<char[]> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return sr.ReadLine().ToCharArray();
        }
    }
}
