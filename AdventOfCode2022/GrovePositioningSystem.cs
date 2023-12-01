namespace AdventOfCode2022
{
    internal class GrovePositioningSystem
    {
        public static long SolveP1(string fileName) => Solve(GetInput(fileName), 1, 1);

        public static long SolveP2(string fileName) => Solve(GetInput(fileName), 811589153, 10);

        private static long Solve(IEnumerable<long> input, long key, int mixesCount)
        {
            var blocks = input.Select((x, i) => new Block
                {
                    Value = x * key,
                    RawIdx = i,
                    MixedIdx = i
                })
                .ToArray();
            var mixedBlocks = blocks.ToArray();
            for (var t = 0; t < mixesCount; ++t)
            {
                for (var i = 0; i < blocks.Length; ++i)
                {
                    var idx = blocks[i].MixedIdx;
                    var offset = blocks[i].Value % (blocks.Length - 1);
                    if (offset == 0)
                    {
                        continue;
                    }

                    var newIdx = idx + offset;
                    newIdx += newIdx <= 0 ? blocks.Length - 1
                        : newIdx >= blocks.Length - 1 ? -(blocks.Length - 1)
                        : 0;
                    var temp = mixedBlocks[idx];
                    if (newIdx > idx)
                    {
                        for (var j = idx + 1; j <= newIdx; ++j)
                        {
                            mixedBlocks[j - 1] = mixedBlocks[j];
                            mixedBlocks[j - 1].MixedIdx = j - 1;
                        }
                    }
                    else
                    {
                        for (var j = idx - 1; j >= newIdx; --j)
                        {
                            mixedBlocks[j + 1] = mixedBlocks[j];
                            mixedBlocks[j + 1].MixedIdx = j + 1;
                        }
                    }

                    mixedBlocks[(int)newIdx] = temp;
                    temp.MixedIdx = (int)newIdx;
                }
            }
            var zeroItem = blocks.First(x => x.Value == 0);
            return blocks[mixedBlocks[(zeroItem.MixedIdx + 1000) % blocks.Length].RawIdx].Value
                + blocks[mixedBlocks[(zeroItem.MixedIdx + 2000) % blocks.Length].RawIdx].Value
                + blocks[mixedBlocks[(zeroItem.MixedIdx + 3000) % blocks.Length].RawIdx].Value;
        }

        private static IEnumerable<long> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                yield return long.Parse(sr.ReadLine());
            }
        }

        class Block
        {
            public long Value { get; set; }
            public int RawIdx { get; set; }
            public int MixedIdx { get; set; }
        }
    }
}
