using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day11 : ISolvable
    {



        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            // Initialize variables
            var points = new List<(int Row, int Col)>();
            var emptyColsUntil = new List<int>() { 0 };
            var emptyRowsUntil = new List<int>() { 0 };

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.Contains("#"))
                {
                    emptyRowsUntil.Add(emptyRowsUntil.Last());
                }
                else
                {
                    emptyRowsUntil.Add(emptyRowsUntil.Last() + 1);
                }

                for (int j = 0; j < line.Length; j++)
                {
                    var c = line[j];
                    if (c == '#')
                    {
                        points.Add((i, j));
                    }
                }
            }

            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines.Select(l => l[j]).Any(c => c == '#'))
                {
                    emptyColsUntil.Add(emptyColsUntil.Last());
                }
                else
                {
                    emptyColsUntil.Add(emptyColsUntil.Last() + 1);
                }
            }

            // Calculate distances
            var distances = new List<long>();

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    var (Row, Col) = points[i];
                    var p2 = points[j];
                    var distance = Math.Abs(Row - p2.Row) + Math.Abs(Col - p2.Col);
                    var distanceWithExpansion = distance + (999999L * (Math.Abs(emptyRowsUntil[Row] - emptyRowsUntil[p2.Row]) + Math.Abs(emptyColsUntil[Col] - emptyColsUntil[p2.Col])));
                    distances.Add(distanceWithExpansion);
                }
            }

            Console.WriteLine($"{distances.Sum()}");
        }
    }
}
