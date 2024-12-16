using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day05 : ISolvable
    {

        private Dictionary<int, List<int>> edges;
        private Dictionary<int, List<int>> reverseEdges;

        private int Comparer(int a, int b)
        {
            if (edges.TryGetValue(a, out var children) && children.Contains(b))
            {
                return -1;
            }

            if (edges.TryGetValue(b, out var children2) && children2.Contains(a))
            {
                return 1;
            }

            return 0;
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path).ToList();
            var sep = lines.IndexOf("");
            var linesForEdges = lines.Take(sep);
            var ordersToCheck = lines.Skip(sep + 1).Select(x => x.Split(",").Select(int.Parse).ToList()).ToList();

            edges = new Dictionary<int, List<int>>();
            reverseEdges = new Dictionary<int, List<int>>();
            foreach (var line in linesForEdges)
            {
                var from = int.Parse(line.Split("|")[0]);
                var to = int.Parse(line.Split("|")[1]);
                if (!edges.ContainsKey(from))
                {
                    edges[from] = new List<int>();
                }
                edges[from].Add(to);

                if (!reverseEdges.ContainsKey(to))
                {
                    reverseEdges[to] = new List<int>();
                }
                reverseEdges[to].Add(from);
            }

            var solution = 0;
            var solution2 = 0;

            foreach (var order in ordersToCheck)
            {
                var correct = true;
                for (int i = 0; i < order.Count - 1 && correct; i++)
                {
                    for (int j = i + 1; j < order.Count && correct; j++)
                    {
                        if (edges.TryGetValue(order[j], out var backwards) && backwards.Any(n => n == order[i]))
                        {
                            correct = false;
                            break;
                        }
                    }
                }

                if (correct)
                {
                    solution += order[order.Count / 2];
                }
                else
                {
                    order.Sort(Comparer);
                    solution2 += order[order.Count / 2];
                }
            }

            Console.WriteLine(solution);
            Console.WriteLine(solution2);
        }
    }
}
