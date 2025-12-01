namespace AdventOfCode2023;

internal class Day8
{
    public static object SolveP1(string fileName)
    {
        var (instr, data) = GetInput(fileName);
        var steps = 0;
        var curr = "AAA";
        while (curr != "ZZZ")
        {
            var (key, left, right) = data.First(x => x.Key == curr);
            curr = instr[steps % instr.Length] == 'R' ? right : left;
            ++steps;
        }
        return steps;
    }

    // WARNING: input dependent solution (sets should be periodic and only of a form: a + a * k)
    public static object SolveP2(string fileName)
    {
        var (instr, data) = GetInput(fileName);
        var starts = data.Where(x => x.Key[2] == 'A').Select(x => x.Key).ToArray();
        var lists = new List<(int, List<(int, string)>)>();
        foreach (var start in starts)
        {
            var curr = start;
            var set = new HashSet<(int, string)>();
            var i = 0;
            while (true)
            {
                var (key, left, right) = data.First(x => x.Key == curr);
                curr = instr[i % instr.Length] == 'R' ? right : left;
                var pathItem = (i % instr.Length, curr);
                if (set.Contains(pathItem))
                {
                    var list = set.ToList();
                    lists.Add((list.IndexOf(pathItem), list));
                    break;
                }
                set.Add(pathItem);

                ++i;
            }
        }

        var forms = lists.Select(x => {
            var starts = x.Item2.Where(y => y.Item2[2] == 'Z').Select(y =>
            {
                return x.Item2.IndexOf(y) + 1;
            })
            .ToArray();

            return (Starts: starts, Period: x.Item2.Count - x.Item1);
        })
        .ToArray();

        var steps = 1L;
        foreach (var form in forms)
        {
            steps = lcm(steps, form.Period);
        }

        return steps;
    }

    private static (string Instr, IEnumerable<(string Key, string Left, string Right)> Data) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);
        var instr = sr.ReadLine();
        sr.ReadLine();
        var data = new List<(string, string, string)>();
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var a = line.Split(" = ");
            var lr = a[1].Trim('(', ')').Split(", ");
            data.Add((a[0], lr[0], lr[1]));
        }

        return (instr, data);
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

    static long lcm(long a, long b)
    {
        return (a / gcd(a, b)) * b;
    }
}
