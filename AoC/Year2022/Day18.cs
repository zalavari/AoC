using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{
    internal class Day18 : ISolvable
    {
        private enum Matter
        {
            None = 0,
            Lava = 1,
            Water = 2,
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);


            var lines = File.ReadAllLines(path).Select(cs => cs.Split(',').Select(int.Parse).ToList());

            var numberOfCubes = lines.Count();

            Console.WriteLine(lines.Max(line => line.Max()));
            Console.WriteLine(lines.Min(line => line.Min()));

            var grid = new List<List<List<Matter>>>();

            var maxCoordinate = 22;

            for (int x = 0; x < maxCoordinate; x++)
            {
                grid.Add(new List<List<Matter>>());
                for (int y = 0; y < maxCoordinate; y++)
                {
                    grid[x].Add(new List<Matter>());
                    for (int z = 0; z < maxCoordinate; z++)
                    {
                        grid[x][y].Add(Matter.None);
                    }
                }
            }

            foreach (var line in lines)
            {
                var x = line[0];
                var y = line[1];
                var z = line[2];
                grid[x + 1][y + 1][z + 1] = Matter.Lava;
            }

            var touching = 0;

            for (int x = 1; x < maxCoordinate - 1; x++)
            {
                for (int y = 1; y < maxCoordinate - 1; y++)
                {
                    for (int z = 1; z < maxCoordinate - 1; z++)
                    {
                        if (grid[x][y][z] != Matter.Lava)
                            continue;

                        if (grid[x + 1][y][z] == Matter.Lava)
                            touching++;

                        if (grid[x - 1][y][z] == Matter.Lava)
                            touching++;

                        if (grid[x][y + 1][z] == Matter.Lava)
                            touching++;

                        if (grid[x][y - 1][z] == Matter.Lava)
                            touching++;

                        if (grid[x][y][z + 1] == Matter.Lava)
                            touching++;

                        if (grid[x][y][z - 1] == Matter.Lava)
                            touching++;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine($"resultA: {6 * numberOfCubes - touching}");

            var queue = new Queue<(int, int, int)>();

            queue.Enqueue((0, 0, 0));

            while (queue.Count > 0)
            {
                var (x, y, z) = queue.Dequeue();

                if (x > 0 && grid[x - 1][y][z] == Matter.None)
                {
                    grid[x - 1][y][z] = Matter.Water;
                    queue.Enqueue((x - 1, y, z));
                }
                if (y > 0 && grid[x][y - 1][z] == Matter.None)
                {
                    grid[x][y - 1][z] = Matter.Water;
                    queue.Enqueue((x, y - 1, z));
                }
                if (z > 0 && grid[x][y][z - 1] == Matter.None)
                {
                    grid[x][y][z - 1] = Matter.Water;
                    queue.Enqueue((x, y, z - 1));
                }
                if (x < maxCoordinate - 1 && grid[x + 1][y][z] == Matter.None)
                {
                    grid[x + 1][y][z] = Matter.Water;
                    queue.Enqueue((x + 1, y, z));
                }
                if (y < maxCoordinate - 1 && grid[x][y + 1][z] == Matter.None)
                {
                    grid[x][y + 1][z] = Matter.Water;
                    queue.Enqueue((x, y + 1, z));
                }
                if (z < maxCoordinate - 1 && grid[x][y][z + 1] == Matter.None)
                {
                    grid[x][y][z + 1] = Matter.Water;
                    queue.Enqueue((x, y, z + 1));
                }

            }

            touching = 0;
            for (int x = 1; x < maxCoordinate - 1; x++)
            {
                for (int y = 1; y < maxCoordinate - 1; y++)
                {
                    for (int z = 1; z < maxCoordinate - 1; z++)
                    {
                        if (grid[x][y][z] != Matter.Lava)
                            continue;

                        if (grid[x + 1][y][z] == Matter.Water)
                            touching++;

                        if (grid[x - 1][y][z] == Matter.Water)
                            touching++;

                        if (grid[x][y + 1][z] == Matter.Water)
                            touching++;

                        if (grid[x][y - 1][z] == Matter.Water)
                            touching++;

                        if (grid[x][y][z + 1] == Matter.Water)
                            touching++;

                        if (grid[x][y][z - 1] == Matter.Water)
                            touching++;
                    }
                }
            }



            Console.WriteLine($"resultB: {touching}");
            Console.WriteLine();
        }


    }
}
