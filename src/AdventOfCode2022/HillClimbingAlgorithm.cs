namespace AdventOfCode2022
{
    internal class HillClimbingAlgorithm
    {
        public static int? SolveP1(string fileName)
        {
            var input = GetInput(fileName).ToArray();
            var dest = input.SelectMany((x, i) => x.Select((y, j) => (i, j, input[i][j])))
                .Where(x => x.Item3 == 'E')
                .Select(x => (x.i, x.j))
                .First();
            var start = input.SelectMany((x, i) => x.Select((y, j) => (i, j, input[i][j])))
                .Where(x => x.Item3 == 'S')
                .Select(x => (x.i, x.j))
                .First();

            return GetShortestPath(input, start, dest);
        }

        public static int? SolveP2(string fileName)
        {
            var input = GetInput(fileName).ToArray();
            var dest = input.SelectMany((x, i) => x.Select((y, j) => (i, j, input[i][j])))
                .Where(x => x.Item3 == 'E')
                .Select(x => (x.i, x.j))
                .First();
            var starts = input.SelectMany((x, i) => x.Select((y, j) => (i, j, input[i][j])))
                .Where(x => GetElevation(x.Item3) == 'a')
                .Select(x => (x.i, x.j))
                .ToArray();

            var res = default(int?);
            foreach (var start in starts)
            {
                var pathLength = GetShortestPath(input, start, dest);
                res = pathLength == null ? res
                        : res == null ? pathLength : Math.Min(res.Value, pathLength.Value);
            }

            return res;
        }

        // Dijkstra implementation
        // at the same time the case is degenerate (all edges are of the same weight) so could be regular bfs
        private static int? GetShortestPath(char[][] field, (int, int) start, (int, int) dest)
        {
            var distances = new int?[field.Length, field[0].Length];
            distances[start.Item1, start.Item2] = 0;
            //should be heap instead of list
            var queue = new List<(int, int)>
                {
                    start
                };
            var visited = new HashSet<(int, int)>();
            while (queue.Any())
            {
                var min = queue.Min(x => distances[x.Item1, x.Item2]);
                var (i, j) = queue.First(x => distances[x.Item1, x.Item2] == min);
                if ((i, j) == dest)
                {
                    break;
                }
                queue.Remove((i, j));
                var options = new[] { (i, j + 1), (i, j - 1), (i - 1, j), (i + 1, j) }
                .Where(x => x.Item1 < field.Length && x.Item1 > -1 && x.Item2 < field[i].Length && x.Item2 > -1)
                .Where(x => GetElevation(field[x.Item1][x.Item2]) - GetElevation(field[i][j]) <= 1)
                .ToArray();

                foreach (var option in options)
                {
                    distances[option.Item1, option.Item2] = distances[option.Item1, option.Item2] == null ? distances[i, j] + 1
                        : Math.Min(distances[option.Item1, option.Item2].Value, distances[i, j].Value + 1);

                    if (!visited.Contains(option))
                    {
                        queue.Add(option);
                        visited.Add(option);
                    }
                }
            }

            return distances[dest.Item1, dest.Item2];
        }

        public static char GetElevation(char x) => x switch
        {
            'S' => 'a',
            'E' => 'z',
            _ => x
        };

        private static IEnumerable<char[]> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                yield return sr.ReadLine().ToCharArray();
            }
        }
    }
}
