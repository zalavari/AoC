using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day13 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var text = File.ReadAllText(path);
            var maps = text
                .Split("\n\n")
                .Select(t => t.Split("\n").Where(t => t.Length > 0).ToArray())
                .ToArray();

            var part1 = new List<int>();
            var part2 = new List<int>();
            foreach (var map in maps)
            {
                int originalMirrorNumber = CalculateMirrorNumber(map, -1);
                part1.Add(originalMirrorNumber);
                int mirrorNumber2 = CalculateMirrorNumberForSmudges(map, originalMirrorNumber);
                if (mirrorNumber2 != -1)
                {
                    part2.Add(mirrorNumber2);
                }
            }

            Console.WriteLine($"{part1.Sum()}");
            Console.WriteLine($"{part2.Sum()}");
        }

        private int CalculateMirrorNumberForSmudges(string[] map, int originalMirrorNumber)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    var mapClone = map.Select(r => r.ToCharArray()).ToArray();

                    if (mapClone[i][j] == '#')
                    {
                        mapClone[i][j] = '.';
                    }
                    else
                    {
                        mapClone[i][j] = '#';
                    }

                    var mirrorNumberCandidate = CalculateMirrorNumber(mapClone.Select(r => new string(r)).ToArray(), originalMirrorNumber);
                    if (mirrorNumberCandidate != -1)
                    {
                        return mirrorNumberCandidate;
                    }
                }
            }

            return -1;
        }

        private int CalculateMirrorNumber(string[] rows, int originalMirrorNumber)
        {
            for (int i = 0; i < rows.Length - 1; i++)
            {
                if (IsSymmetricAt(rows, i))
                {
                    //Console.WriteLine($"Horizontal mirror: {i}");
                    if (originalMirrorNumber != 100 * (i + 1))
                        return 100 * (i + 1);
                }
            }

            var columns = Enumerable.Range(0, rows[0].Length).Select(i => new string(rows.Select(r => r[i]).ToArray())).ToArray();
            for (int i = 0; i < columns.Length - 1; i++)
            {
                if (IsSymmetricAt(columns, i))
                {
                    //Console.WriteLine($"Vertical mirror: {i}");
                    if (originalMirrorNumber != (i + 1))
                        return i + 1;
                }
            }

            return -1;
        }

        private bool IsSymmetricAt(string[] rows, int position)
        {
            var a = position;
            var b = position + 1;
            while (a >= 0 && b < rows.Length)
            {
                if (rows[a] != rows[b])
                {
                    return false;
                }
                a--;
                b++;
            }
            return true;
        }

    }
}
