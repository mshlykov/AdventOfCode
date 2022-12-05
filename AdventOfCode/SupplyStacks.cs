namespace AdventOfCode
{
    internal class SupplyStacks
    {
        public static string SolveP1(string fileName)
        {
            var (stacks, moves) = GetInput(fileName);
            MoveP1(stacks, moves);
            return string.Join(string.Empty, stacks.Select(x => x.Peek()));
        }

        public static string SolveP2(string fileName)
        {
            var (stacks, moves) = GetInput(fileName);
            MoveP2(stacks, moves);
            return string.Join(string.Empty, stacks.Select(x => x.Peek()));
        }

        private static void MoveP1(Stack<char>[] stacks, IEnumerable<(int Amount, int From, int To)> moves)
        {
            foreach (var (amount, from, to) in moves)
            {
                for (var i = amount; i > 0; --i)
                {
                    stacks[to - 1].Push(stacks[from - 1].Pop());
                }
            }
        }

        private static void MoveP2(Stack<char>[] stacks, IEnumerable<(int Amount, int From, int To)> moves)
        {
            foreach (var (amount, from, to) in moves)
            {
                var stack = new Stack<char>();
                for (var i = amount; i > 0; --i)
                {
                    stack.Push(stacks[from - 1].Pop());
                }

                while (stack.Any())
                {
                    stacks[to - 1].Push(stack.Pop());
                }
            }
        }

        private static (Stack<char>[] Stacks, IEnumerable<(int Amount, int From, int To)> Moves) GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            return (GetStacks(sr), GetMoves(sr).ToArray());
        }

        private static IEnumerable<(int Amount, int From, int To)> GetMoves(StreamReader sr)
        {
            while (!sr.EndOfStream)
            {
                var pairs = sr.ReadLine()
                    .Split(" ");
                yield return (int.Parse(pairs[1]), int.Parse(pairs[3]), int.Parse(pairs[5]));
            }
        }

        private static Stack<char>[] GetStacks(StreamReader sr)
        {
            var lines = new List<string>();
            var line = sr.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                lines.Add(line);
                line = sr.ReadLine();
            }

            var stacksCount = lines.Last()
                .Trim()
                .Split("   ")
                .Length;
            var stacks = new Stack<char>[stacksCount];
            for (var i = 0; i < stacks.Length; ++i)
            {
                stacks[i] = new Stack<char>();
            }

            for (var i = lines.Count - 2; i >= 0; --i)
            {
                var items = lines[i].Select((item, idx) => (item, idx))
                .GroupBy(x => x.idx / 4, x => x.item)
                .Select(x => x.ElementAt(1))
                .ToArray();

                for (var j = 0; j < stacks.Length; ++j)
                {
                    if (items[j] != ' ')
                    {
                        stacks[j].Push(items[j]);
                    }
                }
            }

            return stacks;
        }
    }
}
