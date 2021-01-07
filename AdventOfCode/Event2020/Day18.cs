using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Event2020
{
    public class Day18 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            Print("Resulting number", lines.Select(l => Evaluate(l).value).Sum(), true);
        }
        protected override void Part2(string[] lines)
        {
            Print("Resulting number", lines.Select(l => Evaluate(l, true).value).Sum(), true);
        }

        private static (string leftover, long value) Evaluate(string text, bool part2 = false)
        {
            var leftover = text;
            var lastOperator = true;
            var operatorsAndValues = new List<(long number, bool addition)>() { (0, false) };
            while (leftover.Length > 0)
            {
                switch (leftover[0])
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        var numString = Regex.Match(leftover, "^(\\d+)").Groups[1].Value;
                        var num = long.Parse(numString);
                        leftover = leftover[numString.Length..];
                        operatorsAndValues.Add((num, lastOperator));
                        break;
                    case ' ':
                        lastOperator = leftover[1] == '+';
                        leftover = leftover[3..];
                        break;
                    case '(':
                        var (evLeft, evResult) = Evaluate(leftover[1..], part2);
                        leftover = evLeft;
                        operatorsAndValues.Add((evResult, lastOperator));
                        break;
                    case ')':
                        return (leftover[1..], Calculate());
                }
            }
            long Calculate()
            {
                if (part2)
                {
                    for (int i = 0; i < operatorsAndValues.Count; i++)
                    {
                        var (number, addition) = operatorsAndValues[i];
                        if (addition)
                        {
                            operatorsAndValues.RemoveAt(i);
                            operatorsAndValues[--i] = (operatorsAndValues[i].number + number, operatorsAndValues[i].addition);
                        }
                    }
                }
                return operatorsAndValues.Aggregate((a, b) => (b.addition ? a.number + b.number : a.number * b.number, a.addition)).number;
            }
            return (leftover, Calculate());
        }
    }
}
