using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Event2020
{
    public class Day21 : Day<(string[] ingredients, string[] allergens)>
    {
        private Dictionary<string, string> _foundAllergens;

        protected override bool Part2Done => true;

        protected override (string[] ingredients, string[] allergens) MapLines(string line)
        {
            var splitted = line.Split(" (contains ");
            return (splitted[0].Split(" "), splitted[1][..^1].Split(", "));
        }

        protected override void Part1((string[] ingredients, string[] allergens)[] lines)
        {
            var allergensToIngredients = new Dictionary<string, List<string>>();
            var counter = new Dictionary<string, int>();
            var allIngredients = new List<string>();
            foreach (var (ingredients, allergens) in lines)
            {
                foreach (var allergen in allergens)
                {
                    if (allergensToIngredients.TryGetValue(allergen, out var value))
                        allergensToIngredients[allergen] = value.Intersect(ingredients).ToList();
                    else
                        allergensToIngredients.Add(allergen, ingredients.ToList());
                }
                foreach (var ingredient in ingredients)
                {
                    if (counter.TryGetValue(ingredient, out var value))
                        counter[ingredient] = value + 1;
                    else
                        counter.Add(ingredient, 1);
                    if (!allIngredients.Contains(ingredient)) allIngredients.Add(ingredient);
                }
            }
            var foundAllergens = new Dictionary<string, string>();

            var count = allergensToIngredients.Count;
            while (foundAllergens.Count < count)
            {
                var found = allergensToIngredients.First(ai => ai.Value.Count == 1);
                var ingredient = found.Value[0];
                foundAllergens.Add(found.Key, ingredient);
                allergensToIngredients.Remove(found.Key);
                foreach (var ai in allergensToIngredients) ai.Value.Remove(ingredient);
            }

            _foundAllergens = foundAllergens;

            var ok = allIngredients.Where(i => !foundAllergens.ContainsValue(i));
            Print("Resulting count", ok.Select(x => counter[x]).Sum(), true);
            //Debugger.Break();
        }

        protected override void Part2((string[] ingredients, string[] allergens)[] lines)
        {
            Print("String to enter", string.Join(",", _foundAllergens.OrderBy(x => x.Key).Select(x => x.Value)), true);
        }
    }
}
