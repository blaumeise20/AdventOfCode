using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day2 : Day<Day2.Password>
    {
        protected override bool Part2Done => true;

        protected override Password MapLines(string line) => new Password(line);

        protected override void Part1(Password[] passwords) => Print("Resulting count", passwords.Count(p => p.IsValidPart1()), true);
        protected override void Part2(Password[] passwords) => Print("Resulting count", passwords.Count(p => p.IsValidPart2()), true);

        public class Password
        {
            public string PW { get; }
            public int RangeStart { get; }
            public int RangeEnd { get; }
            public char CharToCheck { get; }

            public Password(string text)
            {
                var splitted = text.Split(": ");
                PW = splitted[1];
                var rangeAndChar = splitted[0].Split(" ");
                CharToCheck = rangeAndChar[1][0];
                var range = rangeAndChar[0].Split("-");
                RangeStart = int.Parse(range[0]);
                RangeEnd = int.Parse(range[1]);
            }

            public bool IsValidPart1()
            {
                var count = 0;
                foreach (var letter in PW) if (letter == CharToCheck) count++;
                return count >= RangeStart && count <= RangeEnd;
            }
            public bool IsValidPart2()
            {
                return PW[RangeStart - 1] == CharToCheck ^ PW[RangeEnd - 1] == CharToCheck;
            }
        }
    }
}
