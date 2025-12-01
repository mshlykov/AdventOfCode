namespace AdventOfCode2024;

internal class Day13
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName)
            .Select(x =>
            {
                var (v1, v2, v3) = x;
                var gcd1 = gcd(v1.Item1, v2.Item1);
                var gcd2 = gcd(v1.Item2, v2.Item2);
                return ((v1.Item1 / gcd1, v1.Item2 / gcd2), (v2.Item1 / gcd1, v2.Item2 / gcd2), (v3.Item1 / gcd1, v3.Item2 / gcd2));
            })
            .ToArray();
        // the solution works as there are no systems that have one or infinite number of solutions, so Cramer's rule works
        // otherwise BruteForce would have to be used
        return input.Select(x =>
        {
            var (v1, v2, v3) = x;
            var d = v1.Item1 * v2.Item2 - v1.Item2 * v2.Item1;
            var dx = v3.Item1 * v2.Item2 - v3.Item2 * v2.Item1;
            var dy = v1.Item1 * v3.Item2 - v1.Item2 * v3.Item1;
            return (d: d, dx: dx, dy: dy, x: dx / d, resx: dx % d, y: dy / d, resy: dy % d);
        })
            .Where(x => x.resx == 0 && x.resy == 0 && x.x >= 0 && x.x <= 100 && x.y >= 0 && x.x <= 100)
            .Sum(x => 3 * x.x + x.y);
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName)
           .Select(x =>
           {
               var (v1, v2, v3) = x;
               var gcd1 = gcd(v1.Item1, v2.Item1);
               var gcd2 = gcd(v1.Item2, v2.Item2);
               return ((v1.Item1 / gcd1, v1.Item2 / gcd2), (v2.Item1 / gcd1, v2.Item2 / gcd2), ((v3.Item1 + 10000000000000L) / gcd1, (v3.Item2 + 10000000000000L) / gcd2));
           })
           .ToArray();

        // the solution works as there are no systems that have one or infinite number of solutions, so Cramer's rule works
        // otherwise BruteForce would have to be used
        return input.Select(x =>
            {
                var (v1, v2, v3) = x;
                var d = v1.Item1 * v2.Item2 - v1.Item2 * v2.Item1;
                var dx = v3.Item1 * v2.Item2 - v3.Item2 * v2.Item1;
                var dy = v1.Item1 * v3.Item2 - v1.Item2 * v3.Item1;
                return (d: d, dx: dx, dy: dy, x: dx / d, resx: dx % d, y: dy / d, resy: dy % d);
            })
            .Where(x => x.resx == 0 && x.resy == 0 && x.x >= 0 && x.y >= 0)
            .Sum(x => 3 * x.x + x.y);
    }

    private static long BruteForce(IEnumerable<((long, long), (long, long), (long, long))> input)
    {
        var res = 0L;

        foreach (var (v1, v2, v3) in input)
        {
            var solution = FindSolution(v1.Item1, v2.Item1, v3.Item1);
            if (solution == null)
            {
                continue;
            }

            var cost = long.MaxValue;
            var (optX, optY) = (-1L, -1L);
            for (var (x, y) = solution.Value; x >= 0 && x <= 100; x += v2.Item1, y -= v1.Item1)
            {
                if (y >= 0 && y <= 100 && x * v1.Item2 + y * v2.Item2 == v3.Item2)
                {
                    if (3 * x + y < cost)
                    {
                        (optX, optY) = (x, y);
                        cost = 3 * x + y;
                    }
                }
            }

            for (var (x, y) = solution.Value; x >= 0 && x <= 100; x -= v2.Item1, y += v1.Item1)
            {
                if (y >= 0 && y <= 100 && x * v1.Item2 + y * v2.Item2 == v3.Item2)
                {
                    if (3 * x + y < cost)
                    {
                        (optX, optY) = (x, y);
                        cost = 3 * x + y;
                    }
                }
            }

            res += cost == long.MaxValue ? 0 : cost;
        }

        return res;
    }

    private static (long, long)? FindSolution(long a, long b, long c)
    {
        for (var k = 0; k <= c / a; ++k)
        {
            if ((c - k * a) % b == 0 && (c - k * a) / b >= 0)
            {
                return (k, (c - k * a) / b);
            }
        }

        return null;
    }

    static long gcd(long a, long b)
    {
        while (b != 0L)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static IEnumerable<((long, long), (long, long), (long, long))> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var nums = line[10..].Split(", ")
                .Select(x => long.Parse(x[2..]))
                .ToArray();
            var v1 = (nums[0], nums[1]);
            line = sr.ReadLine();
            nums = line[10..].Split(", ")
                .Select(x => long.Parse(x[2..]))
                .ToArray();
            var v2 = (nums[0], nums[1]);
            line = sr.ReadLine();
            nums = line[7..].Split(", ")
                .Select(x => long.Parse(x[2..]))
                .ToArray();
            var v3 = (nums[0], nums[1]);
            sr.ReadLine();
            yield return (v1, v2, v3);
        }
    }
}
