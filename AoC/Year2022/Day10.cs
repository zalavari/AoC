using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{
    internal class Day10 : ISolvable
    {
        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path).ToList();

            var commands = lines.Select(x => x.Split(" ")).ToList();

            var cycle = 0;
            var X = 1;
            var result = 0;
            foreach (var command in commands)
            {
                if (command[0] == "noop")
                {
                    cycle++;
                    result += AddToResult(cycle, X);
                    Render(cycle, X);

                }
                else if (command[0] == "addx")
                {
                    cycle++;
                    result += AddToResult(cycle, X);
                    Render(cycle, X);
                    cycle++;

                    result += AddToResult(cycle, X);
                    Render(cycle, X);

                    var addition = int.Parse(command[1]);
                    X += addition;



                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            Console.WriteLine();
            Console.WriteLine($"result: {result}");
            Console.WriteLine();
        }

        private int AddToResult(int cycle, int X)
        {
            if (cycle % 40 == 20 && cycle <= 220)
            {
                return cycle * X;
            }
            return 0;
        }

        private void Render(int cycle, int X)
        {
            cycle--;
            X--;

            if (cycle % 40 == 0)
                Console.WriteLine();

            var c = X <= cycle % 40 && cycle % 40 <= X + 2 ? '#' : ' ';

            Console.Write(c);
        }

    }
}
