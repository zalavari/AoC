using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    internal class Day15 : ISolvable
    {
        public void Solve(string path)
        {
            Console.WriteLine(path);

            var sensors = new List<Sensor>();
            var lines = File.ReadAllLines(path).ToList();

            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"Sensor at x=(?<sx>-?\d+), y=(?<sy>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)");
                Match match = pattern.Match(line);
                int sx = int.Parse(match.Groups["sx"].Value);
                int sy = int.Parse(match.Groups["sy"].Value);
                int bx = int.Parse(match.Groups["bx"].Value);
                int by = int.Parse(match.Groups["by"].Value);

                sensors.Add(new Sensor() { X = sx, Y = sy, BX = bx, BY = by });

            }

            var result = 0;

            var minX = -10000000;
            var maxX = 10000000;
            var minY = -10000000;
            var maxY = 10000000;
            var y0 = 2000000;

            /*
            for (int x = minX; x < maxX; x++)
            {
                var alreadyIsABacon = sensors.Any(s => s.BX == x && s.BY == y0);
                var cant = sensors.Any(s => GetDistance(x, y0, s.X, s.Y) <= s.GetDistance());
                
                if (!alreadyIsABacon && cant)
                {
                    result++;
                }
            }*/

            minX = 0;
            minY = 0;
            maxX = 4000000;
            maxY = 4000000;
            for (int y = minY; y < maxY; y++)
            {
                var intervals = sensors
                    .Select(s => s.GetIntersections(y))
                    .Where(s => s.Count >= 2)
                    .Where(s => minX <= s[1] && s[0] <= maxX)
                    .OrderBy(s => s[0])
                    .Select(s => (s[0], s[1]))
                    .ToList();

                foreach (var interval in intervals)
                {
                    // Console.WriteLine($"y: {y} interval: {interval.Item1}/{interval.Item2}");
                }

                var start = intervals[0].Item1;
                var end = intervals[0].Item2;
                for (int i = 1; i < intervals.Count; i++)
                {
                    if (end + 1 <= intervals[i].Item1 - 1)
                    {
                        Console.WriteLine($"Can be distress here: x: {end + 1}-{intervals[i].Item1 - 1}, y: {y}, freq:{(long)4000000 * (end + 1) + y}");
                    }
                    end = Math.Max(intervals[i].Item2, end);
                    // Console.WriteLine(end);
                }

                if (start < minX || maxX < end)
                {
                    //Console.WriteLine($"Can be distress here: ~not very likely~");
                }
            }


            Console.WriteLine();
            Console.WriteLine($"result: {result}");
            Console.WriteLine();
        }

        static int GetDistance(int X1, int Y1, int X2, int Y2)
        {
            return Math.Abs(X1 - X2) + Math.Abs(Y1 - Y2);
        }

        private class Sensor
        {
            public int X, Y;
            public int BX, BY;

            public int GetDistance() => Day15.GetDistance(X, Y, BX, BY);

            public List<int> GetIntersections(int y)
            {

                var a = GetDistance() - Math.Abs(Y - y);
                if (a < 0)
                    return new List<int>();
                return new List<int>() { X - a, X + a };
            }

        }


    }
}
