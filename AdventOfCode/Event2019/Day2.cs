using System;
using System.Linq;

namespace AdventOfCode.Event2019
{
    public class Day2 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines) => Print("Resulting number", Calculate(lines, 12, 2), true);
        protected override void Part2(string[] lines)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    if (Calculate(lines.ToArray(), i, j) == 19690720)
                    {
                        Print("Noun", i, true);
                        Print("Verb", j, true);
                        Print("Resulting number", 100 * i + j, true);
                        return;
                    }
                }
            }
        }

        private static long Calculate(string[] lines, long noun, long verb)
        {
            var storage = lines[0].Split(',').Select(n => long.Parse(n)).ToArray();
            storage[1] = noun;
            storage[2] = verb;
            for (var i = 0; i < storage.Length;)
            {
                switch (storage[i])
                {
                    case 1:
                        storage[storage[i + 3]] = storage[storage[i + 1]] + storage[storage[i + 2]];
                        i += 4;
                        break;
                    case 2:
                        storage[storage[i + 3]] = storage[storage[i + 1]] * storage[storage[i + 2]];
                        i += 4;
                        break;
                    case 99:
                        return storage[0];
                    default:
                        return -1;
                }
            }
            return storage[0];
        }
    }
}
