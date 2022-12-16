namespace AdventOfCode
{
    internal class ProboscideaVolcanium
    {
        public static long SolveP1(string fileName)
        {
            var graph = GetInput(fileName).ToDictionary(x => x.Valve);
            var weightedFullGraph = graph.Keys.Select(x => (Valve: x, graph[x].Flow, Valves: Dijkstra(x, graph)))
                .ToArray();
            var zeroFlowValves = weightedFullGraph.Where(x => x.Flow == 0 && x.Valve != "AA")
                .Select(x => x.Valve)
                .ToArray();
            var weightedGraph = weightedFullGraph.Where(x => !zeroFlowValves.Contains(x.Valve))
                .ToDictionary(x => x.Valve, x => (x.Flow, Valves: x.Valves.Where(x => !zeroFlowValves.Contains(x.Valve)).ToDictionary(x => x.Valve, x => x.Distance)));
            var vertices = weightedGraph.Keys.ToArray();
            var solutions = new Dictionary<string, long>();
            var res = Solve(30, "AA", vertices, solutions, weightedGraph);

            return res;
        }

        private static long Solve(int time, string start, IEnumerable<string> valves, Dictionary<string, long> solutions, Dictionary<string, (int Flow, Dictionary<string, int> Valves)> weightedGraph)
        {
            var key = $"{time}|{start}|{string.Join(", ", valves.OrderBy(x => x))}";
            if (solutions.ContainsKey(key))
            {
                return solutions[key];
            }

            var res = 0L;
            var valvesToVisit = valves.Where(x => x != start)
                .ToArray();
            foreach (var valve in weightedGraph[start].Valves.Keys.Intersect(valvesToVisit))
            {
                var timeLeft = time - (weightedGraph[start].Valves[valve] + 1);
                if (timeLeft <= 0)
                {
                    continue;
                }
                var recSolve = Solve(timeLeft, valve, valvesToVisit, solutions, weightedGraph);
                res = Math.Max(res, weightedGraph[valve].Flow * timeLeft + recSolve);
            }

            solutions.Add(key, res);

            return res;
        }

        public static long SolveP2(string fileName)
        {
            var graph = GetInput(fileName).ToDictionary(x => x.Valve);
            var weightedFullGraph = graph.Keys.Select(x => (Valve: x, graph[x].Flow, Valves: Dijkstra(x, graph)))
                .ToArray();
            var zeroFlowValves = weightedFullGraph.Where(x => x.Flow == 0 && x.Valve != "AA")
                .Select(x => x.Valve)
                .ToArray();
            var weightedGraph = weightedFullGraph.Where(x => !zeroFlowValves.Contains(x.Valve))
                .ToDictionary(x => x.Valve, x => (x.Flow, Valves: x.Valves.Where(x => !zeroFlowValves.Contains(x.Valve)).ToDictionary(x => x.Valve, x => x.Distance)));
            var vertices = weightedGraph.Keys.ToArray();
            var solutions = new Dictionary<string, long>();
            var res = Solve2((26, "AA"), (26, "AA"), vertices, solutions, weightedGraph);

            return res;
        }

        private static long Solve2((int Time, string Start) p1, (int Time, string Start) p2, IEnumerable<string> valves, Dictionary<string, long> solutions, Dictionary<string, (int Flow, Dictionary<string, int> Valves)> weightedGraph)
        {
            var max = p1.CompareTo(p2) == 1 ? p1 : p2;
            var min = p1.CompareTo(p2) == 1 ? p2 : p1;
            var key = $"{string.Join(",", new[] { min, max }.OrderBy(x => x))}|{string.Join(", ", valves.OrderBy(x => x))}";
            if (solutions.ContainsKey(key))
            {
                return solutions[key];
            }

            var res = 0L;
            var valvesToVisit = valves.Where(x => x != p1.Start && x != p2.Start)
                .ToArray();
            var (time, start) = max;
            foreach (var valve in weightedGraph[start].Valves.Keys.Intersect(valvesToVisit))
            {
                var timeLeft = time - (weightedGraph[start].Valves[valve] + 1);
                if (timeLeft <= 0)
                {
                    continue;
                }
                var recSolve = Solve2((timeLeft, valve), min, valvesToVisit.Except(new[] { start }).ToArray(), solutions, weightedGraph);
                res = Math.Max(res, weightedGraph[valve].Flow * timeLeft + recSolve);
            }

            solutions.Add(key, res);

            return res;
        }

        private static IEnumerable<(string Valve, int Distance)> Dijkstra(string start, Dictionary<string, (string Valve, int Flow, string[] Valves)> graph)
        {
            var distances = graph.Keys.ToDictionary(x => x, x => int.MaxValue);
            var queue = new List<string> { start };
            var set = new HashSet<string>
            {
                start
            };
            distances[start] = 0;
            while (queue.Any())
            {
                var min = queue.Min(x => distances[x]);
                var valve = queue.Find(x => distances[x] == min);
                queue.Remove(valve);
                foreach (var neighbour in graph[valve].Valves)
                {
                    if (!set.Contains(neighbour))
                    {
                        set.Add(neighbour);
                        queue.Add(neighbour);
                    }

                    distances[neighbour] = Math.Min(distances[valve] + 1, distances[neighbour]);
                }
            }

            return distances.Select(x => (x.Key, x.Value)).Where(x => x.Key != start && x.Value != int.MaxValue).ToArray();
        }

        private static IEnumerable<(string Valve, int Flow, string[] Valves)> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Split("; ");
                var valve = line[0][6..8];
                var flow = int.Parse(line[0].Split("=").Last());
                var valves = (line[1].StartsWith("tunnels") ? line[1][23..] : line[1][22..]).Split(", ");
                yield return (valve, flow, valves);
            }
        }
    }
}