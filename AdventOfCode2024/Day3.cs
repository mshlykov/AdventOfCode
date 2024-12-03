namespace AdventOfCode2024;

internal class Day3
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName).AsSpan();
        var res = 0L;

        while (input.Length != 0)
        {
            var currPair = GetNextPair(ref input);
            if (currPair != null)
            {
                res += currPair.Value.Item1 * currPair.Value.Item2;
            }
        }
        return res;
    }

    private static (long, long)? GetNextPair(ref ReadOnlySpan<char> input)
    {
        var mulIdx = input.IndexOf("mul(");
        if (mulIdx == -1)
        {
            input = string.Empty;
            return null;
        }

        input = input.Slice(mulIdx + 4);

        var commaIdx = input.IndexOf(",");
        if (commaIdx == -1)
        {
            return null;
        }

        if (!long.TryParse(input.Slice(0, commaIdx), out var item1) || commaIdx == input.Length - 1)
        {
            return null;
        }

        input = input.Slice(commaIdx + 1);

        var closingBracketIdx = input.IndexOf(")");
        if (closingBracketIdx == -1)
        {
            return null;
        }

        if (!long.TryParse(input.Slice(0, closingBracketIdx), out var item2))
        {
            return null;
        }

        input = closingBracketIdx == input.Length - 1 ? string.Empty : input.Slice(closingBracketIdx + 1);

        return (item1, item2);
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName).AsSpan();
        var res = 0L;

        while (input.Length != 0)
        {
            var currPair = GetNextPairWithDosAndDonts(ref input);
            if (currPair != null)
            {
                res += currPair.Value.Item1 * currPair.Value.Item2;
            }
        }

        return res;
    }

    private static (long, long)? GetNextPairWithDosAndDonts(ref ReadOnlySpan<char> input)
    {
        var (dontIdx, mulIdx) = (input.IndexOf("don't()"), input.IndexOf("mul("));

        dontIdx = dontIdx == -1 ? int.MaxValue : dontIdx;
        mulIdx = mulIdx == -1 ? int.MaxValue : mulIdx;

        if (dontIdx != int.MaxValue && Math.Min(dontIdx, mulIdx) == dontIdx)
        {
            input = dontIdx + 7 == input.Length ? string.Empty : input.Slice(dontIdx + 7);
            var doIdx = input.IndexOf("do()");
            if (doIdx == -1)
            {
                input = string.Empty;
                return null;
            }

            input = doIdx + 4 == input.Length ? string.Empty : input.Slice(doIdx + 4);
            return null;
        }

        if (mulIdx != int.MaxValue && Math.Min(dontIdx, mulIdx) == mulIdx)
        {
            return GetNextPair(ref input);
        }

        input = string.Empty;
        return null;
    }

    private static string GetInput(string fileName) => File.ReadAllText(fileName);
}
