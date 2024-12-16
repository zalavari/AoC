using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day08 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path).ToList();
            var mtx = lines.Select(x => x.ToList()).ToList();
            var antennasByFrequency = new Dictionary<char, List<Point>>();
            var antinodes = new List<Point>();

            for (int i = 0; i < mtx.Count; i++)
            {
                for (int j = 0; j < mtx[i].Count; j++)
                {
                    if (mtx[i][j] == '.')
                    {
                        continue;
                    }

                    if (!antennasByFrequency.ContainsKey(mtx[i][j]))
                    {
                        antennasByFrequency[mtx[i][j]] = new List<Point>();
                    }
                    antennasByFrequency[mtx[i][j]].Add(new Point(i, j));
                }
            }

            foreach (var (_, antennas) in antennasByFrequency)
            {
                for (int i = 0; i < antennas.Count; i++)
                {
                    for (int j = i + 1; j < antennas.Count; j++)
                    {
                        var vectorij = new Point(antennas[j].X - antennas[i].X, antennas[j].Y - antennas[i].Y);

                        var point = antennas[i];
                        while (InBoundary(mtx, point.X, point.Y))
                        {
                            antinodes.Add(point);
                            point = new Point(point.X - vectorij.X, point.Y - vectorij.Y);
                        }

                        point = antennas[j];
                        while (InBoundary(mtx, point.X, point.Y))
                        {
                            antinodes.Add(point);
                            point = new Point(point.X + vectorij.X, point.Y + vectorij.Y);
                        }
                    }
                }
            }
            var antinodesInsideMap = antinodes.Distinct().ToList();

            Console.WriteLine($"Solution 1: {antinodesInsideMap.Count}");

        }

        private bool InBoundary(List<List<char>> mtx, int x, int y)
        {
            return x >= 0 && x < mtx.Count && y >= 0 && y < mtx[0].Count;
        }

    }
}
