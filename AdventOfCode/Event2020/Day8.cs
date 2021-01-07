using System;
using System.Collections.Generic;

namespace AdventOfCode.Event2020
{
    public class Day8 : Day<string[]>
    {
        protected override bool Part2Done => true;

        protected override string[] MapLines(string line) => line.Split(" ");

        protected override void Part1(string[][] lines)
        {
            Print("Resulting accumulator", Run(lines).value, true);
        }
        protected override void Part2(string[][] lines)
        {
            var result = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                var instruction = lines[i][0];
                if (instruction is "nop" or "jmp")
                {
                    lines[i][0] = instruction == "jmp" ? "nop" : "jmp";
                    var (value, endlessLoop) = Run(lines);
                    if (!endlessLoop)
                    {
                        result = value;
                        break;
                    }
                    lines[i][0] = instruction == "jmp" ? "jmp" : "nop";
                }
            }
            Print("Resulting accumulator", result, true);
        }

        private (int value, bool endlessLoop) Run(string[][] lines)
        {
            var value = 0;
            var instructionIndex = 0;
            var runnedInstructions = new List<int>();
            while (instructionIndex < lines.Length)
            {
                if (runnedInstructions.Contains(instructionIndex)) return (value, true);
                runnedInstructions.Add(instructionIndex);
                var line = lines[instructionIndex];
                _ = line[0] switch
                {
                    "nop" => instructionIndex++,
                    "jmp" => instructionIndex += int.Parse(line[1]),
                    "acc" => instructionIndex++ + (value += int.Parse(line[1])),
                    _ => throw new NotImplementedException()
                };
            }
            return (value, false);
        }
    }
}
