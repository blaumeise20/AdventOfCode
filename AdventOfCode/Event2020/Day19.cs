using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Event2020
{
    public class Day19 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines) => Do(lines);
        protected override void Part2(string[] lines) => Do(lines, true);

        private void Do(string[] lines, bool part2 = false)
        {
            var parts = string.Join('\n', lines).Split("\n\n");
            var regex = CreateRegex(parts[0].Split('\n'), part2);
            Print("Regex", regex.ToString());
            Print("Resulting count", parts[1].Split('\n').Count(l => regex.IsMatch(l)), true);
        }
        private Regex CreateRegex(string[] lines, bool part2)
        {
            var dict = new Dictionary<int, (int[][] subParts, char letter)>(lines.Select(l =>
            {
                var splitted = l.Split(": ");
                return KeyValuePair.Create(int.Parse(splitted[0]), splitted[1].StartsWith('"') ? (new[] { new[] { 0, 0 } }, splitted[1][1]) : (splitted[1].Split(" | ").Select(s => s.Split(" ").Select(n => int.Parse(n)).ToArray()).ToArray(), '\0'));
            }));
            var calculated = new Dictionary<int, string>();
            if (part2)
            {
                dict[8] = (new[] { new[] { 42 }, new[] { 42, 8 } }, '\0');
                dict[11] = (new[] { new[] { 42, 31 }, new[] { 42, 11, 31 } }, '\0');
                var regex42 = CreateRegexPart(dict, calculated, 42);
                var regex31 = CreateRegexPart(dict, calculated, 31);
                return new Regex($"^({regex42})+({regex42}({regex42}({regex42}({regex42}({regex42}({regex42}({regex42}({regex42}{regex31})?{regex31})?{regex31})?{regex31})?{regex31})?{regex31})?{regex31})?{regex31})$");
            }
            else
            {
                return new Regex($"^{CreateRegexPart(dict, calculated, 0)}$");
            }
        }
        private string CreateRegexPart(Dictionary<int, (int[][] subParts, char letter)> dict, Dictionary<int, string> calculated, int index)
        {
            if (calculated.TryGetValue(index, out var value)) return value;
            var (subParts, letter) = dict[index];
            if (letter != '\0') return letter.ToString();
            var parts = subParts.Select(sp => string.Join("", sp.Select(i => CreateRegexPart(dict, calculated, i)))).ToArray();
            var result = parts.Length > 1 ? $"({string.Join('|', parts)})" : parts[0];
            calculated.Add(index, result);
            return result;
        }
    }
}
