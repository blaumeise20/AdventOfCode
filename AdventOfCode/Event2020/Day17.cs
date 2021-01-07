using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day17 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines)
        {
            Print("Resulting count", Solve3D(lines), true);
        }
        protected override void Part2(string[] lines)
        {
           Print("Resulting count", Solve4DSecond(lines), true);
        }

        private static int Solve3D(string[] lines)
        {
            var currentMatrix = new Dictionary<int, char[][]>();
            var startMatrix = lines.Prepend(new string('.', lines[0].Length)).Append(new string('.', lines[0].Length)).Select(s => $".{s}.".ToCharArray()).ToArray();
            currentMatrix.Add(-1, Enumerable.Range(0, startMatrix.Length).Select(x => new string('.', startMatrix[0].Length).ToCharArray()).ToArray());
            currentMatrix.Add(0, startMatrix);
            currentMatrix.Add(1, Enumerable.Range(0, startMatrix.Length).Select(x => new string('.', startMatrix[0].Length).ToCharArray()).ToArray());
            for (var count = 0; count < 6; count++)
            {
                var newMatrix = new Dictionary<int, char[][]>();
                var startIndex = currentMatrix.First().Key;
                var endIndex = currentMatrix.Last().Key;
                newMatrix.Add(startIndex - 1, Enumerable.Range(0, currentMatrix[0].Length + 2).Select(x => new string('.', currentMatrix[0][0].Length + 2).ToCharArray()).ToArray());
                for (int x = startIndex; x <= endIndex; x++)
                {
                    var currentLayer = currentMatrix[x];
                    var newLayer = new List<char[]>
                    {
                        new string('.', currentLayer[0].Length + 2).ToCharArray()
                    };
                    for (int i = 0; i < currentLayer.Length; i++)
                    {
                        var row = currentLayer[i];
                        var newRow = new List<char>
                        {
                            '.'
                        };
                        for (int j = 0; j < row.Length; j++)
                        {
                            var surroundingCount = GetSurrounding(currentMatrix, i, j, x);
                            if (row[j] == '#' && (surroundingCount is 2 or 3)) newRow.Add('#');
                            else if (row[j] == '.' && (surroundingCount == 3)) newRow.Add('#');
                            else newRow.Add('.');
                        }
                        newRow.Add('.');
                        newLayer.Add(newRow.ToArray());
                    }
                    newLayer.Add(new string('.', currentLayer[0].Length + 2).ToCharArray());
                    newMatrix.Add(x, newLayer.ToArray());
                }
                newMatrix.Add(endIndex + 1, Enumerable.Range(0, currentMatrix[0].Length + 2).Select(x => new string('.', currentMatrix[0][0].Length + 2).ToCharArray()).ToArray());
                currentMatrix = newMatrix;
            }
            return string.Join("", currentMatrix.Select(s => string.Join("", s.Value.Select(a => string.Join("", a))))).Count(f => f == '#');
        }
        private static int GetSurrounding(Dictionary<int, char[][]> layers, int i, int j, int z)
        {
            var count1 = z > layers.First().Key ? GetSurroundingInLayer(layers, i, j, z - 1, true) : 0;
            var count2 = GetSurroundingInLayer(layers, i, j, z, false);
            var count3 = z + 1 < layers.Last().Key ? GetSurroundingInLayer(layers, i, j, z + 1, true) : 0;
            return count1 + count2 + count3;
        }
        private static int GetSurroundingInLayer(Dictionary<int, char[][]> layers, int i, int j, int z, bool includeMiddle)
        {
            var layer = layers[z];
            var result = 0;
            char[] topRow = null;
            if (i > 0) topRow = layer[i - 1];
            char[] middleRow;
            try
            {
                middleRow = layer[i];
            }
            catch (Exception e)
            {
                Debugger.Break();
                Console.WriteLine(e);
                throw new InvalidOperationException();
            }
            char[] bottomRow = null;
            if (i + 1 < layer.Length) bottomRow = layer[i + 1];
            if (topRow != null && j > 0 && topRow[j - 1] == '#') result++;
            if (topRow != null && topRow[j] == '#') result++;
            if (topRow != null && j + 1 < topRow.Length && topRow[j + 1] == '#') result++;
            if (j > 0 && middleRow[j - 1] == '#') result++;
            try
            {
                if (includeMiddle && middleRow[j] == '#') result++;
            }
            catch(Exception e)
            {
                Debugger.Break();
                Console.WriteLine(e);
                throw new InvalidOperationException();
            }
            if (j + 1 < middleRow.Length && middleRow[j + 1] == '#') result++;
            if (bottomRow != null && j > 0 && bottomRow[j - 1] == '#') result++;
            if (bottomRow != null && bottomRow[j] == '#') result++;
            if (bottomRow != null && j + 1 < bottomRow.Length && bottomRow[j + 1] == '#') result++;

            return result;
        }


        private static readonly (int x, int y, int z, int w)[] checkDists = Enumerable.Range(-1, 1).SelectMany(x =>
                                                                                Enumerable.Range(-1, 1).SelectMany(y =>
                                                                                    Enumerable.Range(-1, 1).SelectMany(z =>
                                                                                        Enumerable.Range(-1, 1).Select(w => (x, y, z, w)))))
                                                                            .Where(d => d != (0, 0, 0, 0)).ToArray();
        private static int GetSurrounding(List<List<List<List<char>>>> state, int i, int j, int z, int w) => checkDists.Count(d =>
        {
            var newW = w + d.w;
            if (newW < 0 || newW > state.Count) return false;
            var matrix = state[newW];
            var newZ = z + d.z;
            if (newZ < 0 || newZ > matrix.Count) return false;
            var layer = matrix[newZ];
            var newY = j + d.y;
            if (newY < 0 || newY > layer.Count) return false;
            var row = layer[newY];
            var newX = i + d.x;
            if (newX < 0 || newX > row.Count) return false;
            return row[newX] == '#';
        });
        private static int Solve4D(string[] lines)
        {
            var state = new List<List<List<List<char>>>>();
            var startMatrix = lines.Prepend(new string('.', lines[0].Length)).Append(new string('.', lines[0].Length)).Select(s => $".{s}.".ToCharArray().ToList()).ToList();
            state.Add(new() { Enumerable.Range(0, startMatrix.Count).Select(x => new string('.', startMatrix[0].Count).ToCharArray().ToList()).ToList() });
            state.Add(new() { startMatrix });
            state.Add(new() { Enumerable.Range(0, startMatrix.Count).Select(x => new string('.', startMatrix[0].Count).ToCharArray().ToList()).ToList() });
            for (var count = 0; count < 6; count++)
            {
                var newState = new List<List<List<List<char>>>>();

                for (int w = 0; w < state.Count; w++)
                {
                    var currentMatrix = state[w];
                    var newMatrix = new List<List<List<char>>>
                    {
                        Enumerable.Range(0, currentMatrix[0].Count + 2).Select(x => new string('.', currentMatrix[0][0].Count + 2).ToList()).ToList()
                    };
                    for (int x = 0; x < currentMatrix.Count; x++)
                    {
                        var currentLayer = currentMatrix[x];
                        var newLayer = new List<List<char>>
                        {
                            new string('.', currentLayer[0].Count + 2).ToCharArray().ToList()
                        };
                        for (int i = 0; i < currentLayer.Count; i++)
                        {
                            var row = currentLayer[i];
                            var newRow = new List<char>
                            {
                                '.'
                            };
                            for (int j = 0; j < row.Count; j++)
                            {
                                var surroundingCount = GetSurrounding(state, i, j, x, w);
                                if (row[j] == '#' && (surroundingCount is 2 or 3)) newRow.Add('#');
                                else if (row[j] == '.' && (surroundingCount == 3)) newRow.Add('#');
                                else newRow.Add('.');
                            }
                            newRow.Add('.');
                            newLayer.Add(newRow);
                        }
                        newLayer.Add(new string('.', currentLayer[0].Count + 2).ToCharArray().ToList());
                        newMatrix.Add(newLayer);
                    }
                    newMatrix.Add(Enumerable.Range(0, currentMatrix[0].Count + 2).Select(x => new string('.', currentMatrix[0][0].Count + 2).ToCharArray().ToList()).ToList());
                    currentMatrix = newMatrix;
                }
            }
            return state.SelectMany(x => x).SelectMany(x => x).SelectMany(x => x).Count(x => x == '#');
        }

        private static int Solve4DSecond(string[] lines)
        {
            var input = string.Join('\n', lines);
            var state = new Dictionary<(int x, int y, int z, int w), bool>(8192);
            int _x = 0, _y = 0;
            foreach (var c in input)
            {
                if (c == '\n') { _x = 0; _y++; }
                else state[(_x++, _y, 0, 0)] = c == '#';
            }

            var count = new Dictionary<(int x, int y, int z, int w), int>(8192);
            var dirs = Enumerable.Range(-1, 3)
                .SelectMany(x => Enumerable.Range(-1, 3)
                    .SelectMany(y => Enumerable.Range(-1, 3)
                        .SelectMany(z => Enumerable.Range(-1, 3)
                            .Select(w => (x, y, z, w)))))
                .Where(d => d != (0, 0, 0, 0))
                .ToArray();
            for (int i = 0; i < 6; i++)
            {
                count.Clear();

                // so count has everything, and we can rely on that in final foreach
                foreach (var p in state.Keys)
                    count[p] = 0;

                foreach (var ((x, y, z, w), alive) in state.Where(kvp => kvp.Value))
                    foreach (var (dx, dy, dz, dw) in dirs)
                        count[(x + dx, y + dy, z + dz, w + dw)] =
                            count.GetValueOrDefault((x + dx, y + dy, z + dz, w + dw)) + 1;

                foreach (var (p, c) in count)
                    state[p] = (state.GetValueOrDefault(p), c) switch
                    {
                        (true, >= 2 and <= 3) => true,
                        (false, 3) => true,
                        _ => false,
                    };
            }

            return state.Count(x => x.Value);
        }

        private static int SolveNew(string[] lines)
        {



            return 0;

            var state = new Dictionary<(int x, int y, int z), bool>();

            var surroundings = Enumerable.Range(-1, 1).SelectMany(x =>
                                   Enumerable.Range(-1, 1).SelectMany(y =>
                                       Enumerable.Range(-1, 1).Select(z => (x, y, z))))
                               .Where(d => d != (0, 0, 0)).ToArray();

            for (var count = 0; count < 6; count++)
            {
            }

            return state.Count(f => f.Value);
        }
    }
}
