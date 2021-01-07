using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day6 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            Print("Resulting count", string.Join("\n", lines).Split("\n\n").Select(l => l.Replace("\n", "").Distinct().Count()).Sum(), true);
        }
        protected override void Part2(string[] lines)
        {
            Print("Resulting count", string.Join("\n", lines).Split("\n\n").Select(l => {
                var dist = l.Replace("\n", "").Distinct();
                var newlines = l.Where(c => c == '\n').Count();
                return dist.Where(d => l.Where(c => c == d).Count() == newlines + 1).Count();
            }).Sum(), true);
        }
    }
}
