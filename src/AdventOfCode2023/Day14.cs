namespace AdventOfCode2023;

internal class Day14
{
    public static object SolveP1(string fileName)
    {
        var (oCoords, sCoords, maxI, _) = GetPoints(GetInput(fileName));

        return GetLoad(TiltN(oCoords, sCoords), maxI);
    }

    public static object SolveP2(string fileName)
    {
        var (oCoords, sCoords, maxI, maxJ) = GetPoints(GetInput(fileName));

        var oCurr = oCoords.ToList();
        var loads = new List<long>();
        var periodData = default((int Tail, int Period)?);
        for (var i = 0L; periodData == null && i < 1000000000L; ++i)
        {
            oCurr = TiltN(oCurr, sCoords).ToList();
            oCurr = TiltW(oCurr, sCoords).ToList();
            oCurr = TiltS(oCurr, sCoords, maxI).ToList();
            oCurr = TiltE(oCurr, sCoords, maxJ).ToList();

            loads.Add(GetLoad(oCurr, maxI));
            periodData = GetPeriod(loads);
        }

        if (periodData == null)
        {
            return loads.Last();
        }

        return loads[periodData.Value.Item1 + (int)((1000000000L - periodData.Value.Tail - 1) % periodData.Value.Period)];
    }

    private static (IEnumerable<Point>, IEnumerable<Point>, int, int) GetPoints(IEnumerable<string> data)
    {
        var input = data.ToArray();
        var (maxI, maxJ) = (input.Length - 1, input[0].Length - 1);
        var oCoords = new List<Point>();
        var sCoords = new List<Point>();
        for (var i = 0; i < maxI + 1; ++i)
        {
            for (var j = 0; j < maxJ + 1; ++j)
            {
                if (input[i][j] == '#')
                {
                    sCoords.Add(new Point
                    {
                        I = i,
                        J = j
                    });
                }
                if (input[i][j] == 'O')
                {
                    oCoords.Add(new Point
                    {
                        I = i,
                        J = j
                    });
                }
            }
        }
        return (oCoords, sCoords, maxI, maxJ);
    }

    private static long GetLoad(IEnumerable<Point> oCoords, int maxI) => oCoords.Sum(x => maxI - x.I + 1L);

    private static IEnumerable<Point> TiltN(IEnumerable<Point> oCoords, IEnumerable<Point> sCoords)
    {
        var groups = oCoords.GroupBy(x => (x.J, sCoords.Where(y => y.J == x.J && y.I < x.I).MaxBy(y => y.I)));
        foreach (var group in groups)
        {
            var st = group.Key.Item2 == null ? 0 : group.Key.Item2.I + 1;
            foreach(var (item, idx) in group.Select((x, i) => (x, i)))
            {
                yield return new Point
                {
                    I = st + idx,
                    J = item.J
                };
            }
        }
    }

    private static IEnumerable<Point> TiltS(IEnumerable<Point> oCoords, IEnumerable<Point> sCoords, int maxI)
    {
        var groups = oCoords.GroupBy(x => (x.J, sCoords.Where(y => y.J == x.J && y.I > x.I).MinBy(y => y.I)));
        foreach (var group in groups)
        {
            var st = group.Key.Item2 == null ? maxI : group.Key.Item2.I - 1;
            foreach (var (item, idx) in group.Select((x, i) => (x, i)))
            {
                yield return new Point
                {
                    I = st - idx,
                    J = item.J
                };
            }
        }
    }

    private static IEnumerable<Point> TiltE(IEnumerable<Point> oCoords, IEnumerable<Point> sCoords, int maxJ)
    {
        var groups = oCoords.GroupBy(x => (x.I, sCoords.Where(y => y.I == x.I && y.J > x.J).MinBy(y => y.J)));
        foreach (var group in groups)
        {
            var st = group.Key.Item2 == null ? maxJ : group.Key.Item2.J - 1;
            foreach (var (item, idx) in group.Select((x, i) => (x, i)))
            {
                yield return new Point
                {
                    I = item.I,
                    J = st - idx
                };
            }
        }
    }

    private static IEnumerable<Point> TiltW(IEnumerable<Point> oCoords, IEnumerable<Point> sCoords)
    {
        var groups = oCoords.GroupBy(x => (x.I, sCoords.Where(y => y.I == x.I && y.J < x.J).MaxBy(y => y.J)));
        foreach (var group in groups)
        {
            var st = group.Key.Item2 == null ? 0 : group.Key.Item2.J + 1;
            foreach (var (item, idx) in group.Select((x, i) => (x, i)))
            {
                yield return new Point
                {
                    I = item.I,
                    J = st + idx
                };
            }
        }
    }

    private static (int Tail, int Period)? GetPeriod(List<long> loads)
    {
        var i = loads.Count % 2 == 1 ? 1 : 0;
        for(; loads.Count - i > 5; i += 2)
        {
            var skipped = loads.Skip(i).ToArray();
            if (skipped[..(skipped.Length / 2)].SequenceEqual(skipped[(skipped.Length / 2)..]))
            {
                return (i, skipped.Length / 2);
            }
        }

        return null;
    }

    private static IEnumerable<string> GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        while (!sr.EndOfStream)
        {
            yield return sr.ReadLine();
        }
    }

    private class Point
    {
        public int I { get; set; }
        public int J { get; set; }
    };
}
