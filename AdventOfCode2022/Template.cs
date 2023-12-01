namespace AdventOfCode2022
{
    internal class Template
    {
        public static int SolveP1(string fileName) => 0;

        public static int SolveP2(string fileName) => 0;

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
