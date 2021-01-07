using System;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day9 : Day<long>
    {
        protected override bool Part2Done => true;

        protected override long MapLines(string line) => long.Parse(line);

        protected override void Part1(long[] lines)
        {
            Print("Resulting number", FindWeakNumber(lines, 25), true);
        }
        protected override void Part2(long[] lines)
        {
            var number = FindWeakNumber(lines, 25);
            for (int i = 0; i < lines.Length - 1; i++)
            {
                for (int j = i + 1; j < lines.Length; j++)
                {
                    var range = lines[i..j];
                    if (range.Sum() == number)
                    {
                        Print("Resulting number", range.Min() + range.Max(), true);
                        return;
                    }
                }
            }
        }

        private static long FindWeakNumber(long[] lines, int preambleLength)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i + preambleLength];
                var previousLines = lines[i..(i + preambleLength)];
                if (!previousLines.Any(l => previousLines.Any(l2 => l + l2 == line)))
                    return line;
            }
            return 0;
        }
    }
}
