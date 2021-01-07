using System;

namespace AdventOfCode.Event2020
{
    public class Day12 : Day<(char instruction, int value)>
    {
        protected override bool Part2Done => true;

        protected override (char instruction, int value) MapLines(string line) => (line[0], int.Parse(line[1..]));

        protected override void Part1((char instruction, int value)[] lines)
        {
            var currentX = 0;
            var currentY = 0;
            var facing = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var instruction = lines[i];
                switch (instruction.instruction)
                {
                    case 'N':
                        currentY += instruction.value;
                        break;
                    case 'S':
                        currentY -= instruction.value;
                        break;
                    case 'E':
                        currentX += instruction.value;
                        break;
                    case 'W':
                        currentX -= instruction.value;
                        break;
                    case 'L':
                        facing -= instruction.value / 90;
                        facing = (facing + 4) % 4;
                        break;
                    case 'R':
                        facing += instruction.value / 90;
                        facing %= 4;
                        break;
                    case 'F':
                        switch (facing)
                        {
                            case 0:
                                currentX += instruction.value;
                                break;
                            case 1:
                                currentY -= instruction.value;
                                break;
                            case 2:
                                currentX -= instruction.value;
                                break;
                            case 3:
                                currentY += instruction.value;
                                break;
                        }
                        break;
                }
            }
            PrintResult(currentX, currentY);
        }
        protected override void Part2((char instruction, int value)[] lines)
        {
            var currentX = 0;
            var waypointX = 10;
            var currentY = 0;
            var waypointY = 1;
            void Rotate(int amount)
            {
                var newWaypointX = amount == 1 ? -waypointY : amount == 2 ? -waypointX : waypointY;
                var newWaypointY = amount == 1 ? waypointX : amount == 2 ? -waypointY : -waypointX;
                waypointX = newWaypointX;
                waypointY = newWaypointY;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                var instruction = lines[i];
                switch (instruction.instruction)
                {
                    case 'N':
                        waypointY += instruction.value;
                        break;
                    case 'S':
                        waypointY -= instruction.value;
                        break;
                    case 'E':
                        waypointX += instruction.value;
                        break;
                    case 'W':
                        waypointX -= instruction.value;
                        break;
                    case 'L':
                        Rotate(instruction.value / 90);
                        break;
                    case 'R':
                        Rotate(4 - instruction.value / 90);
                        break;
                    case 'F':
                        var newX = instruction.value * Math.Abs(waypointX);
                        if (waypointX > 0)
                        {
                            currentX += newX;
                        }
                        else
                        {
                            currentX -= newX;
                        }
                        var newY = instruction.value * Math.Abs(waypointY);
                        if (waypointY > 0)
                        {
                            currentY += newY;
                        }
                        else
                        {
                            currentY -= newY;
                        }
                        break;
                }
            }
            PrintResult(currentX, currentY);
        }

        private void PrintResult(long currentX, long currentY)
        {
            long newX = Math.Abs(currentX);
            long newY = Math.Abs(currentY);
            Print("Resulting " + (currentX < 0 ? "south" : "north"), newX);
            Print("Resulting " + (currentY < 0 ? "west" : "east"), newY);
            Print("Resulting distance", newX + newY, true);
        }
    }
}
