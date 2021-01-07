using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day15 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            Run(lines[0].Split(",").Select(n => int.Parse(n)).ToArray(), 2020);
        }

        protected override void Part2(string[] lines)
        {
            Run(lines[0].Split(",").Select(n => int.Parse(n)).ToArray(), 30000000);
        }

        private void Run(int[] numbers, int numToCheck)
        {
            var indexes = new Dictionary<int, List<int>>();
            var numbersSpoken = new List<int>();
            for (int i = 0; i < numToCheck; i++)
            {
                if (i < numbers.Length)
                {
                    numbersSpoken.Add(numbers[i]);
                    indexes.Add(numbers[i], new()
                    {
                        i
                    });
                    continue;
                }
                var lastNumber = numbersSpoken[i - 1];
                var currentIndexes = indexes[lastNumber];
                if (currentIndexes.Count == 1)
                {
                    numbersSpoken.Add(0);
                    if (indexes.ContainsKey(0))
                    {
                        indexes[0].Add(i);
                        if (indexes[0].Count > 2) indexes[0].RemoveAt(0);
                    }
                    else indexes.Add(0, new()
                        {
                            i
                        });
                }
                else
                {
                    var newNumber = currentIndexes[1] - currentIndexes[0];
                    numbersSpoken.Add(newNumber);
                    if (indexes.ContainsKey(newNumber))
                    {
                        indexes[newNumber].Add(i);
                        if (indexes[newNumber].Count > 2) indexes[newNumber].RemoveAt(0);
                    }
                    else indexes.Add(newNumber, new()
                        {
                            i
                        });
                }
            }
            Print("Resulting number", numbersSpoken.Last(), true);
        }
    }
}
