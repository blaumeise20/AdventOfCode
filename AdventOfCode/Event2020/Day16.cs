using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day16 : Day
    {
        protected override bool Part2Done => true;
        private string[][] _data;
        private List<int[]> _validTickets = new List<int[]>();
        private (string field, (int min, int max)[] values)[] _fieldValidation;

        protected override void Part1(string[] lines)
        {
            _data = string.Join("\n", lines).Split("\n\n").Select(l => l.Split("\n")).ToArray();
            _fieldValidation = _data[0].Select(f =>
            {
                var s = f.Split(": ");
                return (field: s[0], values: s[1].Split(" or ").Select(n => { var s2 = n.Split("-"); return (int.Parse(s2[0]), int.Parse(s2[1])); }).ToArray());
            }).ToArray();
            var otherTickets = _data[2][1..].Select(t => t.Split(",").Select(n => int.Parse(n)).ToArray()).ToArray();
            var count = 0;
            foreach (var ticket in otherTickets)
            {
                var isWrong = false;
                foreach (var field in ticket) if (!_fieldValidation.Any(fv => fv.values.Any(v => field >= v.min && field <= v.max))) { count += field; isWrong = true; }
                if (!isWrong) _validTickets.Add(ticket);
            }
            Print("Resulting count", count, true);
        }
        protected override void Part2(string[] lines)
        {
            var myTicket = _data[1][1].Split(",").Select(n => long.Parse(n)).ToArray();
            var newTickets = Enumerable.Range(0, _validTickets[0].Length)
                .Select(i => _validTickets.Select(lst => lst[i]).ToArray()).ToArray();

            var ticketFieldIndexes = new Dictionary<int, string[]>();
            var i = 0;
            foreach (var ticketRow in newTickets)
            {
                ticketFieldIndexes.Add(i, _fieldValidation
                                              .Where(f => ticketRow
                                                  .All(t => f.values
                                                                .Any(v => t >= v.min && t <= v.max)))
                                              .Select(f => f.field)
                                              .ToArray());
                i++;
            }

            var ticketFieldAndIndex = new Dictionary<string, int>();
            while (ticketFieldAndIndex.Count < newTickets.Length)
            {
                for (int j = 0; j < _fieldValidation.Length; j++)
                {
                    var (field, _) = _fieldValidation[j];
                    try
                    {
                        var index = ticketFieldIndexes.Single(ti => ti.Value.Contains(field)).Key;
                        ticketFieldAndIndex.Add(field, index);
                        ticketFieldIndexes.Remove(index);
                    }
                    catch { }
                }
            }

            Print("Resulting number", _fieldValidation.Where(fv => fv.field.StartsWith("departure ")).Select(fv => myTicket[ticketFieldAndIndex[fv.field]]).Aggregate((a, b) => a * b), true);
        }
    }
}
