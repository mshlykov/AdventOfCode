namespace AdventOfCode2022
{
    internal class NotEnoughMinerals
    {
        public static long SolveP1(string fileName)
        {
            var input = GetInput(fileName).ToArray();
            long res = 0;
            var i = 1;
            foreach(var blueprint in input)
            {
                var solutions = new Dictionary<string, long>();
                var min = Solve(solutions, (24, 0, 1, 0, 0, 0, 0), blueprint);
                res += min * i;
                ++i;
            }
            
            return res;
        }

        // caution: very slow
        private static long Solve(Dictionary<string, long> solutions, (int TimeLeft, int Ore, int OreR, int Clay, int ClayR, int Obsid, int ObsidR) state, (int OreOre, int ClayOre, (int Ore, int Clay) ObsidRCost, (int Ore, int Obsid) GeodRCost) bluePrint)
        {
            var key = state.ToString();
            if (solutions.ContainsKey(key))
            {
                return solutions[key];
            }

            if (state.TimeLeft == 0)
            {
                return 0;
            }

            var (timeLeft, ore, oreR, clay, clayR, obsid, obsidR) = state;
            var canConstrOre = ore >= bluePrint.OreOre;
            var canConstrClay = ore >= bluePrint.ClayOre;
            var canConstrObsid = ore >= bluePrint.ObsidRCost.Ore && clay >= bluePrint.ObsidRCost.Clay;
            var canConstrGeode = ore >= bluePrint.GeodRCost.Ore && obsid >= bluePrint.GeodRCost.Obsid;

            timeLeft -= 1;
            ore += oreR;
            clay += clayR;
            obsid += obsidR;
            
            var res = 0L;
            if (canConstrGeode)
            {
                res = timeLeft + Solve(solutions, (timeLeft, ore - bluePrint.GeodRCost.Ore, oreR, clay, clayR, obsid - bluePrint.GeodRCost.Obsid, obsidR), bluePrint);
            }
            else 
            {
                if (canConstrOre)
                {
                    res = Math.Max(res, Solve(solutions, (timeLeft, ore - bluePrint.OreOre, oreR + 1, clay, clayR, obsid, obsidR), bluePrint));
                }
                if (canConstrClay)
                {
                    res = Math.Max(res, Solve(solutions, (timeLeft, ore - bluePrint.ClayOre, oreR, clay, clayR + 1, obsid, obsidR), bluePrint));
                }
                if (canConstrObsid)
                {
                    res = Math.Max(res, Solve(solutions, (timeLeft, ore - bluePrint.ObsidRCost.Ore, oreR, clay - bluePrint.ObsidRCost.Clay, clayR, obsid, obsidR + 1), bluePrint));
                }

                res = Math.Max(res, Solve(solutions, (timeLeft, ore, oreR, clay, clayR, obsid, obsidR), bluePrint));
            }

            solutions.Add(key, res);
            return res;
        }

        public static long SolveP2(string fileName) 
        {
            var input = GetInput(fileName).Take(3).ToArray();
            var res = 1L;
            foreach (var blueprint in input)
            {
                var solutions = new Dictionary<string, long>();
                var min = Solve(solutions, (32, 0, 1, 0, 0, 0, 0), blueprint);
                res *= min;
            }

            return res;
        }

        private static IEnumerable<(int, int, (int, int), (int, int))> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                var costs = line.Split(". ")
                    .Select(x => x.Split("costs ")[^1].Split(" and "))
                    .ToArray();
                yield return (
                        int.Parse(costs[0][0][..^4]),
                        int.Parse(costs[1][0][..^4]),
                        (int.Parse(costs[2][0][..^4]), int.Parse(costs[2][1][..^5])),
                        (int.Parse(costs[3][0][..^4]), int.Parse(costs[3][1][..^9]))
                    );
            }
        }
    }
}
