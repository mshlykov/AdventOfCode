namespace AdventOfCode
{
    internal class BlizzardBasin
    {
        private static (int X, int Y)[] _options = new[]
        {
            (0, 0), (1, 0), (-1, 0), (0, 1), (0, -1)
        };

        public static long SolveP1(string fileName)
        {
            var (maxX, maxY, blizzards) = GetInput(fileName);
            var (start, end) = ((1, 0), (maxX - 1, maxY));
            var precomputedBlizzards = PrecomputeBlizzards(blizzards, maxX, maxY, Lcm(maxX - 1, maxY - 1));
            var res = Solve(start, end, 0, precomputedBlizzards, maxX, maxY);
            return res;
        }

        public static long SolveP2(string fileName)
        {
            var (maxX, maxY, blizzards) = GetInput(fileName);
            var (start, end) = ((1, 0), (maxX - 1, maxY));
            var precomputedBlizzards = PrecomputeBlizzards(blizzards, maxX, maxY, Lcm(maxX - 1, maxY - 1));
            var res = Solve(start, end, 0L, precomputedBlizzards, maxX, maxY);
            res = Solve(end, start, res, precomputedBlizzards, maxX, maxY);
            res = Solve(start, end, res, precomputedBlizzards, maxX, maxY);
            return res;
        }

        private static long Solve((int X, int Y) start, (int X, int Y) end, long startRound, HashSet<(int X, int Y)>[] blizzards, int maxX, int maxY)
        {
            var positions = new HashSet<(int X, int Y)> { start };
            var i = startRound;
            while (!positions.Contains(end))
            {
                var set = blizzards[(i + 1) % blizzards.Length];
                positions = positions.SelectMany(x => _options.Select(y => (X: x.X + y.X, Y: x.Y + y.Y)))
                    .Where(x => x == start || x == end || !set.Contains(x) && x.X > 0 && x.X < maxX && x.Y > 0 && x.Y < maxY)
                    .ToHashSet();
                ++i;
            }

            return i;
        }

        private static (int X, int Y) GetNewBlizzardPos(int maxX, int maxY, char dir, (int X, int Y) currPos)
        {
            var newPos = dir switch
            {
                '>' => (currPos.X + 1, currPos.Y),
                'v' => (currPos.X, currPos.Y + 1),
                '<' => (currPos.X - 1, currPos.Y),
                '^' => (currPos.X, currPos.Y - 1),
                _ => throw new InvalidOperationException()
            };

            newPos = newPos switch
            {
                var (x, y) when x == maxX => (1, y),
                var (x, y) when x == 0 => (maxX - 1, y),
                var (x, y) when y == maxY => (x, 1),
                var (x, y) when y == 0 => (x, maxY - 1),
                _ => newPos
            };

            return newPos;
        }

        private static HashSet<(int X, int Y)>[] PrecomputeBlizzards((char Dir, (int X, int Y) Position)[] blizzards, int maxX, int maxY, int blizzardsPeriod)
        {
            var res = new HashSet<(int X, int Y)>[blizzardsPeriod];
            res[0] = blizzards.Select(x => x.Position)
                .ToHashSet();
            for (var i = 1; i < res.Length; ++i)
            {
                blizzards = blizzards.Select(x => (x.Dir, Position: GetNewBlizzardPos(maxX, maxY, x.Dir, x.Position)))
                .ToArray();
                res[i] = blizzards.Select(x => x.Position)
                    .ToHashSet();
            }

            return res;
        }

        static int Lcm(int a, int b)
        {
            var (tempA, tempB) = (a, b);
            while (tempB != 0)
            {
                int temp = tempB;
                tempB = tempA % tempB;
                tempA = temp;
            }

            return a * b / tempA;
        }

        private static (int MaxX, int MaxY, (char Dir, (int X, int Y))[] Blizzards) GetInput(string fileName)
        {
            var lines = File.ReadAllText(fileName)
                .Split(Environment.NewLine);
            var maxY = lines.Length - 1;
            var maxX = lines[0].Length - 1;
            var blizzards = lines.SelectMany((x, i) => x.Select((y, idx) => (y, idx))
                .Where(y => y.y != '#' && y.y != '.')
                .Select(y => (y.y, (y.idx, i))))
                .ToArray();
            return (maxX, maxY, blizzards);
        }
    }
}
