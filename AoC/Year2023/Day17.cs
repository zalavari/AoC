using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day17 : ISolvable
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
            public int StraightSteps { get; set; }
            public int HeatLoss { get; set; }
        }

        public void Solve(string path)
        {
            bool solveForPart2 = true;
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            var city = lines.Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

            var prQueue = new PriorityQueue<Node, int>();
            prQueue.Enqueue(new Node { Row = 0, Col = 0, Direction = Direction.Right, StraightSteps = 0, HeatLoss = 0 }, 0);
            prQueue.Enqueue(new Node { Row = 0, Col = 0, Direction = Direction.Down, StraightSteps = 0, HeatLoss = 0 }, 0);

            var visited = new HashSet<(int row, int col, Direction dir, int steps)>();
            foreach (var (node, _) in prQueue.UnorderedItems)
            {
                visited.Add((node.Row, node.Col, node.Direction, node.StraightSteps));
            }

            while (prQueue.Count > 0)
            {
                var node = prQueue.Dequeue();

                if (node.Row == city.Length - 1 && node.Col == city[0].Length - 1)
                {
                    Console.WriteLine($"Part1: {node.HeatLoss}");
                    return;
                }

                var nextNodes = new List<Node>();

                if (solveForPart2 && node.StraightSteps < 4)
                {
                    switch (node.Direction)
                    {
                        case Direction.Up:
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up, StraightSteps = node.StraightSteps + 1 });
                            break;
                        case Direction.Right:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right, StraightSteps = node.StraightSteps + 1 });
                            break;
                        case Direction.Down:
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down, StraightSteps = node.StraightSteps + 1 });
                            break;
                        case Direction.Left:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left, StraightSteps = node.StraightSteps + 1 });
                            break;
                    }
                }
                else
                {
                    switch (node.Direction)
                    {
                        case Direction.Up:
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up, StraightSteps = node.StraightSteps + 1 });
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right, StraightSteps = 1 });
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left, StraightSteps = 1 });
                            break;
                        case Direction.Right:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right, StraightSteps = node.StraightSteps + 1 });
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down, StraightSteps = 1 });
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up, StraightSteps = 1 });
                            break;
                        case Direction.Down:
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down, StraightSteps = node.StraightSteps + 1 });
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col + 1, Direction = Direction.Right, StraightSteps = 1 });
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left, StraightSteps = 1 });
                            break;
                        case Direction.Left:
                            nextNodes.Add(new Node { Row = node.Row, Col = node.Col - 1, Direction = Direction.Left, StraightSteps = node.StraightSteps + 1 });
                            nextNodes.Add(new Node { Row = node.Row + 1, Col = node.Col, Direction = Direction.Down, StraightSteps = 1 });
                            nextNodes.Add(new Node { Row = node.Row - 1, Col = node.Col, Direction = Direction.Up, StraightSteps = 1 });
                            break;
                    }
                }

                foreach (var nextNode in nextNodes)
                {
                    if (nextNode.Row < 0 || nextNode.Row >= city.Length ||
                                               nextNode.Col < 0 || nextNode.Col >= city[0].Length)
                    {
                        continue;
                    }


                    if (solveForPart2 && nextNode.StraightSteps > 10)
                    {
                        continue;
                    }
                    else if (!solveForPart2 && nextNode.StraightSteps > 3)
                    {
                        continue;
                    }

                    if (visited.Contains((nextNode.Row, nextNode.Col, nextNode.Direction, nextNode.StraightSteps)))
                    {
                        continue;
                    }

                    nextNode.HeatLoss = node.HeatLoss + city[nextNode.Row][nextNode.Col];

                    visited.Add((nextNode.Row, nextNode.Col, nextNode.Direction, nextNode.StraightSteps));
                    prQueue.Enqueue(nextNode, nextNode.HeatLoss);
                }
            }
        }
    }
}
