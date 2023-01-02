using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    internal class Day04 : ISolvable
    {

        public void Solve(string path)
        {
            var lines = File.ReadAllLines(path);
            var parsedIntervals = lines
                .Select(line =>
                    line
                    .Split(",")
                    .Select(intervals => intervals.Split("-").Select(int.Parse).ToList())
                    .ToList())
                .ToList();


            var containingCount = 0;
            var overlappingCount = 0;
            foreach (var intervalPair in parsedIntervals)
            {
                var a = intervalPair[0];
                var b = intervalPair[1];
                if (Contains(a, b) || Contains(b, a))
                    containingCount++;

                if (Overlaps(a, b))
                    overlappingCount++;
            }

            Console.WriteLine(path);
            Console.WriteLine($"containingCount: {containingCount}");
            Console.WriteLine($"overlappingCount: {overlappingCount}");

            Console.WriteLine();
        }

        bool Contains(List<int> a, List<int> b)
        {
            return a[0] <= b[0] && b[1] <= a[1];
        }

        bool Overlaps(List<int> a, List<int> b)
        {
            return !(a[1] < b[0] || b[1] < a[0]);
        }

    }
}
