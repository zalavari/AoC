using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day21 : ISolvable
    {
        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        private class Node
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int Distance { get; set; }
        }

        public void Solve(string path)
        {

            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            Part1(lines);
            Part2_Faster(lines, 6);
            Part2_Faster(lines, 10);
            Part2_Faster(lines, 50);
            Part2_Faster(lines, 100);
            Part2_Faster(lines, 500);
            Part2_Faster(lines, 1000);
            Part2_Faster(lines, 5000);
        }

        private static void Part1(string[] lines)
        {
            var H = lines.Length;
            var W = lines[0].Length;
            var walls = lines.Select(line => line.Select(c => c == '#').ToList()).ToList();

            var shortestDistances = Enumerable.Range(0, H).Select(_ => Enumerable.Range(0, W).Select(_ => int.MaxValue).ToList()).ToList();

            var queue = new Queue<Node>();

            // Find starting position marked with 'S'
            for (int row = 0; row < H; row++)
            {
                for (int col = 0; col < W; col++)
                {
                    if (lines[row][col] == 'S')
                    {
                        queue.Enqueue(new Node { Row = row, Col = col });
                        shortestDistances[row][col] = 0;
                    }
                }
            }

            while (queue.Any())
            {
                var node = queue.Dequeue();

                var neighbors = new List<Node>
                {
                    new Node { Row = node.Row - 1, Col = node.Col },
                    new Node { Row = node.Row + 1, Col = node.Col },
                    new Node { Row = node.Row, Col = node.Col - 1 },
                    new Node { Row = node.Row, Col = node.Col + 1 }
                };

                foreach (var neighbor in neighbors)
                {
                    if (neighbor.Row < 0 || neighbor.Row >= H || neighbor.Col < 0 || neighbor.Col >= W)
                    {
                        continue;
                    }

                    if (walls[neighbor.Row][neighbor.Col])
                    {
                        continue;
                    }

                    if (shortestDistances[neighbor.Row][neighbor.Col] <= shortestDistances[node.Row][node.Col] + 1)
                    {
                        continue;
                    }

                    shortestDistances[neighbor.Row][neighbor.Col] = shortestDistances[node.Row][node.Col] + 1;
                    queue.Enqueue(neighbor);
                }
            }

            var part1 = shortestDistances.SelectMany(row => row).Where(distance => distance % 2 == 0 && distance <= 64).Count();
            Console.WriteLine(part1);
        }

        private static void Part2(string[] lines, int maxDistance)
        {
            var H = lines.Length;
            var W = lines[0].Length;
            var walls = lines.Select(line => line.Select(c => c == '#').ToList()).ToList();


            var pointReached = new HashSet<(int row, int col)>();
            var queue = new Queue<Node>();

            // Find starting position marked with 'S'

            var parity = 0;
            for (int row = 0; row < H; row++)
            {
                for (int col = 0; col < W; col++)
                {
                    if (lines[row][col] == 'S')
                    {
                        queue.Enqueue(new Node { Row = row, Col = col, Distance = 0 });
                        parity = (row + col) % 2;
                    }
                }
            }

            while (queue.Any())
            {
                var node = queue.Dequeue();

                if (node.Distance >= maxDistance)
                {
                    break;
                }

                var neighbors = new List<Node>
                {
                    new Node { Row = node.Row - 1, Col = node.Col, Distance = node.Distance  +1, },
                    new Node { Row = node.Row + 1, Col = node.Col,  Distance = node.Distance  +1,},
                    new Node { Row = node.Row, Col = node.Col - 1,  Distance = node.Distance  +1,},
                    new Node { Row = node.Row, Col = node.Col + 1,  Distance = node.Distance  +1,}
                };

                foreach (var neighbor in neighbors)
                {

                    var rowWithinLimits = ((neighbor.Row % H) + H) % H;
                    var colWithinLimits = ((neighbor.Col % W) + W) % W;

                    if (walls[rowWithinLimits][colWithinLimits])
                    {
                        continue;
                    }

                    if (pointReached.TryGetValue((neighbor.Row, neighbor.Col), out _))
                    {
                        continue;
                    }

                    pointReached.Add((neighbor.Row, neighbor.Col));
                    queue.Enqueue(neighbor);
                }
            }


            var part2 = pointReached.Where(point => (point.row + point.col) % 2 == parity).Count();
            Console.WriteLine(part2);
        }

        private static void Part2_Faster(string[] lines, int maxDistance)
        {
            var H = lines.Length;
            var W = lines[0].Length;
            var walls = lines.Select(line => line.Select(c => c == '#').ToList()).ToList();


            //var pointReachedByDistance = new List<HashSet<(int row, int col)>>();
            var pointReachedByDistance = Enumerable.Range(0, maxDistance + 1).Select(_ => new HashSet<(int row, int col)>()).ToList();
            var queue = new Queue<Node>();

            // Find starting position marked with 'S'

            var parity = 0;
            for (int row = 0; row < H; row++)
            {
                for (int col = 0; col < W; col++)
                {
                    if (lines[row][col] == 'S')
                    {
                        queue.Enqueue(new Node { Row = row, Col = col, Distance = 0 });
                        parity = (row + col) % 2;
                    }
                }
            }
            pointReachedByDistance[0].Add((queue.Peek().Row, queue.Peek().Col));


            while (queue.Any())
            {
                var node = queue.Dequeue();

                if (node.Distance >= maxDistance)
                {
                    break;
                }

                var neighbors = new List<Node>
                {
                    new Node { Row = node.Row - 1, Col = node.Col, Distance = node.Distance  +1, },
                    new Node { Row = node.Row + 1, Col = node.Col,  Distance = node.Distance  +1,},
                    new Node { Row = node.Row, Col = node.Col - 1,  Distance = node.Distance  +1,},
                    new Node { Row = node.Row, Col = node.Col + 1,  Distance = node.Distance  +1,}
                };

                foreach (var neighbor in neighbors)
                {

                    var rowWithinLimits = ((neighbor.Row % H) + H) % H;
                    var colWithinLimits = ((neighbor.Col % W) + W) % W;

                    if (walls[rowWithinLimits][colWithinLimits])
                    {
                        continue;
                    }

                    if (pointReachedByDistance[neighbor.Distance].TryGetValue((neighbor.Row, neighbor.Col), out _))
                    {
                        continue;
                    }

                    if (neighbor.Distance - 2 >= 0 && pointReachedByDistance[neighbor.Distance - 2].TryGetValue((neighbor.Row, neighbor.Col), out _))
                    {
                        continue;
                    }

                    pointReachedByDistance[neighbor.Distance].Add((neighbor.Row, neighbor.Col));
                    queue.Enqueue(neighbor);
                }
            }

            var result = 0;
            for (var i = 0; i <= maxDistance; i += 2)
            {
                result += pointReachedByDistance[i].Count;
                Console.WriteLine($"Distance {i}: {pointReachedByDistance[i].Count}");
            }


            //var part2 = pointReachedByDistance.SelectMany(row => row).Where(point => (point.row + point.col) % 2 == parity).Count();
            Console.WriteLine(result);
        }

    }
}
