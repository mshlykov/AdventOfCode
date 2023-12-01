using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2022
{
    internal class Day22
    {
        public static int SolveP1(string fileName)
        {
            var (xField, instructions) = GetInput(fileName);
            var yField = new (int, int, string)[xField.Max(x => x.Item2)];
            for(var i = 0; i < yField.Length; ++i)
            {
                var currX = i + 1;
                var minY = xField.Select((x, id) => (x, id))
                    .Where(x => x.x.Item1 <= currX && x.x.Item2 >= currX)
                    .Min(x => x.id);
                var maxY = xField.Select((x, id) => (x, id))
                    .Where(x => x.x.Item1 <= currX && x.x.Item2 >= currX)
                    .Max(x => x.id);
                var str = new StringBuilder();
                for (var j = minY; j <= maxY; ++j)
                {
                    var (minX, maxX, row) = xField[j];
                    str.Append(row[currX - minX]);
                }
                yField[i] = (minY + 1, maxY + 1, str.ToString());
            }

            var regex = new Regex("\\d+|R|L");
            var instrs = regex.Matches(instructions).Select(x => x.Value);
            var dir = "R";
            var pos = (xField[0].Item1 + xField[0].Item3.IndexOf('.'), 1);
            foreach (var instr in instrs)
            {
                if (int.TryParse(instr, out var offset))
                {
                    var (xStep, yStep) = GetIncrement(dir);
                    if (xStep != 0)
                    {
                        var (minX, maxX, row) = xField[pos.Item2 - 1];
                        
                        for (var i = 0; i < offset; ++i)
                        {
                            var newRelX = pos.Item1 - minX + xStep;
                            if (newRelX < 0)
                                newRelX += row.Length;
                            else if (newRelX > row.Length - 1)
                                newRelX -= row.Length;

                            if (row[newRelX] == '#')
                            {
                                break;
                            }

                            pos = (minX + newRelX, pos.Item2);
                        }
                    }
                    else
                    {
                        var (minY, maxY, col) = yField[pos.Item1 - 1];

                        for (var i = 0; i < offset; ++i)
                        {
                            var newRelY = pos.Item2 - minY + yStep;
                            if (newRelY < 0)
                                newRelY += col.Length;
                            else if (newRelY > col.Length - 1)
                                newRelY -= col.Length;

                            if (col[newRelY] == '#')
                            {
                                break;
                            }

                            pos = (pos.Item1, minY + newRelY);
                        }
                    }
                    
                }
                else
                {
                    dir = GetDirection(dir, instr);
                }
            }
            return 1000 * pos.Item2 + 4 * pos.Item1 + GetScore(dir);
        }

        private static int GetScore(string dir) => dir switch
        {
            "R" => 0,
            "B" => 1,
            "L" => 2,
            "T" => 3,
            _ => throw new InvalidOperationException()
        };

        private static (int, int) GetIncrement(string dir) => dir switch
        {
            "T" => (0, -1),
            "B" => (0, 1),
            "R" => (1, 0),
            "L" => (-1, 0),
            _ => throw new InvalidOperationException()
        };

        private static string GetDirection(string currDirection, string turn) => currDirection switch
        {
            "T" when turn == "R" => "R",
            "T" when turn == "L" => "L",
            "R" when turn == "R" => "B",
            "R" when turn == "L" => "T",
            "B" when turn == "R" => "L",
            "B" when turn == "L" => "R",
            "L" when turn == "R" => "T",
            "L" when turn == "L" => "B",
            _ => throw new InvalidOperationException()
        };

        public static int SolveP2(string fileName) 
        {
            var (xField, instructions) = GetInput(fileName);
            var yField = new (int, int, string)[xField.Max(x => x.Item2)];
            for (var i = 0; i < yField.Length; ++i)
            {
                var currX = i + 1;
                var minY = xField.Select((x, id) => (x, id))
                    .Where(x => x.x.Item1 <= currX && x.x.Item2 >= currX)
                    .Min(x => x.id);
                var maxY = xField.Select((x, id) => (x, id))
                    .Where(x => x.x.Item1 <= currX && x.x.Item2 >= currX)
                    .Max(x => x.id);
                var str = new StringBuilder();
                for (var j = minY; j <= maxY; ++j)
                {
                    var (minX, maxX, row) = xField[j];
                    str.Append(row[currX - minX]);
                }
                yField[i] = (minY + 1, maxY + 1, str.ToString());
            }

            var regex = new Regex("\\d+|R|L");
            var instrs = regex.Matches(instructions).Select(x => x.Value);
            var dir = "R";
            var pos = (xField[0].Item1 + xField[0].Item3.IndexOf('.'), 1);
            foreach (var instr in instrs)
            {
                if (int.TryParse(instr, out var offset))
                {
                    var (xStep, yStep) = GetIncrement(dir);
                    if (xStep != 0)
                    {
                        var (minX, maxX, row) = xField[pos.Item2 - 1];

                        for (var i = 0; i < offset; ++i)
                        {
                            var newRelX = pos.Item1 - minX + xStep;
                            if (newRelX < 0)
                                newRelX += row.Length;
                            else if (newRelX > row.Length - 1)
                                newRelX -= row.Length;

                            if (row[newRelX] == '#')
                            {
                                break;
                            }

                            pos = (minX + newRelX, pos.Item2);
                        }
                    }
                    else
                    {
                        var (minY, maxY, col) = yField[pos.Item1 - 1];

                        for (var i = 0; i < offset; ++i)
                        {
                            var newRelY = pos.Item2 - minY + yStep;
                            if (newRelY < 0)
                                newRelY += col.Length;
                            else if (newRelY > col.Length - 1)
                                newRelY -= col.Length;

                            if (col[newRelY] == '#')
                            {
                                break;
                            }

                            pos = (pos.Item1, minY + newRelY);
                        }
                    }

                }
                else
                {
                    dir = GetDirection(dir, instr);
                }
            }
            return 1000 * pos.Item2 + 4 * pos.Item1 + GetScore(dir);
        }

        private static ((int, int, string)[], string) GetInput(string fileName)
        {
            var parts = File.ReadAllText(fileName).Split(Environment.NewLine + Environment.NewLine);
            var field = parts[0].Split(Environment.NewLine)
                .Select(x => 
                {
                    var min = x.Select((x, i) => (x, i))
                        .Where(x => x.x == '#' || x.x == '.')
                        .Min(x => x.i) + 1;
                    var max = min + x[(min - 1)..].Length - 1;
                    return (min, max, x[(min - 1)..]);
                })
                .ToArray();
            return (field, parts[1]);
        }
    }
}
