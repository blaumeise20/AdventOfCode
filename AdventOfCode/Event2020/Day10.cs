using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day10 : Day<int>
    {
        protected override bool Part2Done => true;

        protected override int MapLines(string line) => int.Parse(line);
        protected override int[] ManipulateLines(int[] lines) => lines.OrderBy(l => l).Prepend(0).Append(lines.Max() + 3).ToArray();

        protected override void Part1(int[] lines)
        {
            var diff1 = 0;
            var diff3 = 0;
            for (int i = 0; i < lines.Length - 1; i++)
            {
                var diff = lines[i + 1] - lines[i];
                if (diff == 1) diff1++;
                if (diff == 3) diff3++;
            }
            Print("Resulting number", diff1 * diff3, true);
        }
        protected override void Part2(int[] lines)
        {
            var steps = new List<Step>();
            steps.Add(new(0, 1));
            long result = 0;
            var max = lines.Last();
            while (steps.Count > 0)
            {
                var currentStep = steps[0];
                steps.RemoveAt(0);
                if (currentStep.Adapter < max)
                    foreach (var a in lines.Where(l => l > currentStep.Adapter && l <= currentStep.Adapter + 3))
                    {
                        var existing = steps.FirstOrDefault(s => s.Adapter == a);
                        if (existing is null)
                            steps.Add(new(a, currentStep.Count));
                        else
                            existing.Count += currentStep.Count;
                    }
                else if (currentStep.Adapter == max)
                    result += currentStep.Count;
            }
            Print("Resulting number", result, true);
        }
        class Step
        {
            public int Adapter { get; set; }
            public long Count { get; set; }

            public Step(int adapter, long count)
            {
                Adapter = adapter;
                Count = count;
            }

            public static implicit operator Step((int adapter, long count) tuple) => new Step(tuple.adapter, tuple.count);
        }
    }
}
