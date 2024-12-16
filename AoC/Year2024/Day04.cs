using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day04 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path);
            var mtx = lines.Select(x => x.ToList()).ToList();

            var wordToFind = "XMAS";

            Console.WriteLine(Search(mtx, wordToFind));
            Console.WriteLine(Search2(mtx));
        }

        private int Search(List<List<char>> mtx, string wordToFind)
        {
            var result = 0;
            for (int i = 0; i < mtx.Count; i++)
            {
                for (int j = 0; j < mtx[i].Count; j++)
                {
                    var directions = new List<(int, int)> { (0, 1), (1, 0), (0, -1), (-1, 0), (1, 1), (1, -1), (-1, 1), (-1, -1) };
                    foreach (var (dx, dy) in directions)
                    {
                        var k = 0;
                        while (InBoundary(mtx, i + (k * dx), j + (k * dy)) && mtx[i + (k * dx)][j + (k * dy)] == wordToFind[k])
                        {
                            k++;
                            if (k == wordToFind.Length)
                            {
                                result++;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private int Search2(List<List<char>> mtx)
        {
            var result = 0;
            for (int i = 1; i < mtx.Count - 1; i++)
            {
                for (int j = 1; j < mtx[i].Count - 1; j++)
                {
                    if (mtx[i][j] != 'A')
                    {
                        continue;
                    }

                    var diagonalMatch = (mtx[i - 1][j - 1] == 'M' && mtx[i + 1][j + 1] == 'S') || (mtx[i - 1][j - 1] == 'S' && mtx[i + 1][j + 1] == 'M');
                    var subdiagonalMatch = (mtx[i - 1][j + 1] == 'M' && mtx[i + 1][j - 1] == 'S') || (mtx[i - 1][j + 1] == 'S' && mtx[i + 1][j - 1] == 'M');

                    if (diagonalMatch && subdiagonalMatch)
                    {
                        result++;
                    }


                }
            }

            return result;
        }

        private bool InBoundary(List<List<char>> mtx, int x, int y)
        {
            return x >= 0 && x < mtx.Count && y >= 0 && y < mtx[0].Count;
        }
    }
}
