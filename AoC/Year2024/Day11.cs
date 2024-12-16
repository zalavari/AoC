using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day11 : ISolvable
    {

        private Dictionary<(long, int), long> cache = new Dictionary<(long, int), long>();

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();
            var stones = lines[0].Split().Select(long.Parse).ToList();

            NaiveSolution(stones, 25);
            ImprovedSolution(stones, 75);

            Console.WriteLine();
        }

        private void NaiveSolution(List<long> stones, int generation)
        {
            var oldGeneration = new List<long>(stones);

            for (int i = 0; i < generation; i++)
            {
                var newGeneration = new List<long>();

                foreach (var stone in oldGeneration)
                {
                    if (stone == 0)
                    {
                        newGeneration.Add(1);
                    }
                    else if (HasEvenNumberOfDigits(stone))
                    {
                        var firstHalf = stone.ToString().Substring(0, stone.ToString().Length / 2);
                        var secondHalf = stone.ToString().Substring(stone.ToString().Length / 2);
                        var firstHalfInt = long.Parse(firstHalf);
                        var secondHalfInt = long.Parse(secondHalf);
                        newGeneration.Add(firstHalfInt);
                        newGeneration.Add(secondHalfInt);
                    }
                    else
                    {
                        newGeneration.Add(stone * 2024);
                    }
                }

                oldGeneration = newGeneration;
            }

            Console.WriteLine($"Naive solution after {generation}: {oldGeneration.Count}");
        }

        private void ImprovedSolution(List<long> stones, int generation)
        {
            var count = 0L;
            foreach (var stone in stones)
            {
                count += CountOfNumberAfterGeneration(stone, generation);
            }
            Console.WriteLine($"Improved solution after {generation}: {count}");
        }

        private bool HasEvenNumberOfDigits(long stone)
        {
            var count = 0;
            while (stone > 0)
            {
                count++;
                stone /= 10;
            }
            return count % 2 == 0;
        }

        private long CountOfNumberAfterGeneration(long stone, int generation)
        {
            if (cache.TryGetValue((stone, generation), out var cached))
                return cached;

            var result = 0L;
            if (generation == 0)
            {
                result = 1;
            }
            else if (stone == 0)
            {
                result = CountOfNumberAfterGeneration(1, generation - 1);
            }
            else if (HasEvenNumberOfDigits(stone))
            {
                var firstHalf = stone.ToString().Substring(0, stone.ToString().Length / 2);
                var secondHalf = stone.ToString().Substring(stone.ToString().Length / 2);
                var firstHalfInt = long.Parse(firstHalf);
                var secondHalfInt = long.Parse(secondHalf);
                result = CountOfNumberAfterGeneration(firstHalfInt, generation - 1) + CountOfNumberAfterGeneration(secondHalfInt, generation - 1);
            }
            else
            {
                result = CountOfNumberAfterGeneration(stone * 2024, generation - 1);
            }

            cache[(stone, generation)] = result;
            return result;
        }
    }
}
