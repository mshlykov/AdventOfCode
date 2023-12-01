namespace AdventOfCode2022
{
    internal class RopeBridge
    {
        public static int SolveP1(string fileName) => GetTailPositions(GetInput(fileName), 2).Count;

        public static int SolveP2(string fileName) => GetTailPositions(GetInput(fileName), 10).Count;

        private static HashSet<(int, int)> GetTailPositions(IEnumerable<(string, int)> moves, int knotsCount)
        {
            var set = new HashSet<(int, int)> { (0, 0) };
            var knots = new (int, int)[knotsCount];

            foreach (var (direction, stepsCount) in moves)
            {
                for (var i = 0; i < stepsCount; ++i)
                {
                    var (headX, headY) = knots[0];
                    knots[0] = direction switch
                    {
                        "U" => (headX, headY + 1),
                        "D" => (headX, headY - 1),
                        "R" => (headX + 1, headY),
                        "L" => (headX - 1, headY),
                        _ => throw new InvalidOperationException()
                    };

                    for (var j = 1; j < knots.Length; ++j)
                    {
                        var ((prevX, prevY), (currX, currY)) = (knots[j - 1], knots[j]);
                        if (Math.Abs(prevX - currX) > 1 || Math.Abs(prevY - currY) > 1)
                        {
                            knots[j] = (currX + Math.Sign(prevX - currX), currY + Math.Sign(prevY - currY));
                        }
                    }

                    set.Add(knots[knots.Length - 1]);
                }
            }

            return set;
        }

        private static IEnumerable<(string, int)> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Split(" ").ToArray();
                yield return (line[0], int.Parse(line[1]));
            }
        }
    }
}
