using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Event2018
{
    public class Day1 : Day<int>
    {
        protected override bool Part2Done => true;

        protected override int MapLines(string line) => int.Parse(line);

        protected override void Part1(int[] lines) => Print("Resulting number", lines.Sum(), true);
        protected override void Part2(int[] lines)
        {
            var seenCurrencies = new HashSet<int>() { 0 };
            var i = 0;
            while (true)
            {
                var result = seenCurrencies.Last() + lines[i];
                i = (i + 1) % lines.Length;
                if (!seenCurrencies.Add(result)) { Print("Resulting number", result, true); break; }
            }
        }
    }
}
