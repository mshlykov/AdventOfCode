namespace AdventOfCode2024;

internal class Day17
{
    public static object SolveP1(string fileName)
    {
        var (regs, prog) = GetInput(fileName);

        var output = new List<long>();
        for (var ip = 0; ip < prog.Length;)
        {
            (regs, var newIp, var instrOutput) = Execute(prog[ip], prog[ip + 1], regs);
            if (instrOutput != null)
            {
                output.Add(instrOutput.Value);
            }

            ip = newIp ?? (ip + 2);
        }

        return string.Join(",", output);
    }

    public static object SolveP2(string fileName)
    {
        var (regs, prog) = GetInput(fileName);

        if (fileName != "2.txt")
        {
            // recursive exhaustive search, adjusted to reverse engineered input programme below

            // 0: b = a % 8
            // 2: b = b ^ 7
            // 4: c = (long)(a / 2 ** b)
            // 6: b = b ^ 7
            // 8: b = b ^ c
            // 10: a = (long)(a / 2 ** 3)
            // 12: out b % 8
            // 14: if(a != 0) goto 0

            return Search(0L, prog.Length - 1, prog);
        }
        else
        {
            // recursive exhaustive search, adjusted to reverse engineered test input programme below
            //0: a = (int)a / 2 ** 3
            //2: out a % 8
            //4: if (a != 0) goto 0

            var a = 0L;
            for (var i = prog.Length - 1; i >= 0; --i)
            {
                a += prog[i];
                a *= 8L;
            }

            return a;
        }
    }

    private static long? Search(long currA, int inputIdx, int[] prog)
    {
        if (inputIdx < 0)
        {
            return currA;
        }

        var j = 0;
        for (; j < 8; ++j)
        {
            var a = currA * 8 + j;
            var b = (long)j;
            b ^= 7;
            var c = (long)(a / Math.Pow(2.0, b));
            b ^= 7;
            b ^= c;
            if ((b % 8) == prog[inputIdx])
            {
                var res = Search(a, inputIdx - 1, prog);
                if (res != null)
                {
                    return res;
                }
            }
        }

        return null;
    }

    private static ((long A, long B, long C) regs, int? ip, long? output) Execute(int instr, int operand, (long A, long B, long C) regs)
    {
        var (a, b, c) = regs;
        switch (instr)
        {
            case 0:
                a = (long)(a / Math.Pow(2.0, GetOperandValue(operand, regs, false)));
                return ((a, b, c), null, null);
            case 1:
                b ^= GetOperandValue(operand, regs, true);
                return ((a, b, c), null, null);
            case 2:
                b = GetOperandValue(operand, regs, false) % 8;
                return ((a, b, c), null, null);
            case 3:
                if (a == 0)
                {
                    return ((a, b, c), null, null);
                }

                return ((a, b, c), (int)GetOperandValue(operand, regs, true), null);
            case 4:
                b ^= c;
                return ((a, b, c), null, null);
            case 5:
                return ((a, b, c), null, GetOperandValue(operand, regs, false) % 8);
            case 6:
                b = (long)(a / Math.Pow(2.0, GetOperandValue(operand, regs, false)));
                return ((a, b, c), null, null);
            case 7:
                c = (long)(a / Math.Pow(2.0, GetOperandValue(operand, regs, false)));
                return ((a, b, c), null, null);
            default:
                throw new NotImplementedException();
        }
    }

    private static long GetOperandValue(long value, (long A, long B, long C) regs, bool isLiteral) => value switch
    {
        0 => 0,
        1 => 1,
        2 => 2,
        3 => 3,
        4 => isLiteral ? 4 : regs.A,
        5 => isLiteral ? 5 : regs.B,
        6 => isLiteral ? 6 : regs.C,
        7 => 7,
        _ => throw new InvalidOperationException()
    };

    private static ((long A, long B, long C) regs, int[] prog) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        var regs = new List<long>();
        for (var i = 0; i < 3; ++i)
        {
            regs.Add(long.Parse(sr.ReadLine()[12..]));
        }
        sr.ReadLine();
        var prog = sr.ReadLine()[9..]
            .Split(",")
            .Select(int.Parse)
            .ToArray();

        return ((regs[0], regs[1], regs[2]), prog);
    }
}
