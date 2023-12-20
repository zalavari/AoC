using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day19 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            Part2(lines);
        }

        private class MachinePart
        {
            public long x { get; set; }
            public long m { get; set; }
            public long a { get; set; }
            public long s { get; set; }
        }


        private static void Part1(string[] lines)
        {
            var splitIndex = lines.ToList().FindIndex(s => s == "");

            var workflows = new Dictionary<string, Func<MachinePart, bool>>
            {
                { "A", (part) => true },
                { "R", (part) => false }
            };

            foreach (var line in lines.Take(splitIndex))
            {
                Regex pattern = new Regex(@"(?<name>[a-zA-Z]+)\{(?<rules>.*)\}");
                Match match = pattern.Match(line);
                var name = match.Groups["name"].Value;
                var rules = match.Groups["rules"].Value.Split(",");

                Func<MachinePart, bool> lastRule = (part) => workflows[rules.Last()](part);

                var nextRules = new List<Func<MachinePart, bool>>();
                nextRules.Add(lastRule);

                for (int i = rules.Length - 2; i >= 0; i--)
                {
                    Regex rulePattern = new Regex(@"(?<property>[xmas]{1})(?<relation>[<>]{1})(?<number>[\d]+):(?<operation>[a-zAR]+)");
                    Match ruleMatch = rulePattern.Match(rules[i]);
                    var property = ruleMatch.Groups["property"].Value;
                    var relation = ruleMatch.Groups["relation"].Value;
                    var number = long.Parse(ruleMatch.Groups["number"].Value);
                    var operation = ruleMatch.Groups["operation"].Value;
                    var nextRule = nextRules.Last();

                    Func<MachinePart, bool> rule = (part) =>
                    {
                        //Console.WriteLine($"{name} - {property}{relation}{number}? - {part.x},{part.m},{part.a},{part.s}");
                        if (property == "x")
                        {
                            if (relation == "<")
                            {
                                if (part.x < number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else if (relation == ">")
                            {
                                if (part.x > number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else if (property == "m")
                        {
                            if (relation == "<")
                            {
                                if (part.m < number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else if (relation == ">")
                            {
                                if (part.m > number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else if (property == "a")
                        {
                            if (relation == "<")
                            {
                                if (part.a < number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else if (relation == ">")
                            {
                                if (part.a > number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else if (property == "s")
                        {
                            if (relation == "<")
                            {
                                if (part.s < number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else if (relation == ">")
                            {
                                if (part.s > number)
                                {
                                    return workflows[operation](part);
                                }
                                else
                                {
                                    return nextRule(part);
                                }
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid property");
                        }
                    };

                    nextRules.Add(rule);
                }

                workflows.Add(name, nextRules.Last());


                Console.WriteLine(name);


            }

            var machineParts = new List<MachinePart>();
            foreach (var line in lines.Skip(splitIndex + 1))
            {
                Regex pattern = new Regex(@"\{x=(?<x>[\d]+),m=(?<m>[\d]+),a=(?<a>[\d]+),s=(?<s>[\d]+)\}");
                Match match = pattern.Match(line);
                var x = long.Parse(match.Groups["x"].Value);
                var m = long.Parse(match.Groups["m"].Value);
                var a = long.Parse(match.Groups["a"].Value);
                var s = long.Parse(match.Groups["s"].Value);
                var machinePart = new MachinePart() { x = x, m = m, a = a, s = s };
                machineParts.Add(machinePart);
            }

            foreach (var part in machineParts)
            {
                Console.WriteLine($"{workflows["in"](part)}");
            }
            var result = machineParts.Where(part => workflows["in"](part)).Sum(part => part.x + part.m + part.a + part.s);

            Console.WriteLine($"Result: {result}");
        }

        private class MachinePartInterval
        {
            public int xMin { get; set; }
            public int xMax { get; set; }
            public int mMin { get; set; }
            public int mMax { get; set; }
            public int aMin { get; set; }
            public int aMax { get; set; }
            public int sMin { get; set; }
            public int sMax { get; set; }
        }

        private static void Part2(string[] lines)
        {
            var splitIndex = lines.ToList().FindIndex(s => s == "");

            var accepted = new List<MachinePartInterval>();

            var workflows = new Dictionary<string, Action<MachinePartInterval>>
            {
                { "A", (part) => { accepted.Add(part); } },
                { "R", (part) => { } }
            };

            foreach (var line in lines.Take(splitIndex))
            {
                Regex pattern = new Regex(@"(?<name>[a-zA-Z]+)\{(?<rules>.*)\}");
                Match match = pattern.Match(line);
                var name = match.Groups["name"].Value;
                var rules = match.Groups["rules"].Value.Split(",");

                Action<MachinePartInterval> lastRule = (part) => workflows[rules.Last()](part);

                var nextRules = new List<Action<MachinePartInterval>>();
                nextRules.Add(lastRule);

                for (int i = rules.Length - 2; i >= 0; i--)
                {
                    Regex rulePattern = new Regex(@"(?<property>[xmas]{1})(?<relation>[<>]{1})(?<number>[\d]+):(?<operation>[a-zAR]+)");
                    Match ruleMatch = rulePattern.Match(rules[i]);
                    var property = ruleMatch.Groups["property"].Value;
                    var relation = ruleMatch.Groups["relation"].Value;
                    var number = int.Parse(ruleMatch.Groups["number"].Value);
                    var operation = ruleMatch.Groups["operation"].Value;
                    var nextRule = nextRules.Last();

                    Action<MachinePartInterval> rule = (part) =>
                    {
                        if (part.sMax < part.sMin || part.aMax < part.aMin || part.mMax < part.mMin || part.xMax < part.xMin)
                        {
                            return;
                        }

                        if (property == "x")
                        {
                            if (relation == "<")
                            {
                                var part1 = new MachinePartInterval() { xMin = part.xMin, xMax = Math.Min(part.xMax, number - 1), mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = Math.Max(part.xMin, number), xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                nextRule(part2);
                            }
                            else if (relation == ">")
                            {
                                var part1 = new MachinePartInterval() { xMin = Math.Max(part.xMin, number + 1), xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = part.xMin, xMax = Math.Min(part.xMax, number), mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                nextRule(part2);
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else if (property == "m")
                        {
                            if (relation == "<")
                            {
                                var part1 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = Math.Min(part.mMax, number - 1), aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = Math.Max(part.mMin, number), mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                nextRule(part2);
                            }
                            else if (relation == ">")
                            {
                                var part1 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = Math.Max(part.mMin, number + 1), mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = Math.Min(part.mMax, number), aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                nextRule(part2);
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else if (property == "a")
                        {
                            if (relation == "<")
                            {
                                var part1 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = Math.Min(part.aMax, number - 1), sMin = part.sMin, sMax = part.sMax };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = Math.Max(part.aMin, number), aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                nextRule(part2);
                            }
                            else if (relation == ">")
                            {
                                var part1 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = Math.Max(part.aMin, number + 1), aMax = part.aMax, sMin = part.sMin, sMax = part.sMax };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = Math.Min(part.aMax, number), sMin = part.sMin, sMax = part.sMax };
                                nextRule(part2);
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else if (property == "s")
                        {
                            if (relation == "<")
                            {
                                var part1 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = Math.Min(part.sMax, number - 1) };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = Math.Max(part.sMin, number), sMax = part.sMax };
                                nextRule(part2);
                            }
                            else if (relation == ">")
                            {
                                var part1 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = Math.Max(part.sMin, number + 1), sMax = part.sMax };
                                workflows[operation](part1);

                                var part2 = new MachinePartInterval() { xMin = part.xMin, xMax = part.xMax, mMin = part.mMin, mMax = part.mMax, aMin = part.aMin, aMax = part.aMax, sMin = part.sMin, sMax = Math.Min(part.sMax, number) };
                                nextRule(part2);
                            }
                            else
                            {
                                throw new Exception("Invalid relation");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid property");
                        }
                    };

                    nextRules.Add(rule);
                }

                workflows.Add(name, nextRules.Last());


                Console.WriteLine(name);

            }

            var initialPart = new MachinePartInterval() { xMin = 1, xMax = 4000, mMin = 1, mMax = 4000, aMin = 1, aMax = 4000, sMin = 1, sMax = 4000 };

            workflows["in"](initialPart);

            long result = 0;
            foreach (var partInterval in accepted)
            {
                long x = partInterval.xMax - partInterval.xMin + 1;
                long m = partInterval.mMax - partInterval.mMin + 1;
                long a = partInterval.aMax - partInterval.aMin + 1;
                long s = partInterval.sMax - partInterval.sMin + 1;


                result += x * m * a * s;
            }

            Console.WriteLine($"Result: {result}");
        }

    }
}
