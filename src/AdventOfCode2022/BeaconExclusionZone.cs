using System.Numerics;

namespace AdventOfCode2022
{
    internal class BeaconExclusionZone
    {
        public static int SolveP1(string fileName)
        {
            var searchedY = fileName == "1.txt" ? 10 : 2000000;
            var input = GetInput(fileName)
                .Select(x => (x.Sensor, x.Beacon, Radius: Distance(x.Sensor, x.Beacon)))
                .Where(x => Math.Abs(x.Sensor.Y - searchedY) <= x.Radius)
                .ToArray();
            var set = input.SelectMany(x => new[] { x.Sensor, x.Beacon }).ToHashSet();
            var minX = input.Min(x => x.Sensor.X - (x.Radius - Math.Abs(x.Sensor.Y - searchedY)));
            var maxX = input.Max(x => x.Sensor.X + (x.Radius - Math.Abs(x.Sensor.Y - searchedY)));
            return Enumerable.Range(minX, maxX - minX + 1)
            .Count(x => input.Any(y => Distance((x, searchedY), y.Sensor) <= y.Radius) && !set.Contains((x, searchedY)));
        }

        public static BigInteger SolveP2(string fileName)
        {
            var input = GetInput(fileName)
                .Select(x => (x.Sensor, x.Beacon, Radius: Distance(x.Sensor, x.Beacon)))
                .ToArray();
            var maxXY = fileName == "1.txt" ? 20 : 4000000;
            foreach (var (sensor, _, Radius) in input)
            {
                for (var i = 0; i < Radius; ++i)
                {
                    var options = new[]
                    {
                        (X: sensor.X + i, Y: sensor.Y + Radius + 1 - i),
                        (X: sensor.X - i, Y: sensor.Y + Radius + 1 - i),
                        (X: sensor.X + i, Y: sensor.Y - (Radius + 1 - i)),
                        (X: sensor.X - i, Y: sensor.Y - (Radius + 1 - i))
                    };
                    foreach (var option in options)
                    {
                        if (option.X >= 0 && option.X <= maxXY && option.Y >= 0 && option.Y <= maxXY && !input.Any(y => Distance(option, y.Sensor) <= y.Radius))
                        {
                            return (BigInteger)option.X * 4000000 + (BigInteger)option.Y;
                        }
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public static int Distance((int X, int Y) p1, (int X, int Y) p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

        private static IEnumerable<((int X, int Y) Sensor, (int X, int Y) Beacon)> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Split(":");
                var sensor = line[0][10..].Split(", ")
                    .Select(x => x.Split("="))
                    .Select(x => int.Parse(x[1]))
                    .ToArray();
                var beacon = line[1][22..].Split(", ")
                    .Select(x => x.Split("="))
                    .Select(x => int.Parse(x[1]))
                    .ToArray();
                yield return ((sensor[0], sensor[1]), (beacon[0], beacon[1]));
            }
        }
    }
}
