namespace AdventOfCode
{
    internal class RucksackReorganization
    {
        public static int SolveP1(string fileName) => GetRucksacks(fileName)
            .Sum(x => x.Substring(0, x.Length / 2)
                .ToHashSet()
                .Intersect(x.Substring(x.Length / 2))
                .Sum(x => GetPriority(x)));

        public static int SolveP2(string fileName) => GetRucksacks(fileName)
            .Select((item, idx) => (item, idx))
            .GroupBy(x => x.idx / 3, x => x.item)
            .Sum(y => y.Aggregate(default(IEnumerable<char>), (result, x) => result == null ? x.ToHashSet() : result.Intersect(x))
            .Sum(x => GetPriority(x)));

        private static int GetPriority(char item) => char.IsLower(item) ? item - 'a' + 1 : item - 'A' + 27;

        private static IEnumerable<string> GetRucksacks(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                yield return sr.ReadLine() ?? string.Empty;
            }
        }
    }
}
