#:sdk Microsoft.Net.Sdk

Console.WriteLine(SolveP1("1.txt"));
Console.WriteLine(SolveP1("day1.txt"));
Console.WriteLine(SolveP2("2.txt"));
Console.WriteLine(SolveP2("day1.txt"));

object SolveP1(string fileName)
{
    var current = 50L;
    var password = 0L;
    foreach (var (direction, step) in ParseInstructions(fileName))
    {
        var res = direction switch
        {
            "L" => current - step,
            "R" => current + step,
            _ => throw new InvalidOperationException(),
        };

        res = ((res % 100) + 100) % 100;
        password += res == 0 ? 1 : 0;
        
        current = res;
    }

    return password;
}

object SolveP2(string fileName)
{
    var current = 50L;
    var password = 0L;
    foreach (var (direction, step) in ParseInstructions(fileName))
    {
        var res = direction switch
        {
            "L" => current - step,
            "R" => current + step,
            _ => throw new InvalidOperationException(),
        };

        password += Math.Abs(res / 100) + (res <= 0 && current > 0 ? 1 : 0);
        res = ((res % 100) + 100) % 100;

        current = res;
    }

    return password;
}

IEnumerable<Instruction> ParseInstructions(string fileName)
{
    using var file = File.OpenRead(fileName);
    using StreamReader sr = new(file);
    while (!sr.EndOfStream)
    {
        var line = sr.ReadLine() ?? throw new InvalidOperationException();
        yield return new Instruction(line[0..1], int.Parse(line[1..]));
    }
}

record Instruction(string Direction, int Steps);