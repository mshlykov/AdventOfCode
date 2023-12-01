namespace AdventOfCode2022
{
    internal class TreetopTreeHouse
    {
        public static int SolveP1(string fileName)
        {
            var trees = GetInput(fileName);
            var res = 0;
            for (var i = 0; i < trees.Length; ++i)
            {
                for (var j = 0; j < trees[i].Length; ++j)
                {
                    var visibleLeft = true;

                    for (var t = 0; t < j; ++t)
                    {
                        if (trees[i][j] <= trees[i][t])
                        {
                            visibleLeft = false;
                        }
                    }

                    var visibleRigth = true;
                    for (var t = j + 1; t < trees[i].Length; ++t)
                    {
                        if (trees[i][j] <= trees[i][t])
                            visibleRigth = false;
                    }

                    var visibleTop = true;
                    for (var t = 0; t < i; ++t)
                    {
                        if (trees[i][j] <= trees[t][j])
                            visibleTop = false;
                    }

                    var visibleBottom = true;
                    for (var t = i + 1; t < trees.Length; ++t)
                    {
                        if (trees[i][j] <= trees[t][j])
                            visibleBottom = false;
                    }

                    if (visibleLeft || visibleRigth || visibleTop || visibleBottom) res++;
                }
            }

            return res;
        }


        public static int SolveP2(string fileName)
        {
            var trees = GetInput(fileName);
            var res = -1;
            for (var i = 0; i < trees.Length; ++i)
            {
                for (var j = 0; j < trees[i].Length; ++j)
                {
                    var visibleLeft = 0;

                    for (var t = j - 1; t >= 0; --t)
                    {
                        visibleLeft++;
                        if (trees[i][j] <= trees[i][t])
                        {
                            break;
                        }
                    }

                    var visibleRigth = 0;
                    for (var t = j + 1; t < trees[i].Length; ++t)
                    {
                        visibleRigth++;
                        if (trees[i][j] <= trees[i][t])
                        {
                            break;
                        }
                    }

                    var visibleTop = 0;
                    for (var t = i - 1; t >= 0; --t)
                    {
                        visibleTop++;
                        if (trees[i][j] <= trees[t][j])
                        {
                            break;
                        }
                    }

                    var visibleBottom = 0;
                    for (var t = i + 1; t < trees.Length; ++t)
                    {
                        visibleBottom++;
                        if (trees[i][j] <= trees[t][j])
                        {
                            break;
                        }
                    }

                    res = Math.Max(res, visibleLeft * visibleRigth * visibleTop * visibleBottom);
                }
            }

            return res;
        }

        private static int[][] GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            var res = new List<int[]>();
            while (!sr.EndOfStream)
            {
                res.Add(sr.ReadLine().Select(x => int.Parse(new string(new[] { x }))).ToArray());
            }
            return res.ToArray();
        }
    }
}
