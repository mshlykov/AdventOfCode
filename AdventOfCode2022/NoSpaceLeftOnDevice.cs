namespace AdventOfCode2022
{
    internal class NoSpaceLeftOnDevice
    {
        public static int SolveP1(string fileName) => GetDirsSizes(GetInput(fileName))
            .Where(x => x <= 100000)
            .Sum();

        public static int SolveP2(string fileName)
        {
            var dirsSizes = GetDirsSizes(GetInput(fileName)).ToArray();
            var spaceToFree = 30000000 - (70000000 - dirsSizes.Max());
            var sizeToFree = dirsSizes.Where(x => x >= spaceToFree)
                .Min();

            return sizeToFree;
        }

        private static IEnumerable<int> GetDirsSizes(IEnumerable<string[]> input)
        {
            var stack = new Stack<int>();

            foreach (var line in input)
            {
                if (line[0] == "$")
                {
                    if (line[1] == "cd")
                    {
                        if (line[2] != "..")
                        {
                            stack.Push(0);
                        }
                        else
                        {
                            var traversedDirSize = stack.Pop();
                            yield return traversedDirSize;

                            if (stack.Any())
                            {
                                var parentDirSize = stack.Pop();
                                stack.Push(parentDirSize + traversedDirSize);
                            }
                        }
                    }
                }
                else
                {
                    if (line[0] != "dir")
                    {
                        var currSize = stack.Pop();
                        stack.Push(currSize + int.Parse(line[0]));
                    }
                }
            }

            while (stack.Any())
            {
                var remainingDirSize = stack.Pop();
                yield return remainingDirSize;

                if (stack.Any())
                {
                    var parentDirSize = stack.Pop();
                    stack.Push(parentDirSize + remainingDirSize);
                }
            }
        }

        private static IEnumerable<string[]> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                yield return sr.ReadLine().Split(" ");
            }
        }
    }
}
