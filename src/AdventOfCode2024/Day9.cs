namespace AdventOfCode2024;

internal class Day9
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName);
        var memData = input.Select((x, i) => (Id: i % 2 == 0 ? i / 2 : -1, Size: x))
            .Where(x => x.Size != 0)
            .ToList();

        for (var (i, j) = (FindFreeSlot(memData, 0), FindOccupiedSlot(memData, memData.Count - 1)); i < j; (i, j) = (FindFreeSlot(memData, i), FindOccupiedSlot(memData, j)))
        {
            switch ((memData[i].Size, memData[j].Size))
            {
                case var (size1, size2) when size1 >= size2:
                    {
                        var sizeToInsert = memData[i].Size - memData[j].Size;
                        memData[i] = memData[j];
                        memData[j] = (-1, memData[j].Size);
                        if (sizeToInsert > 0)
                        {
                            memData.Insert(i + 1, (-1, sizeToInsert));
                        }
                        break;
                    }
                case var (size1, size2) when size1 < size2:
                    {
                        var sizeToPreserve = memData[j].Size - memData[i].Size;
                        memData[i] = (memData[j].Id, memData[i].Size);
                        memData[j] = (memData[j].Id, sizeToPreserve);
                        break;
                    }
                default:
                    throw new Exception();
            }
        }

        return GetCheckSum(memData);
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName);
        var memData = input.Select((x, i) => (Id: i % 2 == 0 ? i / 2 : -1, Size: x))
            .Where(x => x.Size != 0)
            .ToList();

        for (var j = FindOccupiedSlot(memData, memData.Count - 1); j != -1; j = FindOccupiedSlot(memData, j))
        {
            var i = FindFreeSlot(memData, 0, j, memData[j].Size);
            if (i == -1)
            {
                --j;
                continue;
            }

            var sizeToInsert = memData[i].Size - memData[j].Size;
            memData[i] = memData[j];
            memData[j] = (-1, memData[j].Size);
            if (sizeToInsert > 0)
            {
                memData.Insert(i + 1, (-1, sizeToInsert));
            }
        }

        return GetCheckSum(memData);
    }

    private static long GetCheckSum(List<(int Id, int Size)> memData)
    {
        IEnumerable<long> LongRange(long start, long count)
        {
            for (var i = 0L; i < count; ++i)
            {
                yield return start + i;
            }
        }

        var startIdx = 0L;
        var result = 0L;
        for (var i = 0; i < memData.Count; startIdx += memData[i].Size, ++i)
        {
            if (memData[i].Id == -1)
            {
                continue;
            }

            result += LongRange(startIdx, memData[i].Size).Sum() * memData[i].Id;
        }

        return result;
    }

    private static int FindOccupiedSlot(List<(int Id, int Size)> memData, int start)
    {
        for (var i = start; i >= 0; --i)
        {
            if (memData[i].Id != -1)
            {
                return i;
            }
        }

        return -1;
    }

    private static int FindFreeSlot(List<(int Id, int Size)> memData, int start)
    {
        for (var i = start; i < memData.Count; ++i)
        {
            if (memData[i].Id == -1)
            {
                return i;
            }
        }

        return -1;
    }

    private static int FindFreeSlot(List<(int Id, int Size)> memData, int start, int end, int size)
    {
        for (var i = start; i < end; ++i)
        {
            if (memData[i].Id == -1 && memData[i].Size >= size)
            {
                return i;
            }
        }

        return -1;
    }

    private static int[] GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        return sr.ReadLine()
            .Select(x => x - '0')
            .ToArray();
    }
}
