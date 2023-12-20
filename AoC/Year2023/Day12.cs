using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day12 : ISolvable
    {
        private Dictionary<string, long> arrangementsCache = new Dictionary<string, long>();

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var arrangementsList = new List<long>();
            foreach (var line in lines)
            {
                arrangementsCache = new Dictionary<string, long>();
                var arr = line.Split(" ");
                var pattern = arr[0];
                var blocks = arr[1].Split(",").Select(int.Parse).ToList();

                pattern = string.Join("?", Enumerable.Repeat(pattern, 5));
                blocks = Enumerable.Repeat(blocks, 5).SelectMany(l => l).ToList();

                var arrangements = CalculateArrangements(pattern, blocks);
                arrangementsList.Add(arrangements);
            }


            Console.WriteLine($"{arrangementsList.Sum()}");
        }

        private long CalculateArrangements(string pattern, List<int> blocks)
        {
            if (arrangementsCache.TryGetValue(pattern + blocks.Count(), out var cached))
                return cached;

            if (pattern == "" && blocks.Count() == 0)
                return 1;
            if (pattern.Length < blocks.Sum() + blocks.Count() - 1)
                return 0;

            if (pattern[0] == '#')
            {
                if (blocks.Count() < 1 || blocks[0] > pattern.Length)
                    return 0;

                var damagedPart = pattern[0..blocks[0]];
                if (damagedPart.Any(c => c != '#' && c != '?'))
                    return 0;

                var newPattern = pattern[blocks[0]..];
                if (newPattern == "")
                    return CalculateArrangements(newPattern, blocks.Skip(1).ToList());

                if (newPattern[0] == '#')
                    return 0;

                return CalculateArrangements(newPattern[1..], blocks.Skip(1).ToList());
            }
            else if (pattern[0] == '.')
            {
                return CalculateArrangements(pattern[1..], blocks);
            }
            else if (pattern[0] == '?')
            {
                var a = CalculateArrangements(pattern[1..], blocks);
                var b = CalculateArrangements("#" + pattern[1..], blocks);
                arrangementsCache.Add(pattern + blocks.Count(), a + b);
                return a + b;
            }
            else
                throw new InvalidOperationException($"{pattern[0]} is unidentifiable!");

        }
    }
}
