namespace AdventOfCode2023;

internal class Day4
{
    public static object SolveP1(string fileName) {
        return GetInput(fileName)
            .Select(x => {
                var inter = x.nums.Intersect(x.winning).Count();
                return inter != 0 ? Math.Pow(2, inter - 1) : 0;
            })
            .Sum();
    }

    public static object SolveP2(string fileName) {
        var cards = GetInput(fileName)
            .Select(x => (val: x.nums.Intersect(x.winning).Count(), mult: 1))
            .ToArray();

        for(var i = 0; i < cards.Length; ++i)
        {
            for(var k = 1;  k <= cards[i].val; ++k)
            {
                cards[i + k] = (cards[i + k].val, cards[i + k].mult + cards[i].mult);
            }
        }

        return cards.Sum(x => x.mult);
    }

    private static IEnumerable<(HashSet<int> winning, HashSet<int> nums)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            var a = line.Split(": ")[^1]
                .Split(" | ")
                .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)))
                .ToArray();

            yield return (a[0].ToHashSet(), a[1].ToHashSet());
        }
    }
}
