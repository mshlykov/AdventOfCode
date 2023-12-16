namespace AdventOfCode2023;

internal class Day16
{
    public static object SolveP1(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();

        return CountEnergized(((0, 0), Dir.R), input);
    }

    public static object SolveP2(string fileName)
    {
        var input = GetInput(fileName)
            .ToArray();

        var starts = new List<((int i, int j), Dir d)>();

        for (var i = 0; i < input.Length; ++i)
        {
            starts.Add(((i, 0), Dir.R));
            starts.Add(((i, input[0].Length - 1), Dir.L));
        }

        for (var i = 0; i < input[0].Length; ++i)
        {
            starts.Add(((0, i), Dir.B));
            starts.Add(((input.Length - 1, i), Dir.T));
        }

        return starts.Max(x => CountEnergized(x, input));
    }

    private static (int i, int j)  GetNextCoord((int i, int j) coords, Dir d) => d switch
    {
        Dir.R => (coords.i, coords.j + 1),
        Dir.L => (coords.i, coords.j - 1),
        Dir.T => (coords.i - 1, coords.j),
        Dir.B => (coords.i + 1, coords.j),
        _ => throw new Exception(),
    };

    private static bool IsInField((int i, int j) beam, string[] input) => beam.i >= 0 && beam.i < input.Length && beam.j >= 0 && beam.j < input[0].Length;

    private static long CountEnergized(((int i, int j), Dir d) start, string[] input)
    {
        var beams = new List<((int i, int j), Dir d)>()
        {
            start
        };
        var allBeams = beams.ToHashSet();

        while (beams.Count != 0)
        {
            var candidates = new List<((int i, int j), Dir d)>();
            foreach (var beam in beams)
            {
                switch (input[beam.Item1.i][beam.Item1.j])
                {
                    case '.':
                        {
                            candidates.Add((GetNextCoord(beam.Item1, beam.d), beam.d));
                        }
                        break;
                    case '\\':
                        {
                            var newDir = beam.d switch
                            {
                                Dir.R => Dir.B,
                                Dir.L => Dir.T,
                                Dir.B => Dir.R,
                                Dir.T => Dir.L,
                                _ => throw new NotImplementedException()
                            };
                            candidates.Add((GetNextCoord(beam.Item1, newDir), newDir));
                        }
                        break;
                    case '/':
                        {
                            var newDir = beam.d switch
                            {
                                Dir.R => Dir.T,
                                Dir.L => Dir.B,
                                Dir.B => Dir.L,
                                Dir.T => Dir.R,
                                _ => throw new NotImplementedException()
                            };
                            candidates.Add((GetNextCoord(beam.Item1, newDir), newDir));
                        }
                        break;
                    case '-':
                        {
                            if (beam.d == Dir.R || beam.d == Dir.L)
                            {
                                candidates.Add((GetNextCoord(beam.Item1, beam.d), beam.d));
                            }
                            else
                            {
                                candidates.Add((GetNextCoord(beam.Item1, Dir.R), Dir.R));
                                candidates.Add((GetNextCoord(beam.Item1, Dir.L), Dir.L));
                            }
                        }
                        break;
                    case '|':
                        {
                            if (beam.d == Dir.T || beam.d == Dir.B)
                            {
                                candidates.Add((GetNextCoord(beam.Item1, beam.d), beam.d));
                            }
                            else
                            {
                                candidates.Add((GetNextCoord(beam.Item1, Dir.T), Dir.T));
                                candidates.Add((GetNextCoord(beam.Item1, Dir.B), Dir.B));
                            }
                        }
                        break;
                }
            }

            beams = candidates.Where(x => IsInField(x.Item1, input) && allBeams.Add(x))
                .ToList();
        }

        return allBeams.DistinctBy(x => x.Item1)
            .Count();
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

    private enum Dir
    {
        R,
        L,
        B,
        T
    }
}
