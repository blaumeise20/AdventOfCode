namespace AdventOfCode.Event2020
{
    public class Day3 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines) => Print("Resulting count", CountWithSlope(lines, 3, 1), true);
        protected override void Part2(string[] lines)
        {
            var slope1 = Print("Right 1, Down 1", (long)CountWithSlope(lines, 1, 1));
            var slope2 = Print("Right 3, Down 1", (long)CountWithSlope(lines, 3, 1));
            var slope3 = Print("Right 5, Down 1", (long)CountWithSlope(lines, 5, 1));
            var slope4 = Print("Right 7, Down 1", (long)CountWithSlope(lines, 7, 1));
            var slope5 = Print("Right 1, Down 2", (long)CountWithSlope(lines, 1, 2));
            Print("Resulting number", slope1 * slope2 * slope3 * slope4 * slope5, true);
        }

        private static int CountWithSlope(string[] lines, int right, int down)
        {
            var treeCount = 0;
            var xIndex = 0;
            for (int i = 0; i < lines.Length; i += down)
            {
                string line = lines[i];
                if (line[xIndex] == '#') treeCount++;
                xIndex = (xIndex + right) % line.Length;
            }
            return treeCount;
        }
    }
}