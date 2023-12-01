namespace AdventOfCode2022
{
    internal class CampCleanup
    {
        public static int SolveP1(string fileName) => GetRanges(fileName)
            .Count(x => x.First.Left >= x.Second.Left && x.First.Right <= x.Second.Right
                || x.Second.Left >= x.First.Left && x.Second.Right <= x.First.Right);

        public static int SolveP2(string fileName) => GetRanges(fileName)
            .Count(x => x.First.Left <= x.Second.Right && x.First.Right >= x.Second.Left);

        private static IEnumerable<((int Left, int Right) First, (int Left, int Right) Second)> GetRanges(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var pairs = sr.ReadLine()
                    .Split(",")
                    .SelectMany(x => x.Split("-"))
                    .Select(x => int.Parse(x))
                    .ToArray();
                yield return ((pairs[0], pairs[1]), (pairs[2], pairs[3]));
            }
        }
    }
}
