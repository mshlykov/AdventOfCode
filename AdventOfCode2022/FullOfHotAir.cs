using System.Text;

namespace AdventOfCode2022
{
    internal class FullOfHotAir
    {
        public static string SolveP1(string fileName) => ToSnafuString(GetInput(fileName).Select(x => ParseSnafuString(x)).Sum());

        private static long ParseSnafuString(string x)
        {
            var (res, pow) = (0L, 1L);
            for (var i = x.Length - 1; i >= 0; --i)
            {
                res += x[i] switch
                {
                    '2' => 2L,
                    '1' => 1L,
                    '0' => 0L,
                    '-' => -1L,
                    '=' => -2L,
                    _ => throw new InvalidOperationException()
                } * pow;
                pow *= 5L;
            }

            return res;
        }

        private static string ToSnafuString(long x)
        {
            if (x == 0)
            {
                return "0";
            }

            var tempX = Math.Abs(x);
            var res = new StringBuilder();
            while (tempX != 0)
            {
                switch (tempX % 5)
                {
                    case 0:
                        res.Append('0');
                        break;
                    case 1:
                        res.Append('1');
                        break;
                    case 2:
                        res.Append('2');
                        break;
                    case 3:
                        res.Append('=');
                        tempX += 5;
                        break;
                    case 4:
                        res.Append('-');
                        tempX += 5;
                        break;
                }

                tempX /= 5;
            }

            if (x < 0)
            {
                for (var i = 0; i < res.Length; ++i)
                {
                    res[i] = res[i] switch
                    {
                        '-' => '1',
                        '=' => '2',
                        '1' => '-',
                        '2' => '=',
                        '0' => '0',
                        _ => throw new InvalidOperationException()
                    };
                }
            }

            for (var i = 0; i < res.Length / 2; ++i)
            {
                (res[res.Length - 1 - i], res[i]) = (res[i], res[res.Length - 1 - i]);
            }

            return res.ToString();
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
}
