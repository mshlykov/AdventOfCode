namespace AdventOfCode2023;

internal class Day13
{
    public static object SolveP1(string fileName) => GetInput(fileName)
        .Select(x => (GetHorNote(x).FirstOrDefault(), GetVertNote(x).FirstOrDefault()))
        .Sum(x => x.Item1 != 0 ? 100L * x.Item1 : x.Item2);

    public static object SolveP2(string fileName) => GetInput(fileName)
        .Select(x => GetWithFixedSmudge(x).FirstOrDefault())
        .Sum(x => x.Item1 != 0 ? 100L * x.Item1 : x.Item2);

    private static IEnumerable<long> GetHorNote(string[] pattern)
    {
        for (var i = 0; i < pattern.Length - 1; ++i)
        {
            var j = 0;
            for (; i + j + 1 < pattern.Length && i - j >= 0 && pattern[i + j + 1] == pattern[i - j]; ++j)
            {
            }

            if (i - j < 0 || i + j + 1 >= pattern.Length)
            {
                yield return i + 1;
                continue;
            }
        }
    }

    private static IEnumerable<long> GetVertNote(string[] pattern)
    {
        for (var i = 0; i < pattern[0].Length - 1; ++i)
        {
            var j = 0;
            for (; i + j + 1 < pattern[0].Length && i - j >= 0; ++j)
            {
                var match = true;
                for (var k = 0; k < pattern.Length; ++k)
                {
                    if (pattern[k][i + j + 1] != pattern[k][i - j])
                    {
                        match = false;
                        break;
                    }
                }

                if (!match)
                {
                    break;
                }
            }

            if (i - j < 0 || i + j + 1 >= pattern[0].Length)
            {
                yield return i + 1;
                continue;
            }
        }
    }

    private static IEnumerable<(long, long)> GetWithFixedSmudge(string[] pattern)
    {
        var init = (GetHorNote(pattern).FirstOrDefault(), GetVertNote(pattern).FirstOrDefault());
        for (var i = 0; i < pattern.Length; ++i)
        {
            for (var j = 0; j < pattern[i].Length; ++j)
            {
                var stringToReplace = $"{pattern[i][..j]}{(pattern[i][j] == '#' ? "." : "#")}{pattern[i][(j + 1)..]}";
                var newPattern = pattern[..i]
                    .Concat(new[] { stringToReplace })
                    .Concat(pattern[(i + 1)..])
                    .ToArray();
                var pair = (GetHorNote(newPattern).FirstOrDefault(x => x != init.Item1), GetVertNote(newPattern).FirstOrDefault(x => x != init.Item2));

                if (pair != (0, 0))
                {
                    yield return pair;
                }
            }
        }
    }

    private static IEnumerable<string[]> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var list = new List<string>();
            var line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                list.Add(line);
                line = sr.ReadLine();
            }
            yield return list.ToArray();
        }
    }
}
