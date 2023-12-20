using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day03 : ISolvable
    {
        private Dictionary<(int, int), List<int>> gears;

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var parts = new List<int>();
            gears = new Dictionary<(int, int), List<int>>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                Regex pattern = new Regex(@"(?<number>\d+)");
                MatchCollection matches = pattern.Matches(line);

                foreach (Match match in matches)
                {
                    var value = int.Parse(match.Value);
                    var isPart = CheckForSymbol(i, match.Index, match.Length, lines, value);
                    if (isPart)
                    {
                        parts.Add(value);
                    }
                }
            }

            var sumOfGearRatios = gears.Where(g => g.Value.Count == 2).Select(kvp => kvp.Value[0] * kvp.Value[1]).Sum();

            //Console.WriteLine(string.Join(", ", parts));
            Console.WriteLine(parts.Sum());
            Console.WriteLine(sumOfGearRatios);
        }

        private bool CheckForSymbol(int row, int start, int length, string[] lines, int value)
        {
            var before = start > 0 && lines[row][start - 1] != '.';
            var after = start + length < lines[row].Length && lines[row][start + length] != '.';
            var isPart = before || after;

            if (start > 0 && lines[row][start - 1] == '*')
            {
                AddNumberToGear(row, start - 1, value);
            }

            if (start + length < lines[row].Length && lines[row][start + length] == '*')
            {
                AddNumberToGear(row, start + length, value);
            }


            for (int i = start - 1; i < start + length + 1; i++)
            {
                if (i < 0 || i >= lines[row].Length)
                {
                    continue;
                }

                if (row - 1 > 0 && lines[row - 1][i] != '.')
                {
                    isPart = true;
                    if (lines[row - 1][i] == '*')
                    {
                        AddNumberToGear(row - 1, i, value);
                    }
                }

                if (row + 1 < lines.Length && lines[row + 1][i] != '.')
                {
                    isPart = true;
                    if (lines[row + 1][i] == '*')
                    {
                        AddNumberToGear(row + 1, i, value);
                    }
                }
            }

            return isPart;
        }

        private void AddNumberToGear(int row, int pos, int value)
        {
            if (!gears.ContainsKey((row, pos)))
            {
                gears.Add((row, pos), new List<int>());
            }

            gears[(row, pos)].Add(value);
        }
    }
}
