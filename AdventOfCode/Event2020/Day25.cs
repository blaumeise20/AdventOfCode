using System;
using System.Diagnostics;

namespace AdventOfCode.Event2020
{
    public class Day25 : Day<int>
    {
        protected override bool Part2Done => false;

        protected override int MapLines(string line) => int.Parse(line);

        protected override void Part1(int[] lines)
        {
            Print("Resulting key", Calculate(GetLoopSize(lines[0]), lines[1]), true);
        }

        private static int GetLoopSize(int publicKey)
        {
            var loopSize = 0;
            var currentNumber = 1;
            while (currentNumber != publicKey)
            {
                currentNumber = currentNumber * 7 % 20201227;
                loopSize++;
            }
            return loopSize;
        }
        private static long Calculate(long loopSize, long subject)
        {
            var currentNumber = 1L;
            while (loopSize-- != 0) currentNumber = currentNumber * subject % 20201227;
            return currentNumber;
        }


        protected override void Part2(int[] lines)
        {
            throw new NotImplementedException();
        }
    }
}
