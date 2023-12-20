using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day16 : ISolvable
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
            public Direction Direction { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            var map = new char[lines.Length, lines[0].Length];
            lines.Select((line, row) => line.Select((c, col) => map[row, col] = c).ToArray()).ToArray();


            var start = new Node { Row = 0, Col = 0, Direction = Direction.Right };
            int part1 = CountEnergizedTiles(map, start);

            int part2 = 0;

            for (int row = 0; row < map.GetLength(0); row++)
            {
                start = new Node { Row = row, Col = 0, Direction = Direction.Right };
                int energizedCount = CountEnergizedTiles(map, start);
                if (energizedCount > part2)
                {
                    part2 = energizedCount;
                }

                start = new Node { Row = row, Col = map.GetLength(1) - 1, Direction = Direction.Left };
                energizedCount = CountEnergizedTiles(map, start);
                if (energizedCount > part2)
                {
                    part2 = energizedCount;
                }
            }

            for (int col = 0; col < map.GetLength(1); col++)
            {
                start = new Node { Row = 0, Col = col, Direction = Direction.Down };
                int energizedCount = CountEnergizedTiles(map, start);
                if (energizedCount > part2)
                {
                    part2 = energizedCount;
                }

                start = new Node { Row = map.GetLength(0) - 1, Col = col, Direction = Direction.Up };
                energizedCount = CountEnergizedTiles(map, start);
                if (energizedCount > part2)
                {
                    part2 = energizedCount;
                }
            }

            Console.WriteLine($"Part1: {part1}");
            Console.WriteLine($"Part2: {part2}");
        }

        private static int CountEnergizedTiles(char[,] map, Node start)
        {
            var visited = new HashSet<(int, int, Direction)>();
            var queue = new Queue<Node>();

            queue.Enqueue(start);
            visited.Add((start.Row, start.Col, start.Direction));

            while (queue.Any())
            {
                var node = queue.Dequeue();
                var nextNodes = new List<Node>();

                if (map[node.Row, node.Col] == '/')
                {
                    switch (node.Direction)
                    {
                        case Direction.Up:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right });
                            break;
                        case Direction.Right:
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up });
                            break;
                        case Direction.Down:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left });
                            break;
                        case Direction.Left:
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down });
                            break;
                    }
                }
                else if (map[node.Row, node.Col] == '\\')
                {
                    switch (node.Direction)
                    {
                        case Direction.Up:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left });
                            break;
                        case Direction.Right:
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down });
                            break;
                        case Direction.Down:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right });
                            break;
                        case Direction.Left:
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up });
                            break;
                    }
                }
                else if (map[node.Row, node.Col] == '-')
                {
                    switch (node.Direction)
                    {
                        case Direction.Up:
                        case Direction.Down:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left });
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right });
                            break;
                        case Direction.Right:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right });
                            break;
                        case Direction.Left:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left });
                            break;
                    }
                }
                else if (map[node.Row, node.Col] == '|')
                {
                    switch (node.Direction)
                    {
                        case Direction.Right:
                        case Direction.Left:
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up });
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down });
                            break;
                        case Direction.Up:
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up });
                            break;
                        case Direction.Down:
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down });
                            break;
                    }
                }
                else
                {
                    switch (node.Direction)
                    {
                        case Direction.Up:
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up });
                            break;
                        case Direction.Right:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right });
                            break;
                        case Direction.Down:
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down });
                            break;
                        case Direction.Left:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left });
                            break;
                    }
                }

                foreach (var nextNode in nextNodes)
                {
                    if (nextNode.Row < 0 || nextNode.Row >= map.GetLength(0) ||
                                               nextNode.Col < 0 || nextNode.Col >= map.GetLength(1))
                    {
                        continue;
                    }
                    if (visited.Contains((nextNode.Row, nextNode.Col, nextNode.Direction)))
                    {
                        continue;
                    }

                    visited.Add((nextNode.Row, nextNode.Col, nextNode.Direction));
                    queue.Enqueue(nextNode);
                }
            }

            var distinctVisitedCount = visited.ToArray().Select(entry => (entry.Item1, entry.Item2)).Distinct().ToArray().Count();
            return distinctVisitedCount;
        }

        private static void PrintMap(char[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                var line = "";
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    line += map[row, col];
                }
                Console.WriteLine(line);
            }
        }


    }
}
