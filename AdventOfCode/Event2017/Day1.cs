using System;
using System.Linq;

namespace AdventOfCode.Event2017
{
    public class Day1 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines) => Print("Resulting count", Solve((i, length) => (i + 1) % length, lines[0]), true);
        protected override void Part2(string[] lines) => Print("Resulting count", Solve((i, length) => (i + length / 2) % length, lines[0]), true);

        private int Solve(Func<int, int, int> lambda, string line)
        {
            var length = line.Length;
            return line.Where((c, i) => c == line[lambda(i, length)]).Select(c => int.Parse(c.ToString())).Sum();
        }
    }
}
