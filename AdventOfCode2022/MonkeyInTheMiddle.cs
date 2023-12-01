namespace AdventOfCode2022
{
    internal class MonkeyInTheMiddle
    {
        public static long SolveP1(string fileName) => Solve(GetInput(fileName), 20, x => x / 3);

        public static long SolveP2(string fileName)
        {
            var input = GetInput(fileName);
            var divisor = input.Aggregate(1, (res, x) => res * x.Item3);
            return Solve(input, 10000, x => x % divisor);
        }

        private static long Solve(IEnumerable<(List<long>, string[], int, int, int)> input, int roundsCount, Func<long, long> reducer)
        {
            var monkeys = input.Select(x => (x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, 0L)).ToArray();

            for (var i = 0; i < roundsCount; ++i)
            {
                for (var j = 0; j < monkeys.Length; ++j)
                {
                    var (items, expression, divisor, monkeyIftrue, monkeyIfFalse, inspectedItemsCount) = monkeys[j];
                    foreach (var item in items)
                    {
                        var newCon = reducer(GetAnxiety(item, expression));
                        if (newCon % divisor == 0)
                        {
                            monkeys[monkeyIftrue].Item1.Add(newCon);
                        }
                        else
                        {
                            monkeys[monkeyIfFalse].Item1.Add(newCon);
                        }
                    }

                    monkeys[j] = (new List<long>(), expression, divisor, monkeyIftrue, monkeyIfFalse, inspectedItemsCount + items.Count);
                }
            }

            var sorted = monkeys.OrderByDescending(x => x.Item6).ToList();

            return sorted[0].Item6 * sorted[1].Item6;
        }

        private static long GetAnxiety(long old, string[] expression)
        {
            var op1 = expression[0] == "old" ? old : long.Parse(expression[0]);
            var op2 = expression[2] == "old" ? old : long.Parse(expression[2]);
            return expression[1] switch
            {
                "+" => op1 + op2,
                "*" => op1 * op2,
                _ => throw new Exception()
            };
        }

        private static IEnumerable<(List<long>, string[], int, int, int)> GetInput(string fileName)
        {
            var input = File.ReadAllText(fileName)
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(x =>
                {
                    var lines = x.Split(Environment.NewLine).ToArray();
                    return (
                    lines[1][18..].Split(", ").Select(z => long.Parse(z)).ToList(),
                    lines[2][19..].Split(" ").ToArray(),
                    int.Parse(lines[3][21..]),
                    int.Parse(lines[4][29..]),
                    int.Parse(lines[5][30..])
                    );
                });

            return input;
        }
    }
}
