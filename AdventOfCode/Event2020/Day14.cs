using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Event2020
{
    public class Day14 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            Print("Resulting number", Do(lines, false), true);
        }

        protected override void Part2(string[] lines)
        {
            Print("Resulting number", Do(lines, true), true);
        }

        private static long Override(long num, string mask)
        {
            var binaryNum = Convert.ToString(num, 2);
            if (binaryNum.Length < mask.Length) binaryNum = binaryNum.PadLeft(mask.Length, '0');
            else if (mask.Length < binaryNum.Length) mask = mask.PadLeft(binaryNum.Length, 'X');
            return Convert.ToInt64(string.Join("", binaryNum.Select((c, i) => mask[i] == 'X' ? c : mask[i])), 2);
        }
        private static string DecodeAddress(long num, string mask)
        {
            var binaryNum = Convert.ToString(num, 2);
            if (binaryNum.Length < mask.Length) binaryNum = binaryNum.PadLeft(mask.Length, '0');
            else if (mask.Length < binaryNum.Length) mask = mask.PadLeft(binaryNum.Length, 'X');
            return string.Join("", binaryNum.Select((c, i) => mask[i] == '0' ? c : mask[i]));
        }

        private static long Do(string[] lines, bool decodeAddress)
        {
            var memMatch = new Regex("^mem\\[(\\d+)\\] = (\\d+)");
            var currentMask = "";
            var memory = new Dictionary<long, long>();
            foreach (var line in lines)
            {
                if (line.StartsWith("mask "))
                    currentMask = line[7..];
                else
                {
                    var match = memMatch.Match(line);
                    var index = long.Parse(match.Groups[1].Value);
                    var value = long.Parse(match.Groups[2].Value);
                    if (decodeAddress)
                    {
                        var address = DecodeAddress(index, currentMask);
                        var xCount = address.Count(f => f == 'X');
                        var max = Convert.ToInt32(new string('1', xCount), 2);
                        for (var i = 0; i <= max; i++)
                        {
                            var j = 0;
                            var binaryI = Convert.ToString(i, 2).PadLeft(xCount, '0');
                            index = Convert.ToInt64(string.Join("", address.Select(c => c == 'X' ? binaryI[j++] : c)), 2);
                            if (memory.ContainsKey(index)) memory[index] = value;
                            else memory.Add(index, value);
                        }
                    }
                    else
                    {
                        value = Override(long.Parse(match.Groups[2].Value), currentMask);
                        if (memory.ContainsKey(index)) memory[index] = value;
                        else memory.Add(index, value);
                    }
                }
            }
            return memory.Select(kv => kv.Value).Sum();
        }
    }
}
