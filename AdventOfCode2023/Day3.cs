namespace AdventOfCode2023
{
    internal class Day3
    {
        public static object SolveP1(string fileName)
        {
            var input = GetInput(fileName).ToArray();
            var maxX = input[0].Length - 1;
            var maxY = input.Length - 1;
            var result = 0L;
            for (var i = 0; i <= maxY; ++i)
            {
                for (var j = 0; j <= maxX; ++j)
                {
                    if (!char.IsDigit(input[i][j]))
                    {
                        continue;
                    }

                    var charsCoords = new List<(int X, int Y)>();
                    while (j <= maxX && char.IsDigit(input[i][j]))
                    {
                        charsCoords.Add((j, i));
                        ++j;
                    }

                    if (!charsCoords.SelectMany(x => GetProbes(x.X, x.Y, maxX, maxY)).Any(y => !char.IsDigit(input[y.Y][y.X]) && input[y.Y][y.X] != '.'))
                    {
                        continue;
                    }

                    var span = input[i].AsSpan(charsCoords[0].X, charsCoords.Count);
                    result += long.Parse(span);
                }
            }
            return result;
        }

        public static object SolveP2(string fileName)
        {
            var input = GetInput(fileName).ToArray();
            var maxX = input[0].Length - 1;
            var maxY = input.Length - 1;
            var numbersData = new List<(long Number, IEnumerable<(int X, int Y)> Coords)>();
            for (var i = 0; i <= maxY; ++i)
            {
                for (var j = 0; j <= maxX; ++j)
                {
                    if (!char.IsDigit(input[i][j]))
                    {
                        continue;
                    }

                    var charsCoords = new List<(int X, int Y)>();
                    while (j <= maxX && char.IsDigit(input[i][j]))
                    {
                        charsCoords.Add((j, i));
                        ++j;
                    }

                    var span = input[i].AsSpan(charsCoords[0].X, charsCoords.Count);
                    var number = long.Parse(span);
                    numbersData.Add((number, charsCoords));
                }
            }

            var result = 0L;
            for (var i = 0; i <= maxY; ++i)
            {
                for (var j = 0; j <= maxX; ++j)
                {
                    if (input[i][j] != '*')
                    {
                        continue;
                    }

                    var data = numbersData
                            .Where(x => x.Coords.SelectMany(y => GetProbes(y.X, y.Y, maxX, maxY)).Contains((j, i)))
                            .ToArray();

                    if (data.Length != 2)
                    {
                        continue;
                    }

                    result += data[0].Number * data[1].Number;
                }
            }
            return result;
        }

        private static IEnumerable<(int X, int Y)> GetProbes(int x, int y, int maxX, int maxY) => new[] {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1), (0, 0), (0, 1),
                (1, -1), (1, 0), (1, 1)
            }
            .Select(p => (x + p.Item1, y + p.Item2))
            .Where(p => p.Item1 >= 0 && p.Item1 <= maxX && p.Item2 >= 0 && p.Item2 <= maxY)
            .ToArray();

        private static IEnumerable<string> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                yield return sr.ReadLine();
            }
        }
    }
}
