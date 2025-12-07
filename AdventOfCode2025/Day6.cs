#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("input.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("input.txt"));

object SolveP1(string fileName)
{
    var (nums, ops) = ParseInput(fileName);
    var results = new List<long>();
    for (var i = 0; i < ops.Length; ++i)
    {
        var res = nums[0][i];
        for (var j = 1; j < nums.Length; ++j)
        {
            switch (ops[i])
            {
                case '+':
                    res += nums[j][i];
                    break;
                case '*':
                    res *= nums[j][i];
                    break;
                case '-':
                    res -= nums[j][i];
                    break;
                case '/':
                    res /= nums[j][i];
                    break;
            }
        }
        results.Add(res);
    }

    return results.Sum();
}

object SolveP2(string fileName)
{
    var results = new List<long>();
    foreach (var (numArr, op) in ParseInput2(fileName))
    {
        var res = numArr[0];
        for (var j = 1; j < numArr.Length; ++j)
        {
            switch (op)
            {
                case '+':
                    res += numArr[j];
                    break;
                case '*':
                    res *= numArr[j];
                    break;
                case '-':
                    res -= numArr[j];
                    break;
                case '/':
                    res /= numArr[j];
                    break;
            }
        }
        results.Add(res);
    }

    return results.Sum();
}

(long[][], char[]) ParseInput(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    return (lines[..^1].Select(xl => xl.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToArray(),
        [.. lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0])]);
}

IEnumerable<(long[], char)> ParseInput2(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var numsLines = lines[..^1];
    var nums = new List<long[]>();
    var numsItem = new List<long>();
    var ops = new Stack<char>(lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]));
    for (var i = lines[0].Length - 1; i >= 0; --i)
    {
        var charsList = new List<char>();
        for (var j = 0; j < numsLines.Length; ++j)
        {
            charsList.Add(numsLines[j][i]);
        }
        var str = new string(charsList.ToArray()).Trim();

        if (!string.IsNullOrEmpty(str))
        {
            numsItem.Add(long.Parse(str));
        }

        if (string.IsNullOrEmpty(str) || i == 0)
        {
            var op = ops.Pop();
            yield return (numsItem.ToArray(), op);

            numsItem = [];
        }
    }
}