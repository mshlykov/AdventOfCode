using System.Text;

namespace AdventOfCode2022
{
    internal class CathodeRayTube
    {
        public static int SolveP1(string fileName)
        {
            var checkPoints = new[] { 20, 60, 100, 140, 180, 220 };
            return GetCycles(GetInput(fileName))
                .Where(x => checkPoints.Contains(x.Cycle))
                .Sum(x => x.Cycle * x.X);
        }

        public static string SolveP2(string fileName)
        {
            var checkPoints = new[] { 40, 80, 120, 160, 200, 240 };
            var sb = new StringBuilder();
            var (pos, currX) = (0, 1);

            foreach (var (cycle, X) in GetCycles(GetInput(fileName)))
            {
                pos++;
                if (new[] { currX, currX + 1, currX + 2 }.Contains(pos))
                {
                    sb.Append('#');
                }
                else
                {
                    sb.Append('.');
                }
                if (checkPoints.Contains(cycle - 1))
                {
                    sb.Append(Environment.NewLine);
                    pos = 0;
                }
                currX = X;
            }

            return sb.ToString();
        }

        private static IEnumerable<(int Cycle, int X)> GetCycles(IEnumerable<(string, int)> instructions)
        {
            var (cycle, X) = (1, 1);
            foreach (var (instruction, param) in instructions)
            {
                if (instruction == "addx")
                {
                    cycle++;
                    yield return (cycle, X);
                    cycle++;
                    X += param;
                    yield return (cycle, X);
                }
                else
                {
                    cycle++;
                    yield return (cycle, X);
                }
            }
        }

        private static IEnumerable<(string, int)> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Split(" ").ToArray();
                yield return line.Length == 2 ? (line[0], int.Parse(line[1])) : (line[0], 0);
            }
        }
    }
}
