using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day22 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines) => Run(lines, false);
        protected override void Part2(string[] lines) => Run(lines, true);

        private void Run(string[] lines, bool part2)
        {
            var players = string.Join("\n", lines).Split("\n\n");
            var (winner, player1, player2) = Play(players[0].Split("\n")[1..].Select(x => int.Parse(x)).ToList(), players[1].Split("\n")[1..].Select(x => int.Parse(x)).ToList(), part2);
            Print("Resulting number", (winner == 1 ? player1 : player2).Reverse<int>().Select((n, i) => n * (i + 1)).Sum(), true);
        }
        private (int winner, List<int> player1, List<int> player2) Play(List<int> player1, List<int> player2, bool part2 = false)
        {
            var lastSets = new HashSet<string>();
            var totalCards = player1.Count + player2.Count;
            while (player1.Count < totalCards && player2.Count < totalCards)
            {
                if (part2 && !lastSets.Add($"{string.Join(",", player1)} {string.Join(",", player2)}")) return (1, player1, player2);
                var (player1Card, player2Card) = (player1.Shift(), player2.Shift());
                if (part2 && player1.Count >= player1Card && player2.Count >= player2Card ? Play(player1.Take(player1Card).ToList(), player2.Take(player2Card).ToList(), true).winner == 1 : player1Card > player2Card)
                    player1.AddRange(new[] { player1Card, player2Card });
                else player2.AddRange(new[] { player2Card, player1Card });
            }
            return (player1.Count == 0 ? 2 : 1, player1, player2);
        }
    }
}
