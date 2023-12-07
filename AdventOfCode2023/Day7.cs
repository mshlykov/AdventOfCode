namespace AdventOfCode2023;

internal class Day7
{
    private static readonly Dictionary<char, int> _ordering = new() {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'J', 10},
        {'T', 9},
        {'9', 8},
        {'8', 7},
        {'7', 6},
        {'6', 5},
        {'5', 4},
        {'4', 3},
        {'3', 2},
        {'2', 1},
    };

    private static readonly Dictionary<char, int> _ordering2 = new() {
        {'A', 13},
        {'K', 12},
        {'Q', 11},
        {'T', 10},
        {'9', 9},
        {'8', 8},
        {'7', 7},
        {'6', 6},
        {'5', 5},
        {'4', 4},
        {'3', 3},
        {'2', 2},
        {'J', 1},
    };

    private enum Hand
    {
        HighCard,
        Pair,
        TwoPair,
        Three,
        FullHouse,
        Four,
        Five
    }

    public static object SolveP1(string fileName) => Solve(fileName, false);

    public static object SolveP2(string fileName) => Solve(fileName, true);

    private static long Solve(string fileName, bool withJoker)
    {
        var input = GetInput(fileName)
            .Select(x => (x.Cards, Hand: GetHand(x.Cards, withJoker), x.Bid))
            .ToArray();
        var ordering = withJoker ? _ordering2 : _ordering;

        Array.Sort(input, (first, second) =>
        {
            var handsComparison = first.Hand.CompareTo(second.Hand);
            if (handsComparison != 0)
            {
                return handsComparison;
            }

            foreach (var (firstChar, secondChar) in first.Cards.Zip(second.Cards))
            {
                var orderingCoparison = ordering[firstChar].CompareTo(ordering[secondChar]);
                if (orderingCoparison != 0)
                {
                    return orderingCoparison;
                }
            }

            return 0;
        });

        return input.Select((x, i) => x.Bid * (i + 1))
            .Sum();
    }

    private static Hand GetHand(string cards, bool withJoker)
    {
        if (withJoker && cards == "JJJJJ")
        {
            return Hand.Five;
        }

        var filteredCards = withJoker ? cards.Where(x => x != 'J') : cards;
        var groupsSizes = filteredCards.GroupBy(x => x, (key, x) => x.Count())
            .OrderByDescending(x => x)
            .ToArray();

        groupsSizes[0] += withJoker ? cards.Count(x => x == 'J') : 0;

        return groupsSizes switch
        {
            _ when groupsSizes[0] == 5 => Hand.Five,
            _ when groupsSizes[0] == 4 => Hand.Four,
            _ when groupsSizes[0] == 3 && groupsSizes[1] == 2 => Hand.FullHouse,
            _ when groupsSizes[0] == 3 => Hand.Three,
            _ when groupsSizes[0] == 2 && groupsSizes[1] == 2 => Hand.TwoPair,
            _ when groupsSizes[0] == 2 => Hand.Pair,
            _ => Hand.HighCard
        };
    }

    private static IEnumerable<(string Cards, long Bid)> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            var lines = sr.ReadLine().Split(" ");

            yield return (lines[0], long.Parse(lines[1]));
        }
    }
}
