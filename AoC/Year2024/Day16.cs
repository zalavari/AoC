using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day16 : ISolvable
    {
        private enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        private static Dictionary<Direction, (int x, int y)> directions = new Dictionary<Direction, (int x, int y)>()
        {
            { Direction.Up, ( 0, -1) },
            { Direction.Down, ( 0,  1) },
            { Direction.Right, ( 1,  0) },
            { Direction.Left, (-1,  0) },
        };

        private class Node
        {
            public Point Position;
            public Direction Direction;
            public Node ReachedFrom;
            public int Steps;
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();
            var map = lines.Select(line => line.ToList()).ToList();

            var startPoint = new Point();
            var endPoint = new Point();

            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x] == 'S')
                    {
                        startPoint = new Point(x, y);
                    }
                    if (map[y][x] == 'E')
                    {
                        endPoint = new Point(x, y);
                    }
                }
            }

            var start = new Node
            {
                Position = startPoint,
                Direction = Direction.Right,
                Steps = 0
            };

            var priorityQueue = new PriorityQueue<Node, int>();
            priorityQueue.Enqueue(start, start.Steps);

            var shortestPathTo = new Dictionary<(Point, Direction), (int steps, List<Node> from)>();
            var endNodes = new List<Node>();

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();
                if (endNodes.Any() && current.Steps > endNodes.First().Steps)
                {
                    break;
                }

                if (current.Position == endPoint)
                {
                    endNodes.Add(current);
                }

                if (shortestPathTo.TryGetValue((current.Position, current.Direction), out var v))
                {
                    if (v.steps < current.Steps)
                        continue;

                    v.from.Add(current.ReachedFrom);
                    continue;
                }
                else
                {
                    shortestPathTo.Add((current.Position, current.Direction), (current.Steps, new List<Node>() { current.ReachedFrom }));
                }

                // Move forward
                var newPosition = new Point(current.Position.X + directions[current.Direction].x, current.Position.Y + directions[current.Direction].y);
                if (map[newPosition.Y][newPosition.X] != '#')
                {
                    var newNode = new Node
                    {
                        Position = newPosition,
                        Direction = current.Direction,
                        Steps = current.Steps + 1,
                        ReachedFrom = current,
                    };
                    priorityQueue.Enqueue(newNode, newNode.Steps);
                }

                // Turn clockwise
                var newDirection = (Direction)(((int)current.Direction + 1) % 4);
                var newNodeClockwise = new Node
                {
                    Position = current.Position,
                    Direction = newDirection,
                    Steps = current.Steps + 1000,
                    ReachedFrom = current,
                };
                priorityQueue.Enqueue(newNodeClockwise, newNodeClockwise.Steps);

                // Turn counter clockwise
                var newDirectionCounterClockwise = (Direction)(((int)current.Direction + 3) % 4);
                var newNodeCounterClockwise = new Node
                {
                    Position = current.Position,
                    Direction = newDirectionCounterClockwise,
                    Steps = current.Steps + 1000,
                    ReachedFrom = current,
                };
                priorityQueue.Enqueue(newNodeCounterClockwise, newNodeCounterClockwise.Steps);
            }

            // Get Points on shortest paths
            var shortestPath = new List<Node>();
            var queue = new Queue<Node>();
            foreach (var endNode in endNodes)
            {
                queue.Enqueue(endNode);
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == null)
                {
                    continue;
                }

                if (shortestPath.Contains(current))
                {
                    continue;
                }
                shortestPath.Add(current);

                shortestPathTo.TryGetValue((current.Position, current.Direction), out var v);
                foreach (var node in v.from)
                {
                    queue.Enqueue(node);
                }
            }

            Console.WriteLine(endNodes.First().Steps);
            Console.WriteLine(shortestPath.Select(node => node.Position).Distinct().ToList().Count());
        }


        private static void PrintMap(List<List<char>> map)
        {
            foreach (var line in map)
            {
                Console.WriteLine(string.Join("", line));
            }
        }

    }
}
