namespace AdventOfCode2024;

internal class Day15
{
    public static object SolveP1(string fileName)
    {
        var (boxesStart, obstacles, start, plan) = GetInput(fileName);

        var currPoint = start;
        var boxes = boxesStart.ToHashSet();
        for (var i = 0; i < plan.Length; ++i)
        {
            var dir = GetDir(plan[i]);
            var listToMove = new List<(long X, long Y)>();
            var p = Translate(currPoint, dir);
            for (; boxes.Contains(p); p = Translate(p, dir))
            {
                listToMove.Add(p);
            }

            if (obstacles.Contains(p))
            {
                continue;
            }
            
            boxes.ExceptWith(listToMove);
            boxes.UnionWith(listToMove.Select(x => Translate(x, dir)));
            currPoint = Translate(currPoint, dir);
        }

        return boxes.Sum(x => x.X * 100L + x.Y);
    }

    public static object SolveP2(string fileName)
    {
        var (boxesStart, obstacles, start, plan) = GetInput2(fileName);

        var currPoint = start;
        var boxes = boxesStart.ToHashSet();
        for (var i = 0; i < plan.Length; ++i)
        {
            var dir = GetDir(plan[i]);
            var isHorizontalMove = dir.Y != 0 && dir.X == 0;
            var listToMove = new List<((long X, long Y) L, (long X, long Y) R)>();
            var lastFrontier = default(IEnumerable<(long X, long Y)>);
            if (isHorizontalMove)
            {
                var p = Translate(currPoint, dir);
                var boxToCheck = dir.Y > 0 ? (p, Translate(p, (0, dir.Y))) : (Translate(p, (0, dir.Y)), p);
                for (; boxes.Contains(boxToCheck);)
                {
                    listToMove.Add(boxToCheck);
                    p = Translate(p, (0, 2 * dir.Y));
                    boxToCheck = dir.Y > 0 ? (p, Translate(p, (0, dir.Y))) : (Translate(p, (0, dir.Y)), p);
                }

                lastFrontier = [p];
            }
            else
            {
                var pointsToCheck = new HashSet<(long X, long Y)>([Translate(currPoint, dir)]);
                for (; !pointsToCheck.Intersect(obstacles).Any() && pointsToCheck.Any(x => boxes.Any(y => y.L == x || y.R == x));)
                {
                    var boxesToAdd = boxes.Where(x => pointsToCheck.Contains(x.R) || pointsToCheck.Contains(x.L))
                        .ToArray();
                    listToMove.AddRange(boxesToAdd);
                    pointsToCheck = boxesToAdd.SelectMany(x => new[] { Translate(x.L, (dir.X, 0)), Translate(x.R, (dir.X, 0)) })
                        .ToHashSet();
                }

                lastFrontier = pointsToCheck;
            }

            if (lastFrontier.Intersect(obstacles).Any())
            {
                continue;
            }

            boxes.ExceptWith(listToMove);
            boxes.UnionWith(listToMove.Select(x => (L: Translate(x.L, dir), R: Translate(x.R, dir))));
            currPoint = Translate(currPoint, dir);
        }

        return boxes.Sum(x => x.L.X * 100L + x.L.Y);
    }

    private static (long X, long Y) Translate((long X, long Y) p, (long X, long Y) dir) => (p.X + dir.X, p.Y + dir.Y);

    private static (long X, long Y) GetDir(char c) => c switch
    {
        'v' => (X: 1, Y: 0),
        '^' => (X: -1, Y: 0),
        '<' => (X: 0, Y: -1),
        '>' => (X: 0, Y: 1),
        _ => throw new NotImplementedException()
    };

    private static (HashSet<((long X, long Y) L, (long X, long Y) R)> boxes, HashSet<(long X, long Y)> obstacles, (long X, long Y) start, string plan) GetInput2(string fileName)
    {
        var text = File.ReadAllText(fileName);
        var parts = text.Split(Environment.NewLine);
        var planStart = Array.IndexOf(parts, string.Empty);
        var field = parts[0..planStart];
        var plan = string.Concat(parts[planStart..]);
        var boxes = new HashSet<((long X, long Y), (long X, long Y))>();
        var obstacles = new HashSet<(long, long)>();
        var start = (-1L, -1L);
        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                switch (field[i][j])
                {
                    case 'O':
                        boxes.Add((((long)i, 2L * j), (i, 2L * j + 1L)));
                        break;
                    case '#':
                        obstacles.Add((i, 2 * j));
                        obstacles.Add((i, 2 * j + 1));
                        break;
                    case '@':
                        start = (i, 2 * j);
                        break;
                    default:
                        break;
                }
            }
        }

        return (boxes, obstacles, start, plan);
    }

    private static (HashSet<(long X, long Y)> boxes, HashSet<(long X, long Y)> obstacles, (long X, long Y) start, string plan) GetInput(string fileName)
    {
        var text = File.ReadAllText(fileName);
        var parts = text.Split(Environment.NewLine);
        var planStart = Array.IndexOf(parts, string.Empty);
        var field = parts[0..planStart];
        var plan = string.Concat(parts[planStart..]);
        var boxes = new HashSet<(long, long)>();
        var obstacles = new HashSet<(long, long)>();
        var start = (-1L, -1L);
        for (var i = 0; i < field.Length; ++i)
        {
            for (var j = 0; j < field[i].Length; ++j)
            {
                switch (field[i][j])
                {
                    case 'O':
                        boxes.Add((i, j));
                        break;
                    case '#':
                        obstacles.Add((i, j));
                        break;
                    case '@':
                        start = (i, j);
                        break;
                    default:
                        break;
                }
            }
        }

        return (boxes, obstacles, start, plan);
    }
}
