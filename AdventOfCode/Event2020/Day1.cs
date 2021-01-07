namespace AdventOfCode.Event2020
{
    public class Day1 : Day<int>
    {
        protected override bool Part2Done => true;

        protected override int MapLines(string line) => int.Parse(line);

        protected override void Part1(int[] numbers)
        {
            var done = false;
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    var x = numbers[i];
                    var y = numbers[j];
                    if (x + y == 2020)
                    {
                        Print("Numbers found", $"{x} + {y} = 2020");
                        Print("Resulting number", x * y, true);
                        done = true;
                        break;
                    }
                }
                if (done) break;
            }
        }
        protected override void Part2(int[] numbers)
        {
            var done = false;
            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    for (int k = i + 1; k < numbers.Length; k++)
                    {
                        var x = numbers[i];
                        var y = numbers[j];
                        var z = numbers[k];
                        if (x + y + z == 2020)
                        {
                            Print("Numbers found", $"{x} + {y} + {z} = 2020");
                            Print("Resulting number", x * y * z, true);
                            done = true;
                            break;
                        }
                    }
                    if (done) break;
                }
                if (done) break;
            }
        }
    }
}
