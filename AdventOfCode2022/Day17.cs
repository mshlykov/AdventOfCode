using System.Diagnostics;

namespace AdventOfCode2022
{
    internal class Day17
    {
        public static long SolveP1(string fileName) => Solve2(0, 2022, 0, GetInput(fileName));

        public static long SolveP2(string fileName) => Solve2(0, 1000000000000L, 0, GetInput(fileName));

        private static long Solve(long itCount, string pattern)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var currPatIdx = 0;
            var occupiedCells = Enumerable.Range(1, 7)
                .Select(x => new HashSet<long>())
                .ToArray();
            var maxY = 0L;

            Console.WriteLine($"Start: {stopWatch.ElapsedMilliseconds}");
            stopWatch.Restart();
            for (var i = 0L; i < itCount; ++i)
            {
                var position = (X: 3L, Y: maxY + 4);
                while (true)
                {
                    var currPattern = pattern[(currPatIdx++) % pattern.Length];
                    var newPosition = currPattern == '<' ? (position.X - 1, position.Y) : (position.X + 1, position.Y);
                    var newPoints = GetOccupiedPoints(newPosition, i);
                    if (!newPoints.Any(x => x.X < 1 || x.X > 7 || occupiedCells[x.X - 1].Contains(x.Y)))
                    {
                        position = newPosition;
                    }

                    newPosition = (position.X, position.Y - 1);
                    newPoints = GetOccupiedPoints(newPosition, i);
                    if (newPoints.Any(x => x.Y < 1 || occupiedCells[x.X - 1].Contains(x.Y)))
                    {
                        var points = GetOccupiedPoints(position, i);
                        foreach (var point in points)
                        {
                            occupiedCells[point.X - 1].Add(point.Y);

                            maxY = Math.Max(maxY, point.Y);
                        }

                        break;
                    }

                    position = newPosition;
                }

                if (i % 1000000 == 0 && i != 0)
                {
                    Console.WriteLine($"{i}: {stopWatch.ElapsedMilliseconds}");
                    stopWatch.Restart();
                }
                //Console.WriteLine(string.Join(", ", occupiedCells.Select(x => x.Count)));
            }
            Console.WriteLine($"End: {stopWatch.ElapsedMilliseconds}");


            return maxY;
        }

        private static long Solve2(long startIt, long itCount, int startPatIdx, string pattern)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var currPatIdx = startPatIdx;
            var reversedCells = new Dictionary<long, int>();
            var maxY = 0L;
            //var firstRepeat = -1L;
            //Console.WriteLine($"Start: {stopWatch.ElapsedMilliseconds}");
            //stopWatch.Restart();
            for (var i = startIt; i < itCount; ++i)
            {
                //Console.WriteLine($"i: {i}");
                var position = (X: 3, Y: maxY + 4);
                while (true)
                {
                    var currPattern = pattern[currPatIdx % pattern.Length];
                    var newPosition = currPattern == '<' ? (position.X - 1, position.Y) : (position.X + 1, position.Y);
                    var newPoints = GetOccupiedPoints2(newPosition, i);
                    if (!newPoints.Any(x => (x.X & 0b000000001) == 0b000000001 || (x.X & 0b100000000) == 0b100000000 || reversedCells.TryGetValue(x.Y, out var val) && (val & x.X) != 0))
                    {
                        position = newPosition;
                    }
                    ++currPatIdx;

                    newPosition = (position.X, position.Y - 1);
                    newPoints = GetOccupiedPoints2(newPosition, i);
                    if (newPoints.Any(x => x.Y < 1 || reversedCells.TryGetValue(x.Y, out var val) && (val & x.X) != 0))
                    {
                        var points = GetOccupiedPoints2(position, i);
                        foreach (var point in points)
                        {
                            if (!reversedCells.ContainsKey(point.Y))
                            {
                                reversedCells.Add(point.Y, point.X);
                            }
                            else
                            {
                                reversedCells[point.Y] |= point.X;
                            }
                            maxY = Math.Max(maxY, point.Y);
                        }

                        //if (reversedCells[maxY] == 0b11111110)
                        //{
                        //    if (firstRepeat == -1)
                        //    {
                        //        firstRepeat = i % 5;
                        //    }
                        //    else if(firstRepeat == (i % 5))
                        //    {
                        //        Console.WriteLine("-------------------------------------");
                        //        for (var t = Math.Max(maxY - 11, 0); t <= maxY; ++t)
                        //        {
                        //            var num = reversedCells.ContainsKey(t + 1) ? reversedCells[t + 1] : 0;
                        //            if (num == 0)
                        //            {
                        //                Console.WriteLine("0000000");
                        //            }
                        //            else
                        //            {
                        //                for (var j = 1; j <= 7; ++j)
                        //                {
                        //                    Console.Write((num & (1 << j)) == (1 << j) ? "1" : "0");
                        //                }
                        //                Console.Write(Environment.NewLine);
                        //            }
                        //        }
                        //        var residual = Solve2(itCount - (itCount % i) + 1, itCount, currPatIdx, pattern);

                        //        return residual + itCount / (i + 1) * maxY;
                        //    }

                        //}

                        if (maxY >= 100)
                        {
                            for (var offset = 5; offset <= maxY / 2; ++offset)
                            {
                                var equal = true;
                                var transition = maxY / 2;
                                for (var j = transition + 1; j < offset; ++j)
                                {
                                    if (reversedCells[maxY - j] != reversedCells[maxY - offset - j])
                                    {
                                        equal = false;
                                        break;
                                    }
                                }
                                if (equal)
                                {

                                    var residual = Solve2(itCount - (itCount % (i / 2)) + 1, itCount, currPatIdx, pattern);

                                    return residual + itCount / ((i / 2) + 1) * maxY;
                                }
                            }
                            
                        }

                        break;
                    }

                    position = newPosition;
                }
            }

            Console.WriteLine("-------------------------------------");
            for (var t = 0 /*Math.Max(maxY - 11, 0)*/; t <= maxY; ++t)
            {
                var num = reversedCells.ContainsKey(t + 1) ? reversedCells[t + 1] : 0;
                if (num == 0)
                {
                    Console.WriteLine("0000000");
                }
                else
                {
                    for (var j = 1; j <= 7; ++j)
                    {
                        Console.Write((num & (1 << j)) == (1 << j) ? "1" : "0");
                    }
                    Console.Write(Environment.NewLine);
                }

                //Console.WriteLine(Convert.ToString(num, 2));
            }


            return maxY;
        }

        private static IEnumerable<(int X, long Y)> GetOccupiedPoints2((int X, long Y) position, long i) => (i % 5) switch
        {
            0 => new[]
            {
                (0b1111 << position.X, position.Y)
            },
            1 => new[]
            {
                (0b10 << position.X, position.Y + 2),
                (0b111 << position.X, position.Y + 1),
                (0b10 << position.X, position.Y)
            },
            2 => new[]
            {
                (0b100 << position.X, position.Y + 2),
                (0b100 << position.X, position.Y + 1),
                (0b111 << position.X, position.Y)
            },
            3 => new[]
            {
                (0b1 << position.X, position.Y + 3),
                (0b1 << position.X, position.Y + 2),
                (0b1 << position.X, position.Y + 1),
                (0b1 << position.X, position.Y)
            },
            4 => new[]
            {
                (0b11 << position.X, position.Y + 1),
                (0b11 << position.X, position.Y)
            },
            _ => throw new ArgumentException()
        };

        private static IEnumerable<(long X, long Y)> GetOccupiedPoints((long X, long Y) position, long i) => (i % 5) switch
        {
            0 => new[]
            {
                (position.X, position.Y),
                (position.X + 1, position.Y),
                (position.X + 2, position.Y),
                (position.X + 3, position.Y)
            },
            1 => new[]
            {
                (position.X, position.Y + 1),
                (position.X + 1, position.Y + 1),
                (position.X + 2, position.Y + 1),
                (position.X + 1, position.Y),
                (position.X + 1, position.Y + 2)
            },
            2 => new[]
            {
                (position.X, position.Y),
                (position.X + 1, position.Y),
                (position.X + 2, position.Y),
                (position.X + 2, position.Y + 1),
                (position.X + 2, position.Y + 2),
            },
            3 => new[]
            {
                (position.X, position.Y),
                (position.X, position.Y + 1),
                (position.X, position.Y + 2),
                (position.X, position.Y + 3)
            },
            4 => new[]
            {
                (position.X, position.Y),
                (position.X + 1, position.Y),
                (position.X, position.Y + 1),
                (position.X + 1, position.Y + 1)
            },
            _ => throw new ArgumentException()
        };

        private static string GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            return sr.ReadLine();
        }
    }
}
