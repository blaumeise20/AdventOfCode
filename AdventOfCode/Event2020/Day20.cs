using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Event2020
{
    public class Day20 : Day
    {
        protected override bool Part2Done => true;
        private Tile[] _tiles;
        private Tile[] _cornerTiles;
        private int _size;
        private Tile[,] _grid;

        protected override void Part1(string[] lines)
        {
            _tiles = string.Join("\n", lines).Split("\n\n").Select(p => p.Split('\n')).Select(p => new Tile(p[1..], long.Parse(p[0][5..^1]))).ToArray();
            _cornerTiles = new Tile[4];
            var i = 0;
            foreach (var tile in _tiles)
            {
                if (tile.IsCornerTile(_tiles)) _cornerTiles[i++] = tile;
                if (i == 4) break;
            }
            Print("Resulting number", _cornerTiles.Aggregate(1L, (a, b) => a * b.Id), true);
        }

        protected override void Part2(string[] lines)
        {
            _size = (int)Math.Sqrt(_tiles.Length);
            _grid = new Tile[_size, _size];
            Move(_grid);
            Orientate(_grid);
            Print("Resulting count", SearchAndReplace(_grid), true);
        }

        private void Move(Tile[,] grid)
        {
            grid[0, 0] = _cornerTiles[0];
            var otherTiles = grid[0, 0].GetConnectionTiles(_tiles);
            grid[0, 1] = otherTiles[0];
            grid[1, 0] = otherTiles[1];
            var ids = new List<long>() { _cornerTiles[0].Id, otherTiles[0].Id, otherTiles[1].Id };
            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    if (grid[row, col] != null) continue;
                    var neighbours = (row == 0 ? grid[row, col - 1] : grid[row - 1, col]).GetConnectionTiles(_tiles).Where(t => !ids.Contains(t.Id));
                    if (neighbours.Count() > 1)
                    {
                        var neighbour = grid[row + 1, col - 2].GetConnectionTiles(_tiles).Intersect(neighbours).First();
                        grid[row + 1, col - 1] = neighbour;
                        var otherNeighbour = neighbours.First(n => n.Id != neighbour.Id);
                        grid[row, col] = otherNeighbour;
                        ids.AddRange(new[] { neighbour.Id, otherNeighbour.Id });
                    }
                    else
                    {
                        var firstNeighbour = neighbours.First();
                        grid[row, col] = firstNeighbour;
                        ids.Add(firstNeighbour.Id);
                    }
                }
            }
        }
        private void Orientate(Tile[,] grid)
        {
            var sides = grid[0, 0].GetConnectionSides(new[] { grid[0, 1], grid[1, 0] });
            grid[0, 0].Rotate(sides switch
            {
                (false, true, true, false) => 0,
                (!false, !false, !true, !true) => 1,
                (!false, !true, !true, !false) => 2,
                (!true, !true, !false, !false) => 3
            });
            if (!grid[0, 1].CanConnect(grid[0, 0].RightEdge)) grid[0, 0].FlipDiagonal();

            for (int row = 0; row < _size; row++)
            {
                for (int col = 0; col < _size; col++)
                {
                    if (col == 0)
                    {
                        if (row == 0) continue;
                        while (!grid[row - 1, col].CanConnect(grid[row, col].TopEdge)) grid[row, col].Rotate(1);
                        if (grid[row - 1, col].BottomEdge != grid[row, col].TopEdge) grid[row, col].FlipVertical();
                    }
                    else
                    {
                        while (!grid[row, col - 1].CanConnect(grid[row, col].LeftEdge)) grid[row, col].Rotate(1);
                        if (grid[row, col - 1].RightEdge != grid[row, col].LeftEdge) grid[row, col].FlipHorizontal();
                    }
                }
            }
        }
        private int SearchAndReplace(Tile[,] grid)
        {
            var resultRows = new TileRow[_size];
            for (int i = 0; i < _size; i++)
            {
                resultRows[i] = new TileRow();
                for (int j = 0; j < _size; j++) resultRows[i].Add(grid[i, j]);
            }
            var completeField = resultRows.SelectMany(r => r.GetText()).ToArray();
            var monsterCount = 0;
            for (int i = 0; i < 8; i++)
            {
                monsterCount = CountMonsters(completeField);
                if (monsterCount > 0) return completeField.Select(x => x.CountValue('#')).Sum() - monsterCount * 15;
                completeField = Tile.Rotate(completeField);
                if (i == 3) completeField = Tile.FlipHorizontal(completeField);
            }
            throw new InvalidOperationException("Should never happen :-(");
        }
        private static int CountMonsters(string[] field)
        {
            var count = 0;
            var regex1 = new Regex("^..................#.$");
            var regex2 = new Regex("^#....##....##....###$");
            var regex3 = new Regex("^.#..#..#..#..#..#...$");
            for (int i = 0; i < field.Length - 2; i++)
            {
                for (int j = 0; j < field[i].Length - 20; j++)
                {
                    if (regex1.IsMatch(field[i][j..(j + 20)]) &&
                        regex2.IsMatch(field[i + 1][j..(j + 20)]) &&
                        regex3.IsMatch(field[i + 2][j..(j + 20)])) count++;
                }
            }
            return count;
        }

        public class TileRow
        {
            public List<string[]> Tiles { get; } = new();
            public void Add(Tile tile) => Tiles.Add(tile.FieldWithoutBorders);
            public IEnumerable<string> GetText() => Tiles[0].Select((_, i) => string.Join("", Tiles.Select(t => t[i])));
        }

        public class Tile
        {
            public string[] Edges => new[] { TopEdge, RightEdge, BottomEdge, LeftEdge };
            public string TopEdge { get; private set; }
            public string RightEdge { get; private set; }
            public string BottomEdge { get; private set; }
            public string LeftEdge { get; private set; }
            public string[] CompleteField { get; private set; }
            public string[] FieldWithoutBorders => CompleteField[1..^1].Select(l => l[1..^1]).ToArray();
            public long Id { get; }

            public Tile(string[] field, long id)
            {
                Assign(field);
                Id = id;
            }

            public bool CanConnect(Tile otherTile) => otherTile.Id != Id && otherTile.Edges.Any(e => CanConnect(e));
            public bool CanConnect(string edge) => Edges.Any(e => e == edge || e == edge.Reverse());
            public bool IsCornerTile(Tile[] tiles) => tiles.Count(t => t.Id != Id && CanConnect(t)) == 2;
            public (bool top, bool right, bool bottom, bool left) GetConnectionSides(Tile[] tiles) => (tiles.Any(t => t.CanConnect(TopEdge)), tiles.Any(t => t.CanConnect(RightEdge)), tiles.Any(t => t.CanConnect(BottomEdge)), tiles.Any(t => t.CanConnect(LeftEdge)));
            public Tile[] GetConnectionTiles(Tile[] tiles) => tiles.Where(t => t.CanConnect(this)).ToArray();

            public void Rotate(int count)
            {
                var currentField = CompleteField;
                for (int i = 0; i < count; i++) currentField = Rotate(currentField);
                Assign(currentField);
            }
            public void FlipHorizontal() => Assign(FlipHorizontal(CompleteField));
            public void FlipVertical() => Assign(CompleteField.Select(l => string.Join("", l.Reverse())).ToArray());
            public void FlipDiagonal() => Assign(FlipDiagonal(CompleteField));

            private void Assign(string[] field)
            {
                CompleteField = field;
                TopEdge = CompleteField[0];
                BottomEdge = CompleteField[^1];
                LeftEdge = string.Join("", CompleteField.Select(l => l[0]));
                RightEdge = string.Join("", CompleteField.Select(l => l[^1]));
            }

            public static string[] FlipHorizontal(string[] field) => field.Reverse().ToArray();
            public static string[] FlipDiagonal(string[] field) => Enumerable.Range(0, field[0].Length).Select(i => string.Join("", field.Select(lst => lst[i]))).ToArray();
            public static string[] Rotate(string[] field) => FlipDiagonal(field).Select(l => l.Reverse()).ToArray();
        }
    }
}
