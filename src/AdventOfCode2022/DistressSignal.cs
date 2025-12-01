namespace AdventOfCode2022
{
    internal class DistressSignal
    {
        public static int SolveP1(string fileName) => GetInput(fileName)
            .Select((x, i) => (x.Item1.CompareTo(x.Item2), i + 1))
            .Where(x => x.Item1 == -1)
            .Sum(x => x.Item2);

        public static int SolveP2(string fileName)
        {
            var dp1 = Packet.Create(new List<Packet>
            {
                Packet.Create(new List<Packet>
                {
                    Packet.Create(2)
                })
            });

            var dp2 = Packet.Create(new List<Packet>
            { 
                Packet.Create(new List<Packet>
                {
                    Packet.Create(6)
                })
            });

            var input = GetInput(fileName)
                .SelectMany(x => new[] { x.Item1, x.Item2 })
                .Concat(new[] { dp1, dp2 })
                .OrderBy(x => x)
                .ToList();

            return (input.IndexOf(dp1) + 1) * (input.IndexOf(dp2) + 1);
        }

        private static IEnumerable<(Packet, Packet)> GetInput(string fileName)
        {
            return File.ReadAllText(fileName)
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(x =>
                {
                    var packets = x.Split(Environment.NewLine).Select(y => Packet.Parse(y)).ToArray();
                    return (packets[0], packets[1]);
                })
                .ToArray();
        }

        abstract record Packet : IComparable<Packet>
        {
            record IntPacket(int Content) : Packet;
            record ListPacket(List<Packet> Content) : Packet;

            public static Packet Create(int content) => new IntPacket(content);

            public static Packet Create(List<Packet> content) => new ListPacket(content);

            public static Packet Parse(string str)
            {
                var stack = new Stack<Packet>();
                stack.Push(Create(new List<Packet>()));
                for (var i = 0; i < str.Length; ++i)
                {
                    if (str[i] == '[')
                    {
                        var currPacket = stack.Peek() as ListPacket;
                        var newPacket = Create(new List<Packet>());
                        currPacket.Content.Add(newPacket);
                        stack.Push(newPacket);
                    }
                    else if (str[i] == ']')
                    {
                        stack.Pop();
                    }
                    else if (str[i] != ',')
                    {
                        var j = i + 1;
                        for (; str[j] != ',' && str[j] != ']' && j < str.Length; ++j) { }

                        var currPacket = stack.Peek() as ListPacket;
                        currPacket.Content.Add(Create(int.Parse(str[i..j])));
                        i = j - 1;
                    }
                }

                return stack.Peek();
            }

            private static int CompareTo(Packet p1, Packet p2)
            {
                if (p1 is IntPacket p1Int && p2 is IntPacket p2Int)
                {
                    return p1Int.Content.CompareTo(p2Int.Content);
                }

                var p1Comp = (p1 as ListPacket) ?? (Create(new List<Packet>() { p1 }) as ListPacket);
                var p2Comp = (p2 as ListPacket) ?? (Create(new List<Packet>() { p2 }) as ListPacket);
                var len = Math.Min(p1Comp.Content.Count, p2Comp.Content.Count);
                for (var i = 0; i < len; ++i)
                {
                    var comp = p1Comp.Content[i].CompareTo(p2Comp.Content[i]);
                    if (comp != 0)
                    {
                        return comp;
                    }
                }

                return p1Comp.Content.Count.CompareTo(p2Comp.Content.Count);
            }

            public int CompareTo(Packet? other) => CompareTo(this, other);
        }
    }
}
