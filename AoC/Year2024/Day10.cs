using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day10 : ISolvable
    {
        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();

            var map = lines.Select(lines => lines.ToCharArray().Select(c => int.Parse(c.ToString())).ToList()).ToList();
            var solution1 = 0;
            var solution2 = 0;

            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j] == 0)
                    {
                        var t = GetReachableEnds(0, i, j, map).Distinct().Count();
                        var t2 = GetReachableEnds(0, i, j, map).Count();
                        Console.WriteLine($"[{i},{j}]: {t}");
                        solution1 += t;
                        solution2 += t2;
                    }
                }
            }

            Console.WriteLine(solution1);
            Console.WriteLine(solution2);
        }

        public List<Point> GetReachableEnds(int expectedHeight, int i, int j, List<List<int>> map)
        {
            if (OutsideBoundary(i, j, map) || expectedHeight != map[i][j])
            {
                return new List<Point>();
            }

            if (map[i][j] == 9)
            {
                return new List<Point>() { new Point() { X = i, Y = j } };
            }

            var result = new List<Point>();
            result.AddRange(GetReachableEnds(expectedHeight + 1, i + 1, j, map));
            result.AddRange(GetReachableEnds(expectedHeight + 1, i - 1, j, map));
            result.AddRange(GetReachableEnds(expectedHeight + 1, i, j + 1, map));
            result.AddRange(GetReachableEnds(expectedHeight + 1, i, j - 1, map));

            return result;
        }

        private static bool OutsideBoundary(int i, int j, List<List<int>> map)
        {
            return i < 0 || i >= map.Count || j < 0 || j >= map[i].Count;
        }

    }
}
