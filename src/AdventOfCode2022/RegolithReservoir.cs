namespace AdventOfCode2022
{
    internal class RegolithReservoir
    {
        public static int SolveP1(string fileName)
        {
            var set = new HashSet<(int X, int Y)>();
            var (xDict, yDict) = GetRocksLookups(GetInput(fileName));
            var maxY = yDict.Keys.Max();
            var pos = (X: 0, Y: 0);
            while (pos.Y < maxY)
            {
                pos = (500, 0);
                while (true)
                {
                    var options = new[] {
                        (X: pos.X, Y: pos.Y + 1),
                        (X: pos.X - 1, Y: pos.Y + 1),
                        (X: pos.X + 1, Y: pos.Y + 1),
                    };
                    var newPos = options.FirstOrDefault(x => !IsRock(xDict, yDict, x) && !set.Contains(x));
                    if (newPos == (0, 0))
                    {
                        set.Add(pos);
                        break;
                    }

                    pos = newPos;
                    if (newPos.Y >= maxY)
                    {
                        break;
                    }
                }
            }

            return set.Count;
        }

        public static int SolveP2(string fileName)
        {
            var set = new HashSet<(int X, int Y)>();
            var (xDict, yDict) = GetRocksLookups(GetInput(fileName));
            var maxY = yDict.Keys.Max();
            var pos = (X: 0, Y: 0);
            while (pos != (500, 0))
            {
                pos = (X: 500, Y: 0);
                while (true)
                {
                    var options = new[] {
                        (X: pos.X, Y: pos.Y + 1),
                        (X: pos.X - 1, Y: pos.Y + 1),
                        (X: pos.X + 1, Y: pos.Y + 1),
                    };
                    var newPos = options.FirstOrDefault(x => !IsRock(xDict, yDict, x) && !set.Contains(x));
                    if (newPos == (0, 0) || newPos.Y > maxY + 1)
                    {
                        set.Add(pos);
                        break;
                    }

                    pos = newPos;
                }
            }

            return set.Count;
        }

        public static bool IsRock(IDictionary<int, List<(int Start, int End)>> xDict, IDictionary<int, List<(int Start, int End)>> yDict, (int X, int Y) point) => xDict.ContainsKey(point.X) && xDict[point.X].Any(x => point.Y >= x.Start && point.Y <= x.End)
                    || yDict.ContainsKey(point.Y) && yDict[point.Y].Any(x => point.X >= x.Start && point.X <= x.End);

        private static (Dictionary<int, List<(int Start, int End)>>, Dictionary<int, List<(int Start, int End)>>) GetRocksLookups(IEnumerable<(int X, int Y)[]> paths)
        {
            var xDict = new Dictionary<int, List<(int, int)>>();
            var yDict = new Dictionary<int, List<(int, int)>>();
            foreach (var path in paths)
            {
                for (var i = 1; i < path.Length; ++i)
                {
                    var (currX, currY) = path[i];
                    var (prevX, prevY) = path[i - 1];
                    if (currX == prevX)
                    {
                        if (!xDict.ContainsKey(currX))
                        {
                            xDict.Add(currX, new List<(int, int)>());
                        }
                        xDict[currX].Add((Math.Min(currY, prevY), Math.Max(currY, prevY)));
                    }
                    else
                    {
                        if (!yDict.ContainsKey(currY))
                        {
                            yDict.Add(currY, new List<(int, int)>());
                        }
                        yDict[currY].Add((Math.Min(currX, prevX), Math.Max(currX, prevX)));
                    }
                }
            }

            return (xDict, yDict);
        }

        private static IEnumerable<(int X, int Y)[]> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                yield return sr.ReadLine().Split("->").Select(x =>
                    {
                        var xy = x.Split(",").ToArray();
                        return (int.Parse(xy[0]), int.Parse(xy[1]));
                    })
                    .ToArray();
            }
        }
    }
}
