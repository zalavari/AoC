using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    internal class Day03 : ISolvable
    {

        public void Solve(string path)
        {
            var lines = File.ReadAllLines(path);
            var resultA = lines
                .Select(line => line.Select(conv))
                .Select(sack => sack.Take(sack.Count() / 2).ToHashSet()
                      .Intersect(sack.Skip(sack.Count() / 2).ToHashSet())
                      .FirstOrDefault())
                .Sum();

            var sacks = lines
                .Select(line => line.Select(conv))
                .Select(sack => sack.ToHashSet())
                .ToList();

            var sum = 0;
            for (int i = 0; i < sacks.Count; i += 3)
            {
                sum += sacks[i].Intersect(sacks[i + 1]).Intersect(sacks[i + 2]).First();
            }

            Console.WriteLine(path);
            Console.WriteLine($"ResultA: {resultA}");
            Console.WriteLine($"ResultB: {sum}");

            Console.WriteLine();
        }

        int conv(char c)
        {
            var r = char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27;
            return r;
        }
    }
}
