using System;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day5 : Day<int>
    {
        protected override bool Part2Done => true;

        protected override int MapLines(string line) => Convert.ToInt32(line.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1'), 2);

        protected override void Part1(int[] seats) => Print("Highest seat ID", seats.Max(), true);
        protected override void Part2(int[] seats)
        {
            for (int i = 0; i < seats.Length; i++)
            {
                for (int j = 0; j < seats.Length; j++)
                {
                    var a = seats[i];
                    var b = seats[j];
                    if (a - b == 2 && !seats.Contains(a - 1))
                    {
                        Print("Seats found", $"{b}, {a}");
                        Print("Resulting number", a - 1, true);
                    }
                }
            }
        }
    }
}