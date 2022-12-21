namespace AdventOfCode
{
    internal class MonkeyMath
    {
        public static long SolveP1(string fileName) => GetSubTreeValue("root", GetInput(fileName).ToDictionary(x => x.Item1, x => x.Item2));

        public static long SolveP2(string fileName)
        {
            var input = GetInput(fileName).ToDictionary(x => x.Item1, x => x.Item2);
            var (operation, operand1, operand2) = GetOperation(input["root"]);
            var opToCalc = Contains("humn", operand1, input) ? operand2 : operand1;
            var opToSearch = opToCalc == operand1 ? operand2 : operand1;
            var subtreeValue = GetSubTreeValue(opToCalc, input);
            var res = GetTermValue("humn", subtreeValue, opToSearch, input);

            return res;
        }

        private static long GetTermValue(string term, long res, string start, Dictionary<string, string> edges)
        {
            var expr = edges[start];
            if (long.TryParse(expr, out var _))
            {
                return res;
            }
            var (operation, operand1, operand2) = GetOperation(edges[start]);
            var opToCalc = Contains(term, operand1, edges) ? operand2 : operand1;
            var opToSearch = opToCalc == operand1 ? operand2 : operand1;
            var subtreeToCalcValue = GetSubTreeValue(opToCalc, edges);
            var subtreeToSearchValue = operation switch
            {
                "*" => res / subtreeToCalcValue,
                "+" => res - subtreeToCalcValue,
                "-" => opToCalc == operand1 ? subtreeToCalcValue - res: subtreeToCalcValue + res,
                "/" => opToCalc == operand1 ? subtreeToCalcValue / res : subtreeToCalcValue * res,
                _ => throw new InvalidOperationException()
            };
            return GetTermValue(term, subtreeToSearchValue, opToSearch, edges);
        }

        private static (string, string, string) GetOperation(string expr)
        {
            var operation = expr switch
            {
                _ when expr.Contains('*') => "*",
                _ when expr.Contains('+') => "+",
                _ when expr.Contains('-') => "-",
                _ when expr.Contains('/') => "/",
                _ => throw new InvalidOperationException()
            };

            var operands = expr.Split($" {operation} ")
                .ToArray();
            return (operation, operands[0], operands[1]);
        }

        private static bool Contains(string term, string start, Dictionary<string, string> edges)
        {
            var stack = new Stack<string>();
            var visited = new HashSet<string>();
            stack.Push(start);
            while(stack.Any())
            {
                var dest = stack.Peek();
                var expr = edges[dest];
                if (long.TryParse(expr, out _))
                {
                    if (dest == term)
                    {
                        return true;
                    }
                    visited.Add(dest);
                    stack.Pop();
                }
                else
                {
                    switch (GetOperation(expr))
                    {
                        case var (_, operand1, _) when !visited.Contains(operand1):
                            stack.Push(operand1);
                            break;
                        case var (_, _, operand2) when !visited.Contains(operand2):
                            stack.Push(operand2);
                            break;
                        default:
                            visited.Add(dest);
                            stack.Pop();
                            break;
                    }
                }
            }

            return false;
        }

        private static long GetSubTreeValue(string start, Dictionary<string, string> edges)
        {
            var stack = new Stack<string>();
            var results = new Dictionary<string, long>();
            stack.Push(start);
            while (stack.Any())
            {
                var dest = stack.Peek();
                var expr = edges[dest];
                if (long.TryParse(expr, out var num))
                {
                    results.Add(dest, num);
                    stack.Pop();
                }
                else
                {
                    switch(GetOperation(expr))
                    {
                        case var (_, operand1, _) when !results.ContainsKey(operand1):
                            stack.Push(operand1);
                            break;
                        case var (_, _, operand2) when !results.ContainsKey(operand2):
                            stack.Push(operand2);
                            break;
                        case var (operation, operand1, operand2):
                            var res = operation switch
                            {
                                "*" => results[operand1] * results[operand2],
                                "+" => results[operand1] + results[operand2],
                                "-" => results[operand1] - results[operand2],
                                "/" => results[operand1] / results[operand2],
                                _ => throw new InvalidOperationException()
                            };
                            results.Add(dest, res);
                            stack.Pop();
                            break;
                    }
                }
            }

            return results[start];
        }

        private static IEnumerable<(string, string)> GetInput(string fileName)
        {
            using var file = File.OpenRead(fileName);
            using StreamReader sr = new(file);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Split(": ");
                yield return (line[0], line[1]);
            }
        }
    }
}
