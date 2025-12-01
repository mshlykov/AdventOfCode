namespace AdventOfCode2022
{
    internal class CalorieCounting
    {
        public static int SolveP1(string fileName) => GetSums(fileName)
            .Max();

        public static int SolveP2(string fileName) => GetSums(fileName)
            .OrderByDescending(x => x)
            .Take(3)
            .Sum();

        private static IEnumerable<int> GetSums(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var sum = 0;
                while (!sr.EndOfStream && int.TryParse(sr.ReadLine(), out var currentNumber))
                {
                    sum += currentNumber;
                }

                yield return sum;
            }
        }
    }
}
