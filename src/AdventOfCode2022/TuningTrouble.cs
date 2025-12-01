namespace AdventOfCode2022
{
    internal class TuningTrouble
    {
        public static int SolveP1(string fileName) => GetMessageStart(GetInput(fileName), 4);

        public static int SolveP2(string fileName) => GetMessageStart(GetInput(fileName), 14);
        
        private static int GetMessageStart(string payload, int offset)
        {
            var i = offset;
            var set = new HashSet<char>();
            for (; set.Count < offset && i < payload.Length; ++i)
            {
                set.Clear();
                for (var j = 0; j < offset; ++j)
                {
                    set.Add(payload[i - j - 1]);
                }
            }

            return i - 1;
        }

        private static string GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            return sr.ReadLine();
        }
    }
}
