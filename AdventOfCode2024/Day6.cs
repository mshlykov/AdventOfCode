namespace AdventOfCode2024;

internal class Day6
{
    public static object SolveP1(string fileName)
    {
        var field = GetInput(fileName)
            .Select(x => x.ToCharArray())
            .ToArray();
        var currPosition = Enumerable.Range(0, field.Length)
            .SelectMany(x => Enumerable.Range(0, field[x].Length).Select(y => (x, y)))
            .First(x => field[x.x][x.y] != '.' && field[x.x][x.y] != '#');
        var currDirection = field[currPosition.Item1][currPosition.Item2];
        var positions = new List<(int, int)>() { currPosition };
        while (true)
        {
            var (i, j) = GetNextPosition(currPosition, currDirection, field);
            if (!IsInField((i, j), field))
            {
                break;
            }
            while (field[i][j] == '#')
            {
                currDirection = TurnRight(currDirection);
                (i, j) = GetNextPosition(currPosition, currDirection, field);
            }
            currPosition = (i, j);
            positions.Add((i, j));
        }

        return positions.Distinct().Count();
    }

    private static char TurnRight(char dir) => dir switch
    {
        '^' => '>',
        '>' => 'v',
        'v' => '<',
        '<' => '^',
        _ => throw new Exception()
    };

    private static (int, int) GetNextPosition((int, int) currPosition, char currDirection, char[][] field)
    {
        var (i, j) = currPosition;
        return currDirection switch
        {
            '^' => (i - 1, j),
            '>' => (i, j + 1),
            'v' => (i + 1, j),
            '<' => (i, j - 1),
            _ => throw new Exception()
        };
    }

    private static bool IsInField((int, int) currPosition, char[][] field)
    {
        var (i, j) = currPosition;
        return i >= 0 && i < field.Length && j >= 0 && j < field[i].Length;
    }


    //non-optimal, could be improved by calculating next obstacle immediately when moving in a certain direction
    public static object SolveP2(string fileName)
    {
        var field = GetInput(fileName)
            .Select(x => x.ToCharArray())
            .ToArray();
        var startPosition = Enumerable.Range(0, field.Length)
            .SelectMany(x => Enumerable.Range(0, field[x].Length).Select(y => (x, y)))
            .First(x => field[x.x][x.y] != '.' && field[x.x][x.y] != '#');
        var startDirection = field[startPosition.Item1][startPosition.Item2];
        var res = 0L;

        for (var k = 0; k < field.Length; ++k)
        {
            for (var t = 0; t < field[k].Length; ++t)
            {
                if (field[k][t] != '.')
                {
                    continue;
                }
                field[k][t] = '#';

                var currPosition = startPosition;
                var currDirection = startDirection;
                var positions = new HashSet<((int, int), char)>() { (currPosition, currDirection) };
                while (true)
                {
                    var (i, j) = GetNextPosition(currPosition, currDirection, field);
                    if (!IsInField((i, j), field))
                    {
                        break;
                    }
                    while (field[i][j] == '#')
                    {
                        currDirection = TurnRight(currDirection);
                        (i, j) = GetNextPosition(currPosition, currDirection, field);
                    }
                    currPosition = (i, j);
                    if (!positions.Add(((i, j), currDirection))) 
                    {
                        ++res;
                        break;
                    };
                }

                field[k][t] = '.';
            }
        }

        return res;
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
}
