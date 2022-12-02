namespace AdventOfCode
{
    internal class RockPaperScissors
    {
        public static int SolveP1(string fileName) => GetMoves(fileName)
            .Select(x => GetScoreP1(x.Move, x.Strategy))
            .Sum();

        public static int SolveP2(string fileName) => GetMoves(fileName)
            .Select(x => GetScoreP2(x.Move, x.Strategy))
            .Sum();

        private static IEnumerable<(string Move, string Strategy)> GetMoves(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                if (line is not null)
                {
                    var pair = line.Split(' ');

                    yield return (pair[0], pair[1]);
                }
            }
        }

        private static int GetBaseScore(string move) => move switch
        {
            "A" => 1,
            "B" => 2,
            "C" => 3,
            _ => throw new InvalidOperationException()
        };

        public static string GetWinningMove(string move) => move switch
        {
            "A" => "B",
            "B" => "C",
            "C" => "A",
            _ => throw new InvalidOperationException()
        };

        public static string GetLosingMove(string move) => move switch
        {
            "A" => "C",
            "B" => "A",
            "C" => "B",
            _ => throw new InvalidOperationException()
        };

        private static int GetScoreP1(string move, string strategy)
        {
            var counterMove = strategy switch
            {
                "X" => "A",
                "Y" => "B",
                "Z" => "C",
                _ => throw new InvalidOperationException()
            };

            var matchScore = (move, counterMove) switch
            {
                { move: var x, counterMove: var y } when (GetWinningMove(y) == x) => 0,
                { move: var x, counterMove: var y } when (GetLosingMove(y) == x) => 6,
                { move: _, counterMove: _ } => 3
            };

            return GetBaseScore(counterMove) + matchScore;
        }

        private static int GetScoreP2(string move, string strategy)
        {
            var (counterMove, matchScore) = strategy switch
            {
                "X" => (GetLosingMove(move), 0),
                "Y" => (move, 3),
                "Z" => (GetWinningMove(move), 6),
                _ => throw new InvalidOperationException()
            };

            return GetBaseScore(counterMove) + matchScore;
        }
    }
}
