using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day01 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var list1 = new List<int>();
            var list2 = new List<int>();

            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                var numbers = line.Split("   ").Select(int.Parse).ToList();
                list1.Add(numbers[0]);
                list2.Add(numbers[1]);
            }

            list1.Sort();
            list2.Sort();

            var sumOfDiffs = list1.Select((n, i) => Math.Abs(n - list2[i])).ToList().Sum();

            var dictionary = new Dictionary<int, int>();
            foreach (var number in list2)
            {
                if (dictionary.ContainsKey(number))
                {
                    dictionary[number]++;
                }
                else
                {
                    dictionary.Add(number, 1);
                }
            }

            var similarityScore = 0;
            foreach (var number in list1)
            {
                if (dictionary.ContainsKey(number))
                {
                    similarityScore += number * dictionary[number];
                }
            }

            Console.WriteLine(sumOfDiffs);
            Console.WriteLine(similarityScore);
        }
    }
}
