using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day13 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            var time = int.Parse(lines[0]);
            var busIds = lines[1].Split(",").Select(l => l == "x" ? -1 : int.Parse(l)).ToArray();

            var nearestBus = int.MaxValue;
            var nearestBusId = 0;
            foreach (var busId in busIds.Where(b => b != -1))
            {
                var busTime = 0;
                while (busTime < time)
                {
                    busTime += busId;
                }
                if (busTime < nearestBus)
                {
                    nearestBus = busTime;
                    nearestBusId = busId;
                }
            }
            Print("Resulting number", (nearestBus - time) * nearestBusId, true);
        }

        protected override void Part2(string[] lines)
        {
            var busIds = lines[1].Split(",").Select(l => l == "x" ? -1 : int.Parse(l)).ToArray();
            var busesAndTimes = busIds.Select((s, ix) => (s, ix)).Where(s => s.s != (-1)).ToList();
            long increment = busesAndTimes[0].s;
            var busIndex = 1;
            long i;
            for (i = busesAndTimes[0].s; busIndex < busesAndTimes.Count; i += increment)
            {
                if ((i + busesAndTimes[busIndex].ix) % busesAndTimes[busIndex].s == 0)
                {
                    increment *= busesAndTimes[busIndex].s;
                    busIndex++;
                }
            }
            Print("Resulting number", i - increment, true);
        }
    }
}
