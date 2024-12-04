using System.Reflection;

namespace AdventOfCode2024;

internal class Day4
{
    private static char[] Xmas = [ 'X', 'M', 'A', 'S' ];
    private static char[][] Mas = [
        [ 'A', 'M', 'M', 'S', 'S' ],
        [ 'A', 'M', 'S', 'S', 'M' ],
        [ 'A', 'S', 'S', 'M', 'M' ],
        [ 'A', 'S', 'M', 'M', 'S' ]
    ];

    public static object SolveP1(string fileName) {
        var data = GetInput(fileName).ToArray();
        var res = 0L;
        for (var i = 0; i < data.Length; ++i)
        {
            for (var j = 0; j < data[i].Length; ++j)
            {
                res += CountXmas(data, (i, j));
            }
        }

        return res;
    }

    private static long CountXmas(string[] data, (int, int) point)
    {
        var (i, j) = point;
        var leftAvailable = i > 2;
        var rightAvailable = i < data.Length - 3;
        var topAvailable = j > 2;
        var bottomAvailable = j < data[i].Length - 3;

        var res = 0;

        res += leftAvailable && new[] { data[i][j], data[i - 1][j], data[i - 2][j], data[i - 3][j] }.SequenceEqual(Xmas) ? 1 : 0;
        res += leftAvailable && topAvailable && new[] { data[i][j], data[i - 1][j - 1], data[i - 2][j - 2], data[i - 3][j - 3] }.SequenceEqual(Xmas) ? 1 : 0;
        res += leftAvailable && bottomAvailable && new[] { data[i][j], data[i - 1][j + 1], data[i - 2][j + 2], data[i - 3][j + 3] }.SequenceEqual(Xmas) ? 1 : 0;
        res += rightAvailable && new[] { data[i][j], data[i + 1][j], data[i + 2][j], data[i + 3][j] }.SequenceEqual(Xmas) ? 1 : 0;
        res += rightAvailable && topAvailable && new[] { data[i][j], data[i + 1][j - 1], data[i + 2][j - 2], data[i + 3][j - 3] }.SequenceEqual(Xmas) ? 1 : 0;
        res += rightAvailable && bottomAvailable && new[] { data[i][j], data[i + 1][j + 1], data[i + 2][j + 2], data[i + 3][j + 3] }.SequenceEqual(Xmas) ? 1 : 0;
        res += bottomAvailable && new[] { data[i][j], data[i][j + 1], data[i][j + 2], data[i][j + 3] }.SequenceEqual(Xmas) ? 1 : 0;
        res += topAvailable && new[] { data[i][j], data[i][j - 1], data[i][j - 2], data[i][j - 3] }.SequenceEqual(Xmas) ? 1 : 0;

        return res;
    }

    public static object SolveP2(string fileName)
    {
        var data = GetInput(fileName).ToArray();
        var res = 0L;
        for (var i = 0; i < data.Length; ++i)
        {
            for (var j = 0; j < data[i].Length; ++j)
            {
                res += IsMas(data, (i, j)) ? 1 : 0;
            }
        }

        return res;
    }

    private static bool IsMas(string[] data, (int, int) point)
    {
        var (i, j) = point;
        if (i < 1 || i > data.Length - 2 || j < 1 || j > data[0].Length - 2)
        {
            return false;
        }

        var chars = new[] { data[i][j], data[i - 1][j - 1], data[i - 1][j + 1], data[i + 1][j + 1], data[i + 1][j - 1] };
        return Mas.Any(x => x.SequenceEqual(chars));
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
