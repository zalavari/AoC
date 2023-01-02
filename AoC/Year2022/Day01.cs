using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    internal class Day01 : ISolvable
    {

        public void Solve(string path)
        {
            var lines = File.ReadAllLines(path);
            //var N = int.Parse(lines[0]);
            //var M = lines[1].Split(" ").Select(x => int.Parse(x)).ToList();
            var sums = new List<int>();

            var calSum = 0;

            foreach (var line in lines)
            {
                if (line == "")
                {
                    sums.Add(calSum);
                    calSum = 0;
                }
                else
                {
                    calSum += int.Parse(line);
                }
            }

            sums.Add(calSum);
            calSum = 0;

            sums.Sort();
            sums.Reverse();

            Console.WriteLine(path);
            Console.WriteLine($"Max: {sums.Max()}");
            Console.WriteLine($"Top3: {sums.Take(3).Sum()}");

            Console.WriteLine();
        }
    }
}
