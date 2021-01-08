using System.Linq;

namespace AdventOfCode.Event2017
{
    public class Day2 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines) => Print("Resulting number", lines.Select(l => l.Split("\t").Select(n => int.Parse(n))).Select(s => s.Max() - s.Min()).Sum(), true);
        protected override void Part2(string[] lines)
        {
            Print("Resulting number", lines.Select(l => l.Split("\t").Select(n => int.Parse(n))).Select(s =>
            {
                foreach (var n1 in s)
                    foreach (var n2 in s)
                        if (n1 != n2 && n1 % n2 == 0) return n1 / n2;
                return 0;
            }).Sum(), true);
        }
    }
}
