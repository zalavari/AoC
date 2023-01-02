using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{
    internal class Day09 : ISolvable
    {
        private class ChainLink
        {
            public int X, Y;
        }

        public void Solve(string path)
        {
            ChainLink head = new ChainLink();
            ChainLink tail = new ChainLink();

            var chain = new List<ChainLink>();
            for (int i = 0; i < 10; i++)
            {
                chain.Add(new ChainLink());
            }

            var lines = File.ReadAllLines(path).ToList();

            var commands = lines.Select(x => x.Split(" ")).ToList();

            var tailPlaces = new HashSet<(int, int)>();
            tailPlaces.Add((tail.X, tail.Y));

            foreach (var command in commands)
            {
                var dir = command[0];
                var times = int.Parse(command[1]);

                for (int i = 0; i < times; i++)
                {
                    switch (dir)
                    {
                        case "U":
                            chain.First().Y++;
                            break;
                        case "D":
                            chain.First().Y--;
                            break;
                        case "R":
                            chain.First().X++;
                            break;
                        case "L":
                            chain.First().X--;
                            break;
                        default:
                            break;
                    }

                    for (int j = 1; j < chain.Count; j++)
                    {
                        var A = chain[j - 1];
                        var B = chain[j];
                        if (Math.Abs(A.X - B.X) > 1 || Math.Abs(A.Y - B.Y) > 1)
                        {
                            B.X += Math.Sign(A.X - B.X);
                            B.Y += Math.Sign(A.Y - B.Y);
                        }
                    }

                    tailPlaces.Add((chain.Last().X, chain.Last().Y));

                    //Console.WriteLine($"({HX},{HY}), ({TX},{TY})");
                }
            }


            Console.WriteLine(path);

            Console.WriteLine($"result: {tailPlaces.Count}");

            Console.WriteLine();
        }

    }
}
