using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day08 : ISolvable
    {
        private static long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private static long lcm(long a, long b)
        {
            return a / gcf(a, b) * b;
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            var instructions = lines[0];

            Dictionary<string, (string left, string right)> nodes = new Dictionary<string, (string left, string right)>();

            foreach (var line in lines.Skip(2))
            {
                Regex pattern = new Regex(@"(?<node>\w+) = \((?<left>\w+), (?<right>\w+)\)");
                Match match = pattern.Match(line);
                nodes.Add(match.Groups["node"].Value, (match.Groups["left"].Value, match.Groups["right"].Value));
            }

            //Part1(instructions, nodes);

            Part2(instructions, nodes);
        }

        private static void Part2(string instructions, Dictionary<string, (string left, string right)> nodes)
        {
            var steps = new List<long>();
            foreach (var start in nodes.Keys)
            {
                if (start.Last() != 'A')
                    continue;

                var node = start;
                var step = 0L;

                while (node.Last() != 'Z')
                {
                    var (left, right) = nodes[node];
                    if (instructions[(int)(step % instructions.Length)] == 'R')
                    {
                        node = right;
                    }
                    else
                    {
                        node = left;
                    }
                    step++;
                }

                steps.Add(step);
            }

            var result = steps.Aggregate(lcm);
            Console.WriteLine("Part2: " + result);
        }

        private static void Part1(string instructions, Dictionary<string, (string left, string right)> nodes)
        {
            var node = "AAA";
            var steps = 0;

            while (node != "ZZZ")
            {
                var (left, right) = nodes[node];
                if (instructions[steps % instructions.Length] == 'R')
                {
                    node = right;
                }
                else
                {
                    node = left;
                }
                steps++;
            }

            Console.WriteLine("Part 1: " + steps);
        }
    }
}
