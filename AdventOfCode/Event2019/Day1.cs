using System;
using System.Linq;

namespace AdventOfCode.Event2019
{
    public class Day1 : Day<int>
    {
        protected override bool Part2Done => true;

        protected override int MapLines(string line) => int.Parse(line);

        protected override void Part1(int[] lines) => Print("Total fuel required", lines.Select(CalcFuel).Sum(), true);
        protected override void Part2(int[] lines) => Print("Total fuel required", lines.Select(CalcFuel2).Sum(), true);

        private int CalcFuel2(int input)
        {
            int totalFuel, lastFuel = totalFuel = CalcFuel(input);
            while (lastFuel > 0) totalFuel += Math.Max(lastFuel = CalcFuel(lastFuel), 0);
            return totalFuel;
        }
        private int CalcFuel(int input) => input / 3 - 2;
    }
}
