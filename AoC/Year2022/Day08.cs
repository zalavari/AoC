using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{
    internal class Day08 : ISolvable
    {
        public void Solve(string path)
        {
            var lines = File.ReadAllLines(path).ToList();

            // var mtx = new List<List<int>>();

            var map = lines.Select(line => line.Select(c => int.Parse(c.ToString())).ToList()).ToList();
            var visibles = new List<List<bool>>();
            var scenicScores = new List<List<int>>();

            var height = map.Count;
            var width = map.First().Count;

            for (int i = 0; i < height; i++)
            {
                visibles.Add(new List<bool>());
                scenicScores.Add(new List<int>());
                for (int j = 0; j < width; j++)
                {
                    visibles[i].Add(false);
                    scenicScores[i].Add(1);
                }
            }

            var size = height;

            MarkVisibles(map, visibles);
            CalculateScenicFromOneDirection(map, scenicScores);

            RotateMatrix(map, size);
            RotateMatrix(visibles, size);
            RotateMatrix(scenicScores, size);

            MarkVisibles(map, visibles);
            CalculateScenicFromOneDirection(map, scenicScores);

            RotateMatrix(map, size);
            RotateMatrix(visibles, size);
            RotateMatrix(scenicScores, size);

            MarkVisibles(map, visibles);
            CalculateScenicFromOneDirection(map, scenicScores);

            RotateMatrix(map, size);
            RotateMatrix(visibles, size);
            RotateMatrix(scenicScores, size);

            MarkVisibles(map, visibles);
            CalculateScenicFromOneDirection(map, scenicScores);

            RotateMatrix(map, size);
            RotateMatrix(visibles, size);
            RotateMatrix(scenicScores, size);


            Console.WriteLine(path);

            Console.WriteLine($"resultA: {visibles.Sum(l => l.Count(t => t))}");
            Console.WriteLine($"resultB: {scenicScores.Max(l => l.Max())}");

            Console.WriteLine();
        }

        void CalculateScenicFromOneDirection(List<List<int>> map, List<List<int>> scenic)
        {
            var N = map.Count;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    var s = 0;
                    var k = 0;
                    for (k = j + 1; k < N && map[i][k] < map[i][j]; k++)
                    {
                        s++;
                    }

                    if (k < N)
                        s++;

                    scenic[i][j] *= s;

                }
            }
        }

        void MarkVisibles(List<List<int>> mtx, List<List<bool>> visibles)
        {
            var N = mtx.Count;

            for (int i = 0; i < N; i++)
            {
                var maxHeight = -1;
                for (int j = 0; j < N; j++)
                {
                    var height = mtx[i][j];
                    if (height > maxHeight)
                    {
                        maxHeight = height;
                        visibles[i][j] = true;
                    }
                }
            }
        }

        private void RotateMatrix<T>(List<List<T>> A, int size)
        {
            int n = size;
            for (int i = 0; i < n / 2; i++)
            {
                for (int j = i; j < n - i - 1; j++)
                {
                    T temp = A[i][j];
                    A[i][j] = A[n - 1 - j][i];
                    A[n - 1 - j][i] = A[n - 1 - i][n - 1 - j];
                    A[n - 1 - i][n - 1 - j] = A[j][n - 1 - i];
                    A[j][n - 1 - i] = temp;
                }
            }
        }

        private string GetStringRepresentation(List<List<int>> matrix)
        {
            return string.Join("\n", matrix.Select(line => string.Join("", line)));
        }

        private string GetStringRepresentationTab(List<List<int>> matrix)
        {
            return string.Join("\n", matrix.Select(line => string.Join("\t", line)));
        }


        private string GetStringRepresentation(List<List<bool>> matrix)
        {
            return string.Join("\n", matrix.Select(line => string.Join("", line.Select(b => b ? '1' : '0'))));
        }

    }
}
