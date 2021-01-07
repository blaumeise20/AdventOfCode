using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day23 : Day
    {
        protected override bool Part2Done => true;
        protected override int RunPart2 => 5;

        protected override void Part1(string[] lines)
        {
            var cups = lines[0].ToCharArray().Select(n => int.Parse(n.ToString())).ToList();
            for (int count = 0; count < 100; count++)
            {
                var currentCup = cups.Shift();
                var pickedCups = cups.Shift(3);
                var destCup = currentCup - 1;
                while (!cups.Contains(destCup))
                {
                    destCup--;
                    if (destCup < 1)
                    {
                        destCup = cups.Max();
                        break;
                    }
                }
                var index = cups.IndexOf(destCup);
                cups.InsertRange(index + 1, pickedCups);
                cups.Add(currentCup);
            }
            while (cups[0] != 1) cups.Add(cups.Shift());
            Print("Resulting number", string.Join("", cups.Skip(1)), true);
        }

        //protected override void Part1(string[] lines)
        //{
        //    var count = 9;
        //    var nums = new int[count].Select((_, i) => i + 1).ToArray();
        //    var originalCups = lines[0].ToCharArray().Select(n => int.Parse(n.ToString())).ToArray();
        //    for (var i = 0; i < originalCups.Length; i++) nums[i] = originalCups[i];
        //    var obj = new Dictionary<int, int>();
        //    for (var i = 0; i < nums.Length; i++) obj[nums[i]] = i + 1 < nums.Length ? nums[i + 1] : nums[0];

        //    var currentNum = originalCups[0];
        //    for (var i = 0; i < 100; i++)
        //    {
        //        var num1 = obj[currentNum];
        //        var num2 = obj[num1];
        //        var num3 = obj[num2];
        //        var nexts = new[] { num1, num2, num3 };
        //        var destNum = currentNum - 1;
        //        if (destNum == 0) destNum = nums.Length;
        //        while (nexts.Contains(destNum))
        //        {
        //            destNum--;
        //            if (destNum == 0) destNum = nums.Length;
        //        }
        //        obj[currentNum] = obj[num3];
        //        currentNum = obj[num3];
        //        obj[num3] = obj[destNum];
        //        obj[destNum] = num1;
        //        if (i == 9) Debugger.Break();
        //    }
        //    Debugger.Break();
        //    Print("Resulting number", obj[1] * obj[obj[1]], true);
        //}
        protected override void Part2(string[] lines)
        {
            /*var count = 1000000;
            var nums = new int[count].Select((_, i) => i + 1L).ToArray();
            var originalCups = lines[0].ToCharArray().Select(n => long.Parse(n.ToString())).ToArray();
            for (var i = 0; i < originalCups.Length; i++) nums[i] = originalCups[i];
            var obj = new Dictionary<long, long>();
            for (var i = 0; i < nums.Length; i++) obj[nums[i]] = i + 1 < nums.Length ? nums[i + 1] : nums[0];

            var currentNum = originalCups[0];
            for (var i = 0; i < 10000000; i++)
            {
                var num1 = obj[currentNum];
                var num2 = obj[num1];
                var num3 = obj[num2];
                var nexts = new[] { num1, num2, num3 };
                var destNum = currentNum - 1;
                if (destNum == 0) destNum = nums.Length;
                while (nexts.Contains(destNum))
                {
                    destNum--;
                    if (destNum == 0) destNum = nums.Length;
                }
                obj[currentNum] = obj[num3];
                currentNum = obj[num3];
                obj[num3] = obj[destNum];
                obj[destNum] = num1;
            }*/
            var obj = Run(lines, 1000000, 10000000);
            Print("Resulting number", obj[1] * obj[obj[1]], true);
        }
        private static Dictionary<long, long> Run(string[] lines, int extendTo, int repeation)
        {
            var originalCups = lines[0].ToCharArray().Select(n => long.Parse(n.ToString()));
            var max = originalCups.Max();
            var joiner = Enumerable.Range((int)max + 1, (int)(extendTo - max)).Select(n => Convert.ToInt64(n));
            var nums = originalCups.Concat(joiner).ToArray();
            var obj = new Dictionary<long, long>(nums.Select((n, i) => KeyValuePair.Create(n, i + 1 < nums.Length ? nums[i + 1] : nums[0])));

            var currentNum = originalCups.First();
            for (var i = 0; i < repeation; i++)
            {
                var num1 = obj[currentNum];
                var num2 = obj[num1];
                var num3 = obj[num2];
                var destNum = currentNum - 1 == 0 ? nums.Length : currentNum - 1;
                while (destNum == num1 || destNum == num2 || destNum == num3) if (--destNum == 0) destNum = nums.Length;
                currentNum = obj[currentNum] = obj[num3];
                obj[num3] = obj[destNum];
                obj[destNum] = num1;
            }
            return obj;
        }
    }
}
