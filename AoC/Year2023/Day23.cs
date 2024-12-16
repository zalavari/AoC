using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day23 : ISolvable
    {
        private class Point
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int Distance { get; set; } // Only used in part 2
        }

        public void Solve(string filePath)
        {
            Console.WriteLine(filePath);
            var lines = File.ReadAllLines(filePath);

            var grid = lines.Select(line => line.ToList()).ToList();

            var startingPosition = new Point { Row = 0, Col = grid[0].IndexOf('.') };
            var startingPath = new List<Point>();
            startingPath.Add(startingPosition);
            var targetPosition = new Point { Row = grid.Count - 1, Col = grid[grid.Count - 1].IndexOf('.') };

            // var result = GetLongestPathPart1(grid, startingPath, targetPosition);
            Part2WithGraph(lines);
        }

        private class Node
        {
            public Point Position { get; set; }
            public int Distance { get; set; }
            public List<(Node neighbour, int weight)> Neighbors { get; set; } = new();
        }

        private void Part2WithGraph(string[] lines)
        {
            var walls = lines.Select(line => line.Select(c => c == '#').ToList()).ToList();
            var startingNode = new Node()
            {
                Position = new Point { Row = 0, Col = walls[0].IndexOf(false) }
            };
            var targetNode = new Node()
            {
                Position = new Point { Row = walls.Count - 1, Col = walls[walls.Count - 1].IndexOf(false) }
            };
            var nodes = GetIntersections(walls);
            nodes.Add(startingNode);
            nodes.Add(targetNode);

            // Print Nodes
            foreach (var node in nodes)
            {
                Console.WriteLine($"Node: ({node.Position.Row},{node.Position.Col})");
            }

            CalculateNeighbours(nodes, walls);

            // Print Nodes
            foreach (var node in nodes)
            {
                Console.WriteLine($"Node: ({node.Position.Row},{node.Position.Col}) CountOfNeighbours: ({node.Neighbors.Count})");
                Console.WriteLine($"Neighbours: {string.Join(", ", node.Neighbors.Select(n => $"{n.neighbour.Position.Row},{n.neighbour.Position.Col},{n.weight}"))}");
            }

            Console.WriteLine($"Intersections: {nodes.Count - 2}");

            // Calculate longest path
            var result = GetLongestPath(new List<Node>() { startingNode }, targetNode);
            Console.WriteLine(result);
        }

        private int GetLongestPath(List<Node> path, Node target)
        {
            // Console.WriteLine($"Inspecting: {string.Join(", ", path.Select(n => $"({n.Row},{n.Col})"))}");
            var node = path.Last();

            if (node.Position.Row == target.Position.Row && node.Position.Col == target.Position.Col)
                return node.Distance;

            var nextNodes = node.Neighbors
                .Where(n => !path.Any(p => p.Position.Row == n.neighbour.Position.Row && p.Position.Col == n.neighbour.Position.Col))
                .ToList();

            var longest = 0;
            foreach (var (neighbour, weight) in nextNodes)
            {
                var newPath = new List<Node>(path);
                newPath.Add(new Node() { Position = neighbour.Position, Neighbors = neighbour.Neighbors, Distance = node.Distance + weight });
                var l = GetLongestPath(newPath, target);
                if (l > longest)
                    longest = l;
            }
            return longest;
        }

        private void CalculateNeighbours(List<Node> nodes, List<List<bool>> walls)
        {
            foreach (var node in nodes)
            {
                var queue = new Queue<Point>();
                queue.Enqueue(node.Position);
                var visited = new HashSet<(int row, int col)>();
                visited.Add((node.Position.Row, node.Position.Col));
                while (queue.Count > 0)
                {
                    var point = queue.Dequeue();

                    //if (visited.Contains((point.Row, point.Col)))
                    //    continue;

                    //if (walls[point.Row][point.Col])
                    //    continue;

                    if ((node.Position.Row != point.Row || node.Position.Col != point.Col) && nodes.Any(n => n.Position.Row == point.Row && n.Position.Col == point.Col))
                    {
                        var neighbour = nodes.First(n => n.Position.Row == point.Row && n.Position.Col == point.Col);
                        node.Neighbors.Add((neighbour, point.Distance));
                        continue;
                    }

                    var nextPoints = new List<Point>();
                    nextPoints.Add(new Point { Row = point.Row - 1, Col = point.Col, Distance = point.Distance + 1 });
                    nextPoints.Add(new Point { Row = point.Row + 1, Col = point.Col, Distance = point.Distance + 1 });
                    nextPoints.Add(new Point { Row = point.Row, Col = point.Col - 1, Distance = point.Distance + 1 });
                    nextPoints.Add(new Point { Row = point.Row, Col = point.Col + 1, Distance = point.Distance + 1 });

                    //var n1 = nextPoints
                    //    .Where(n => n.Row >= 0 && n.Row < walls.Count && n.Col >= 0 && n.Col < walls[0].Count)
                    //    .ToList();

                    //var n2 = n1
                    //    .Where(n => !walls[n.Row][n.Col])
                    //    .ToList();

                    //var n3 = n2
                    //    .Where(n => !visited.Contains((n.Row, n.Col)))
                    //    .ToList();

                    nextPoints = nextPoints
                        .Where(n => n.Row >= 0 && n.Row < walls.Count && n.Col >= 0 && n.Col < walls[0].Count)
                        .Where(n => !walls[n.Row][n.Col])
                        .Where(n => !visited.Contains((n.Row, n.Col)))
                        .ToList();

                    foreach (var nextPoint in nextPoints)
                    {
                        queue.Enqueue(nextPoint);
                        visited.Add((nextPoint.Row, nextPoint.Col));
                    }
                }
            }
        }

        private static List<Node> GetIntersections(List<List<bool>> walls)
        {
            var nodes = new List<Node>();
            for (int i = 0; i < walls.Count; i++)
            {
                for (int j = 0; j < walls[i].Count; j++)
                {
                    if (walls[i][j])
                        continue;

                    var nextPoints = new List<Point>();
                    nextPoints.Add(new Point { Row = i - 1, Col = j, });
                    nextPoints.Add(new Point { Row = i + 1, Col = j, });
                    nextPoints.Add(new Point { Row = i, Col = j - 1, });
                    nextPoints.Add(new Point { Row = i, Col = j + 1, });

                    nextPoints = nextPoints
                        .Where(n => n.Row >= 0 && n.Row < walls.Count && n.Col >= 0 && n.Col < walls[0].Count)
                        .Where(n => !walls[n.Row][n.Col])
                        .ToList();

                    if (nextPoints.Count <= 2)
                        continue;

                    // It's a junction
                    var node = new Node()
                    {
                        Position = new Point { Row = i, Col = j }
                    };
                    nodes.Add(node);
                }
            }
            return nodes;
        }



        private int GetLongestPathPart2NonRecurseButSlow(List<List<char>> grid, List<Point> path, Point target)
        {
            // Console.WriteLine($"Inspecting: {string.Join(", ", path.Select(n => $"({n.Row},{n.Col})"))}");

            var stack = new Stack<HashSet<Point>>();
            stack.Push(new HashSet<Point>(path));
            var longest = 0;
            while (stack.Count > 0)
            {
                var currentPath = stack.Pop();
                var point = currentPath.Last();
                if (point.Row == target.Row && point.Col == target.Col)
                {
                    if (currentPath.Count > longest)
                        longest = currentPath.Count;
                    continue;
                }

                var nextPoints = new List<Point>();
                nextPoints.Add(new Point { Row = point.Row - 1, Col = point.Col, });
                nextPoints.Add(new Point { Row = point.Row + 1, Col = point.Col, });
                nextPoints.Add(new Point { Row = point.Row, Col = point.Col - 1, });
                nextPoints.Add(new Point { Row = point.Row, Col = point.Col + 1, });

                nextPoints = nextPoints
                    .Where(n => n.Row >= 0 && n.Row < grid.Count && n.Col >= 0 && n.Col < grid[0].Count)
                    .Where(n => grid[n.Row][n.Col] != '#')
                    .Where(n => !currentPath.Any(p => p.Row == n.Row && p.Col == n.Col))
                    .ToList();

                if (nextPoints.Count == 0)
                    continue;
                else if (nextPoints.Count == 1)
                {
                    currentPath.Add(nextPoints[0]);
                    stack.Push(currentPath);
                    continue;
                }
                else
                {
                    foreach (var nextPoint in nextPoints)
                    {
                        var newPath = new HashSet<Point>(currentPath);
                        newPath.Add(nextPoint);
                        stack.Push(newPath);
                    }
                }

            }
            return longest;
        }

        private int GetLongestPathPart1(List<List<char>> grid, List<Point> path, Point target)
        {
            // Console.WriteLine($"Inspecting: {string.Join(", ", path.Select(n => $"({n.Row},{n.Col})"))}");
            var node = path.Last();

            if (node.Row == target.Row && node.Col == target.Col)
                return path.Count;

            var nextNodes = new List<Point>();

            if (grid[node.Row][node.Col] == '^')
                nextNodes.Add(new Point { Row = node.Row - 1, Col = node.Col, });
            else if (grid[node.Row][node.Col] == 'v')
                nextNodes.Add(new Point { Row = node.Row + 1, Col = node.Col, });
            else if (grid[node.Row][node.Col] == '<')
                nextNodes.Add(new Point { Row = node.Row, Col = node.Col - 1, });
            else if (grid[node.Row][node.Col] == '>')
                nextNodes.Add(new Point { Row = node.Row, Col = node.Col + 1, });
            else
            {
                nextNodes.Add(new Point { Row = node.Row - 1, Col = node.Col, });
                nextNodes.Add(new Point { Row = node.Row + 1, Col = node.Col, });
                nextNodes.Add(new Point { Row = node.Row, Col = node.Col - 1, });
                nextNodes.Add(new Point { Row = node.Row, Col = node.Col + 1, });
            }

            nextNodes = nextNodes
                .Where(n => n.Row >= 0 && n.Row < grid.Count && n.Col >= 0 && n.Col < grid[0].Count)
                .Where(n => grid[n.Row][n.Col] != '#')
                .Where(n => !path.Any(p => p.Row == n.Row && p.Col == n.Col))
                .ToList();

            var longest = 0;
            foreach (var nextNode in nextNodes)
            {
                var newPath = new List<Point>(path);
                newPath.Add(nextNode);
                var l = GetLongestPathPart1(grid, newPath, target);
                if (l > longest)
                    longest = l;
            }
            return longest;
        }
    }
}
