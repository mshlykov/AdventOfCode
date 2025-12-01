namespace AdventOfCode2022
{
    internal class BoilingBoulders
    {
        public static int SolveP1(string fileName)
        {
            var set = GetInput(fileName)
                .ToHashSet();
            var area = 0;
            foreach (var p in set)
            {
                area += 6 - GetAdjacentCells(p).Count(x => set.Contains(x));
            }
            return area;
        }

        public static int SolveP2(string fileName)
        {
            var set = GetInput(fileName)
                .ToHashSet();
            var area = 0;
            foreach (var p in set)
            {
                var adjacentCells = GetAdjacentCells(p);
                var occupiedCells = adjacentCells.Where(x => set.Contains(x)).ToArray();
                area += 6 - occupiedCells.Length - adjacentCells.Except(occupiedCells).Count(x => IsInternal(x, set));
            }
            return area;
        }

        private static (int, int, int)[] GetAdjacentCells((int X, int Y, int Z) p) => new[] {
            (p.X + 1, p.Y, p.Z),
            (p.X - 1, p.Y, p.Z),
            (p.X , p.Y + 1, p.Z),
            (p.X , p.Y - 1, p.Z),
            (p.X , p.Y, p.Z + 1),
            (p.X , p.Y, p.Z - 1)
        };

        private static bool IsInternal((int X, int Y, int Z) p, HashSet<(int X, int Y, int Z)> set) => IsInternalRec(p, set, new HashSet<(int X, int Y, int Z)>());

        private static bool IsInternalRec((int X, int Y, int Z) p, HashSet<(int X, int Y, int Z)> set, HashSet<(int X, int Y, int Z)> visitedSet)
        {
            visitedSet.Add(p);
            return set.Any(x => x.X >= p.X && x.Y == p.Y && x.Z == p.Z) && set.Any(x => x.X <= p.X && x.Y == p.Y && x.Z == p.Z)
                && set.Any(x => x.X == p.X && x.Y >= p.Y && x.Z == p.Z) && set.Any(x => x.X == p.X && x.Y <= p.Y && x.Z == p.Z)
                && set.Any(x => x.X == p.X && x.Y == p.Y && x.Z >= p.Z) && set.Any(x => x.X == p.X && x.Y == p.Y && x.Z <= p.Z)
                && GetAdjacentCells(p).Where(x => !set.Contains(x) && !visitedSet.Contains(x)).All(x => IsInternalRec(x, set, visitedSet));
        }

        private static IEnumerable<(int X, int Y, int Z)> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var coords = sr.ReadLine().Split(",").ToArray();
                yield return (int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]));
            }
        }
    }
}