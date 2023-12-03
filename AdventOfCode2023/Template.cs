namespace AdventOfCode2023
{
    internal class Template
    {
        public static object SolveP1(string fileName) => 0;

        public static object SolveP2(string fileName) => 0;

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
