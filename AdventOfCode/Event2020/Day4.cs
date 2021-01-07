using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Event2020
{
    public class Day4 : Day
    {
        protected override bool Part2Done => true;

        protected override void Part1(string[] lines) => Print("Resulting number", CountPassports(lines, false), true);
        protected override void Part2(string[] lines) => Print("Resulting number", CountPassports(lines, true), true);

        private static int CountPassports(string[] lines, bool checkValue)
        {
            var passports = string.Join("\n", lines).Split("\n\n").Select(p =>
            {
                return new Dictionary<string, string>(p.Split(new char[] { ' ', '\n' }).Select(p =>
                {
                    var splitted = p.Split(":");
                    return KeyValuePair.Create(splitted[0], splitted[1]);
                }));
            }).ToArray();
            var count = 0;
            foreach (var passport in passports) if ((passport.Count == 8 || (passport.Count == 7 && !passport.ContainsKey("cid"))) && (!checkValue || CheckPassportValue(passport))) count++;
            return count;
        }

        private static bool CheckPassportValue(Dictionary<string, string> passport)
        {
            var byrValue = passport["byr"];
            if (!Regex.Match(byrValue, "^\\d{4}$").Success) return false;
            var byr = int.Parse(byrValue);
            if (byr < 1920 || byr > 2002) return false;

            var iyrValue = passport["iyr"];
            if (!Regex.Match(iyrValue, "^\\d{4}$").Success) return false;
            var iyr = int.Parse(iyrValue);
            if (iyr < 2010 || iyr > 2020) return false;

            var eyrValue = passport["eyr"];
            if (!Regex.Match(eyrValue, "^\\d{4}$").Success) return false;
            var eyr = int.Parse(eyrValue);
            if (eyr < 2020 || eyr > 2030) return false;

            var hgtValue = passport["hgt"];
            if (!hgtValue.EndsWith("cm") && !hgtValue.EndsWith("in")) return false;
            var hgt = int.Parse(hgtValue.Substring(0, hgtValue.Length - 2));
            if (hgtValue.EndsWith("cm") && (hgt < 150 || hgt > 193)) return false;
            if (hgtValue.EndsWith("in") && (hgt < 59 || hgt > 76)) return false;

            if (!Regex.Match(passport["hcl"], "^#[0-9a-f]{6}$").Success) return false;

            if (!"amb blu brn gry grn hzl oth".Split(" ").Contains(passport["ecl"])) return false;

            if (!Regex.Match(passport["pid"], "^\\d{9}$").Success) return false;

            return true;
        }
    }
}
