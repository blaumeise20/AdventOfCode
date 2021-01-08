using System.Linq;

namespace AdventOfCode.Event2018
{
    public class Day2 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            var two = lines.Where(l => l.Any(c => l.Count(lc => lc == c) == 2)).Count();
            var three = lines.Where(l => l.Any(c => l.Count(lc => lc == c) == 3)).Count();
            Print("Resulting number", two * three, true);
        }
        protected override void Part2(string[] lines)
        {
            for (int i = 0; i < lines.Length - 1; i++)
            {
                for (int j = i + 1; j < lines.Length; j++)
                {
                    var (line1, line2) = (lines[i], lines[j]);
                    var correctChars = string.Join("", line1.Where((c, i) => c == line2[i]));
                    if (line1.Length - correctChars.Length == 1)
                    {
                        Print("Box 1", line1);
                        Print("Box 2", line2);
                        Print("Resulting chars", correctChars, true);
                        return;
                    }
                }
            }
        }
    }
}
