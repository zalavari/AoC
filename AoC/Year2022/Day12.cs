using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{
    internal class Day12 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var matrix = new List<List<int>>();

            var lines = File.ReadAllLines(path).ToList();

            int SX = 0, SY = 0;
            int EX = 0, EY = 0;

            for (int l = 0; l < lines.Count; l++)
            {
                matrix.Add(new List<int>());
                var line = lines[l];
                for (int c = 0; c < line.Length; c++)
                {
                    var character = line[c];
                    if (character == 'S')
                    {
                        SX = l;
                        SY = c;
                        matrix[l].Add(0);
                    }
                    else if (character == 'E')
                    {
                        EX = l;
                        EY = c;
                        matrix[l].Add('z' - 'a');
                    }
                    else
                    {
                        matrix[l].Add(line[c] - 'a');
                    }
                }
            }

            var queue = new Queue<(int, int)>();
            var dict = new Dictionary<(int, int), int>();

            queue.Enqueue((EX, EY));
            dict.Add((EX, EY), 0);

            while (queue.Any())
            {
                var (x, y) = queue.Dequeue();

                if (x - 1 >= 0 && matrix[x][y] - matrix[x - 1][y] <= 1 && !dict.ContainsKey((x - 1, y)))
                {
                    dict.Add((x - 1, y), dict[(x, y)] + 1);
                    queue.Enqueue((x - 1, y));
                }

                if (x + 1 < matrix.Count && matrix[x][y] - matrix[x + 1][y] <= 1 && !dict.ContainsKey((x + 1, y)))
                {
                    dict.Add((x + 1, y), dict[(x, y)] + 1);
                    queue.Enqueue((x + 1, y));
                }

                if (y - 1 >= 0 && matrix[x][y] - matrix[x][y - 1] <= 1 && !dict.ContainsKey((x, y - 1)))
                {
                    dict.Add((x, y - 1), dict[(x, y)] + 1);
                    queue.Enqueue((x, y - 1));
                }

                if (y + 1 < matrix.First().Count() && matrix[x][y] - matrix[x][y + 1] <= 1 && !dict.ContainsKey((x, y + 1)))
                {
                    dict.Add((x, y + 1), dict[(x, y)] + 1);
                    queue.Enqueue((x, y + 1));
                }
            }

            var minimumDistance = int.MaxValue;
            for (int x = 0; x < matrix.Count; x++)
            {
                for (int y = 0; y < matrix.First().Count; y++)
                {
                    if (matrix[x][y] == 0 && dict.TryGetValue((x, y), out var distance))
                    {
                        if (distance < minimumDistance)
                            minimumDistance = distance;
                    }
                }
            }


            Console.WriteLine();
            Console.WriteLine($"resultA: {dict[(SX, SY)]}");
            Console.WriteLine($"resultB: {minimumDistance}");
            Console.WriteLine();
        }



    }
}
