namespace AdventOfCode2023;

internal class Day19
{
    public static object SolveP1(string fileName)
    {
        var (rules, details) = GetInput(fileName);
        return details.Where(x => Eval(x, rules) == "A").Sum(x => x["x"] + x["m"] + x["a"] + x["s"]);
    }

    public static object SolveP2(string fileName)
    {
        var (rules, _) = GetInput(fileName);
        var dict = new Dictionary<string, (long Min, long Max)>()
        {
            { "x", (1, 4000) },
            { "m", (1, 4000) },
            { "a", (1, 4000) },
            { "s", (1, 4000) },
        };

        return GetConstraints("in", dict, rules).Sum(x => (x["x"].Max - x["x"].Min + 1) *
        (x["m"].Max - x["m"].Min + 1) *
        (x["a"].Max - x["a"].Min + 1) *
        (x["s"].Max - x["s"].Min + 1));
    }

    private static IEnumerable<Dictionary<string, (long Min, long Max)>> GetConstraints(string name, Dictionary<string, (long Min, long Max)> cons, Dictionary<string, List<(string, string, long?, string)>> rulesData)
    {
        if (name == "A")
        {
            return new[] { cons };
        }
        if (name == "R")
        {
            return Enumerable.Empty<Dictionary<string, (long Min, long Max)>>();
        }

        var rules = rulesData[name];

        var res = new List<Dictionary<string, (long Min, long Max)>>();
        var consCopy = new Dictionary<string, (long Min, long Max)>(cons);
        foreach (var rule in rules)
        {
            var newDict = new Dictionary<string, (long Min, long Max)>(consCopy);
            if (string.IsNullOrEmpty(rule.Item1))
            {
                res.AddRange(GetConstraints(rule.Item4, newDict, rulesData));
            }
            else
            {
                var (min, max) = consCopy[rule.Item1];
                switch (rule.Item2)
                {
                    case "<" when rule.Item3 > min:
                        newDict[rule.Item1] = (min, Math.Min(max, rule.Item3.Value) - 1);
                        consCopy[rule.Item1] = (Math.Min(max, rule.Item3.Value), max);
                        res.AddRange(GetConstraints(rule.Item4, newDict, rulesData));
                        break;
                    case ">" when rule.Item3 < max:
                        newDict[rule.Item1] = (Math.Max(min, rule.Item3.Value) + 1, max);
                        consCopy[rule.Item1] = (min, Math.Max(min, rule.Item3.Value));
                        res.AddRange(GetConstraints(rule.Item4, newDict, rulesData));
                        break;
                    default:
                        throw new Exception();
                }

                if (consCopy[rule.Item1].Min > consCopy[rule.Item1].Max)
                {
                    break;
                }
            }
        }

        return res;
    }

    private static string Eval(Dictionary<string, long> detail, Dictionary<string, List<(string, string, long?, string)>> rulesData)
    {
        var currState = "in";
        while (currState != "A" && currState != "R")
        {
            var rules = rulesData[currState];
            var newState = default(string);
            foreach (var rule in rules)
            {
                newState = EvalRule(detail, rule);
                if (!string.IsNullOrEmpty(newState))
                {
                    break;
                }
            }

            currState = newState;
        }

        return currState;
    }

    private static string EvalRule(Dictionary<string, long> detail, (string, string, long?, string) rule)
    {
        if (string.IsNullOrEmpty(rule.Item1))
        {
            return rule.Item4;
        }

        var match = rule.Item2 switch
        {
            ">" => detail[rule.Item1] > rule.Item3.Value,
            "<" => detail[rule.Item1] < rule.Item3.Value,
            _ => throw new Exception()
        };

        return match ? rule.Item4 : null;
    }

    private static (Dictionary<string, List<(string, string, long?, string)>> Rules, List<Dictionary<string, long>> Details) GetInput(string fileName)
    {
        using var file = File.OpenRead(fileName);
        using StreamReader sr = new(file);

        var line = sr.ReadLine();
        var rulesData = new Dictionary<string, List<(string, string, long?, string)>>();
        while (!string.IsNullOrWhiteSpace(line))
        {
            var ruleStart = line.IndexOf("{");
            var name = line[..ruleStart];
            var rulesStrs = line[ruleStart..].Trim('{', '}').Split(",");
            var rules = new List<(string, string, long?, string)>();
            foreach (var ruleStr in rulesStrs)
            {
                if (ruleStr.Contains(':'))
                {
                    var idx = ruleStr.IndexOf(':');
                    var field = ruleStr[..1];
                    var op = ruleStr[1..2];
                    var val = long.Parse(ruleStr[2..idx]);
                    var dest = ruleStr[(idx + 1)..];
                    rules.Add((field, op, val, dest));
                }
                else
                {
                    rules.Add((null, null, null, ruleStr));
                }
            }

            rulesData.Add(name, rules);
            line = sr.ReadLine();
        }

        line = sr.ReadLine();
        var details = new List<Dictionary<string, long>>();
        while (!string.IsNullOrEmpty(line))
        {
            var vals = new Dictionary<string, long>();
            line = line.Trim('{', '}');
            var items = line.Split(",")
                .Select(x =>
                {
                    var data = x.Split("=");
                    return (data[0], long.Parse(data[1]));
                })
                .ToArray();

            foreach (var item in items)
            {
                vals.Add(item.Item1, item.Item2);
            }

            details.Add(vals);
            line = sr.ReadLine();
        }

        return (rulesData, details);
    }
}
