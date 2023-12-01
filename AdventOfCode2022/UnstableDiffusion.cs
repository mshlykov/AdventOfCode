namespace AdventOfCode2022
{
    internal class UnstableDiffusion
    {
        private static (int X, int Y)[] _adjacentPositions = new[]
        {
            (-1, -1), (-1, 0), (-1, 1),
            (1, -1), (1, 0), (1, 1),
            (0, 1), (0, -1)
        };

        private static (int X, int Y)[][] _positionsToCheck = new[]
        {
            new[] { (-1, -1), (0, -1), (1, -1) },
            new[] { (1, 1), (0, 1), (-1, 1) },
            new[] { (-1, 1), (-1, 0), (-1, -1) },
            new[] { (1, -1), (1, 0), (1, 1) }
        };

        public static long SolveP1(string fileName)
        {
            var elfs = GetInput(fileName).ToHashSet();
            for (var i = 0; i < 10; ++i)
            {
                var positions = elfs.Select(x => (CurrPosition: x, NewPosition: GetNewPosition(x, i, elfs)))
                    .Where(x => x.CurrPosition != x.NewPosition)
                    .GroupBy(x => x.NewPosition)
                    .Where(x => x.Count() == 1)
                    .Select(x => x.First())
                    .ToArray();

                foreach (var (currPosition, newPosition) in positions)
                {
                    elfs.Remove(currPosition);
                    elfs.Add(newPosition);
                }
            }

            var (minX, maxX, minY, maxY) = elfs.Aggregate((MinX: int.MaxValue, MaxX: int.MinValue, MinY: int.MaxValue, MaxY: int.MinValue), (result, x) => (Math.Min(result.MinX, x.X), Math.Max(result.MaxX, x.X), Math.Min(result.MinY, x.Y), Math.Max(result.MaxY, x.Y)));
            return (maxX - minX + 1) * (maxY - minY + 1) - elfs.Count;
        }

        public static int SolveP2(string fileName)
        {
            var elfs = GetInput(fileName).ToHashSet();
            for (var i = 0; true; ++i)
            {
                var positions = elfs.Select(x => (CurrPosition: x, NewPosition: GetNewPosition(x, i, elfs)))
                    .Where(x => x.CurrPosition != x.NewPosition)
                    .GroupBy(x => x.Item2)
                    .Where(x => x.Count() == 1)
                    .Select(x => x.First())
                    .ToArray();

                foreach (var (currPosition, newPosition) in positions)
                {
                    elfs.Remove(currPosition);
                    elfs.Add(newPosition);
                }

                if (!positions.Any())
                {
                    return i + 1;
                }
            }
        }

        private static (int X, int Y) GetNewPosition((int X, int Y) currPosition, int round, HashSet<(int, int)> positions)
        {
            if (_adjacentPositions.Any(x => positions.Contains((currPosition.X + x.X, currPosition.Y + x.Y))))
            {
                for (int j = 0; j < _positionsToCheck.Length; ++j)
                {
                    var idx = (round + j) % _positionsToCheck.Length;
                    if (_positionsToCheck[idx].All(x => !positions.Contains((currPosition.X + x.X, currPosition.Y + x.Y))))
                    {
                        return (currPosition.X + _positionsToCheck[idx][1].X, currPosition.Y + _positionsToCheck[idx][1].Y);
                    }
                }
            }

            return currPosition;
        }

        private static IEnumerable<(int X, int Y)> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            var i = 0;
            while (!sr.EndOfStream)
            {
                foreach (var (_, j) in sr.ReadLine().Select((x, j) => (x, j)).Where(x => x.x == '#'))
                    yield return (j, i);
                ++i;
            }
        }
    }
}