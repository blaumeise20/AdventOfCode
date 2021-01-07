using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day24 : Day<string[]>
    {
        protected override bool Part2Done => true;

        private HexagonField _dict;

        protected override string[] MapLines(string line)
        {
            var result = new List<string>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 'e') result.Add("e");
                if (line[i] == 'w') result.Add("w");
                if (line[i] == 's') result.Add("s" + line[++i]);
                if (line[i] == 'n') result.Add("n" + line[++i]);
            }
            return result.ToArray();
        }

        protected override void Part1(string[][] fields)
        {
            _dict = new HexagonField();
            foreach (var field in fields)
            {
                foreach (var chars in field)
                    _dict.Move(chars);
                _dict.Flip();
                _dict.ResetPosition();
            }
            Print("Resulting count", _dict.Count(), true);
        }
        protected override void Part2(string[][] lines)
        {
            for (int i = 0; i < 100; i++)
            {
                var visited = new Dictionary<(int x, int y), bool>();
                var neighboutsToVisit = _dict.Select(f => (f.x, f.y)).ToList();
                while (neighboutsToVisit.Count > 0)
                {
                    var (x, y) = neighboutsToVisit.Shift();
                    if (visited.ContainsKey((x, y))) continue;
                    var neighbours = _dict.GetNeighbours(x, y);
                    var filledCount = neighbours.Count(n => n.filled);
                    var current = _dict.Get(x, y);
                    if (!current && filledCount == 0) continue;
                    visited.Add((x, y), current && filledCount is 0 or > 2 || !current && filledCount == 2);
                    foreach (var n in neighbours) if (!neighboutsToVisit.Contains((n.x, n.y)) && !visited.ContainsKey((n.x, n.y))) neighboutsToVisit.Add((n.x, n.y));
                }
                foreach (var ((x, y), flip) in visited) if (flip) _dict.Flip(x, y);
            }
            Print("Resulting count", _dict.Count(), true);
        }

        public class HexagonField : IEnumerable<(int x, int y, bool filled)>
        {
            private readonly Dictionary<int, Dictionary<int, bool>> _field = new();
            private int _x;
            private int _y;
            public void Move(string direction) => (_x, _y) = direction switch
            {
                "e" => (_x + 1, _y),
                "w" => (_x - 1, _y),
                "se" => (_x + 1, _y - 1),
                "sw" => (_x, _y - 1),
                "ne" => (_x, _y + 1),
                "nw" => (_x - 1, _y + 1),
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
            public int Count() => _field.SelectMany(f => f.Value.Select(x => x.Value)).Count(x => x);
            public void ResetPosition() => (_x, _y) = (0, 0);
            public void Flip() => Flip(_x, _y);
            public void Flip(int x, int y)
            {
                if (_field.TryGetValue(y, out var line))
                {
                    if (line.TryGetValue(x, out var value)) line[x] = !value;
                    else line.Add(x, true);
                }
                else _field.Add(y, new() { [x] = true });
            }
            public bool Get(int x, int y) => _field.TryGetValue(y, out var line) && line.TryGetValue(x, out var value) && value;
            public (int x, int y, bool filled)[] GetNeighbours(int x, int y) => new[]
                {
                    (x + 1, y, Get(x + 1, y)),
                    (x - 1, y, Get(x - 1, y)),
                    (x + 1, y - 1, Get(x + 1, y - 1)),
                    (x, y - 1, Get(x, y - 1)),
                    (x, y + 1, Get(x, y + 1)),
                    (x - 1, y + 1, Get(x - 1, y + 1)),
                };

            public IEnumerator<(int x, int y, bool filled)> GetEnumerator() => _field.SelectMany(kv => kv.Value.Select(kv2 => (kv2.Key, kv.Key, kv2.Value))).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
