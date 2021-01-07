using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Event2020
{
    public class Day7 : Day
    {
        protected override bool Part2Done => true;

        protected override string MapLines(string line) => line[0..^1];

        protected override void Part1(string[] lines)
        {
            bool Check(string line) => !line.EndsWith("no other bags") && (line.Split(" bags contain ")[1].Contains("shiny gold") || line.Split(" bags contain ")[1].Split(", ").Any(b => Check(lines.FirstOrDefault(l => l.StartsWith(Regex.Match(b, "^\\d+ ([a-z ]+) bags?$").Groups[1].Value)))));
            Print("Resulting count", lines.Where(Check).Count(), true);
        }
        protected override void Part2(string[] lines)
        {
            int CountInBag(string bag) => 1 + lines.FirstOrDefault(l => l.StartsWith(bag)).Split(" bags contain ")[1].Split(", ").Sum(b =>
            {
                var match = Regex.Match(b, "^(\\d+) ([a-z ]+) bags?$").Groups;
                return b == "no other bags" ? 0 : int.Parse(match[1].Value) * CountInBag(match[2].Value);
            });
            Print("Resulting count", CountInBag("shiny gold") - 1, true);
        }
    }
}
