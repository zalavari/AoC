using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Year2024
{


    internal class Day14 : ISolvable
    {

        private List<(int, int)> directions = new List<(int, int)> { (0, 1), (1, 0), (0, -1), (-1, 0) };
        private class Point
        {
            public Point(long x, long y)
            {
                X = x;
                Y = y;
            }

            public long X { get; set; }
            public long Y { get; set; }
        }

        private class Robot
        {
            public Point P { get; set; }
            public Point V { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();

            var robots = new List<Robot>();

            foreach (var line in lines)
            {
                // example: p=28,64 v=73,39
                Regex pattern = new Regex(@"p=(?<px>[0-9-]+),(?<py>[0-9-]+) v=(?<vx>[0-9-]+),(?<vy>[0-9-]+)");
                Match match = pattern.Match(line);

                int px = int.Parse(match.Groups["px"].Value);
                int py = int.Parse(match.Groups["py"].Value);
                int vx = int.Parse(match.Groups["vx"].Value);
                int vy = int.Parse(match.Groups["vy"].Value);

                robots.Add(new Robot
                {
                    P = new Point(px, py),
                    V = new Point(vx, vy)
                });
            }

            var Height = 7;
            var Width = 11;

            Height = 103;
            Width = 101;
            var timeToSimulate = 10000;

            //    Simulate(robots, Height, Width, timeToSimulate);

            for (int i = 1; i < timeToSimulate; i++)
            {
                Console.WriteLine($"Iteration {i}:");
                Simulate(robots, Height, Width, 1);

                if (i == 100)
                {
                    CalculateSafetyFactor(robots, Height, Width);
                }

                var distinct = robots.Select(r => (r.P.X, r.P.Y)).Distinct().Count();

                if (distinct == robots.Count)
                {
                    Console.WriteLine($"Robots are not overlapping at iteration {i}");
                    Print(robots, Height, Width);
                    break;
                }

            }

        }

        private static void CalculateSafetyFactor(List<Robot> robots, int Height, int Width)
        {
            var robotCountInQuadrant = new List<int>();
            robotCountInQuadrant.Add(robots.Where(r => r.P.X < Width / 2 && r.P.Y < Height / 2).Count());
            robotCountInQuadrant.Add(robots.Where(r => r.P.X > (Width / 2) && r.P.Y < Height / 2).Count());
            robotCountInQuadrant.Add(robots.Where(r => r.P.X < Width / 2 && r.P.Y > (Height / 2)).Count());
            robotCountInQuadrant.Add(robots.Where(r => r.P.X > (Width / 2) && r.P.Y > (Height / 2)).Count());

            var safetyFactor = 1;
            foreach (var count in robotCountInQuadrant)
            {
                safetyFactor *= count;
            }

            Console.WriteLine(safetyFactor);
        }

        private static bool InBounds(List<List<int>> map, int row, int col)
        {
            return row >= 0 && row < map.Count && col >= 0 && col < map[0].Count;
        }




        private static void Simulate(List<Robot> robots, int Height, int Width, int timeToSimulate)
        {
            foreach (var robot in robots)
            {
                var newX = (((robot.P.X + (robot.V.X * timeToSimulate)) % Width) + Width) % Width;
                var newY = (((robot.P.Y + (robot.V.Y * timeToSimulate)) % Height) + Height) % Height;

                robot.P = new Point(newX, newY);
            }
        }


        private static void Print(List<Robot> robots, int Height, int Width)
        {
            var stringBuilder = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var count = robots.Count(r => r.P.X == x && r.P.Y == y);
                    if (count > 0)
                    {
                        stringBuilder.Append(count);
                    }
                    else
                    {
                        stringBuilder.Append(".");
                    }
                }
                stringBuilder.AppendLine();
            }

            Console.WriteLine(stringBuilder.ToString());
        }
    }
}
