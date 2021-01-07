using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day11 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            Print("Resulting count", Solve(4, GetSurrounding, lines), true);
        }
        protected override void Part2(string[] lines)
        {
            Print("Resulting count", Solve(5, GetSeen, lines), true);
        }

        private static int Solve(int seatsNeeded, Func<char[][], int, int, int> surrounding, string[] lines)
        {
            var lastSeats = lines.Select(s => s.ToCharArray()).ToArray();
            while (true)
            {
                var newSeats = new List<char[]>();
                for (int i = 0; i < lastSeats.Length; i++)
                {
                    var row = lastSeats[i];
                    var newRow = new List<char>();
                    for (int j = 0; j < row.Length; j++)
                    {
                        if (row[j] == 'L' && surrounding(lastSeats, i, j) == 0) newRow.Add('#');
                        else if (row[j] == '#' && surrounding(lastSeats, i, j) >= seatsNeeded) newRow.Add('L');
                        else newRow.Add(row[j]);
                    }
                    newSeats.Add(newRow.ToArray());
                }
                if (lastSeats.Select(s => string.Join("", s)).SequenceEqual(newSeats.Select(s => string.Join("", s)))) break;
                lastSeats = newSeats.ToArray();
            }
            return string.Join("", lastSeats.Select(s => string.Join("", s))).Count(f => f == '#');
        }

        private static int GetSurrounding(char[][] seats, int i, int j)
        {
            var result = 0;
            char[] topRow = null;
            if (i > 0) topRow = seats[i - 1];
            char[] middleRow = seats[i];
            char[] bottomRow = null;
            if (i + 1 < seats.Length) bottomRow = seats[i + 1];

            if (topRow != null && j > 0 && topRow[j - 1] == '#') result++;
            if (topRow != null && topRow[j] == '#') result++;
            if (topRow != null && j + 1 < topRow.Length && topRow[j + 1] == '#') result++;
            if (j > 0 && middleRow[j - 1] == '#') result++;
            if (j + 1 < middleRow.Length && middleRow[j + 1] == '#') result++;
            if (bottomRow != null && j > 0 && bottomRow[j - 1] == '#') result++;
            if (bottomRow != null && bottomRow[j] == '#') result++;
            if (bottomRow != null && j + 1 < bottomRow.Length && bottomRow[j + 1] == '#') result++;

            return result;
        }
        private static bool IsInDirection(char[][] seats, int i, int j, int direction)
        {
            while (true)
            {
                if (direction < 3) i--;
                if (direction >= 2 && direction < 5) j++;
                if (direction >= 4 && direction < 7) i++;
                if (direction >= 6 || direction == 0) j--;

                if (i < 0 || j < 0 || i == seats.Length || j == seats[0].Length) return false;

                if (seats[i][j] == '#') return true;
                if (seats[i][j] == 'L') return false;
            }
        }
        private static int GetSeen(char[][] seats, int i, int j)
        {
            var result = 0;
            char[] topRow = null;
            if (i > 0) topRow = seats[i - 1];
            char[] middleRow = seats[i];
            char[] bottomRow = null;
            if (i + 1 < seats.Length) bottomRow = seats[i + 1];

            if (topRow != null && j > 0 && IsInDirection(seats, i, j, 0)) result++;
            if (topRow != null && IsInDirection(seats, i, j, 1)) result++;
            if (topRow != null && j + 1 < topRow.Length && IsInDirection(seats, i, j, 2)) result++;
            if (j > 0 && IsInDirection(seats, i, j, 7)) result++;
            if (j + 1 < middleRow.Length && IsInDirection(seats, i, j, 3)) result++;
            if (bottomRow != null && j > 0 && IsInDirection(seats, i, j, 6)) result++;
            if (bottomRow != null && IsInDirection(seats, i, j, 5)) result++;
            if (bottomRow != null && j + 1 < bottomRow.Length && IsInDirection(seats, i, j, 4)) result++;

            return result;
        }
    }
}
