using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    internal class Day05 : ISolvable
    {
        List<Stack<char>> stacks;
        public void Solve(string path)
        {
            var lines = File.ReadAllLines(path).ToList();

            var stackstrings = lines
                .Take(lines.FindIndex(line => line == "") - 1)
                .ToList();

            stackstrings.Reverse();

            stacks = new List<Stack<char>>();
            for (int j = 0; 4 * j + 2 < stackstrings.First().Length; j++)
            {
                stacks.Add(new Stack<char>());
            }

            foreach (var stackstring in stackstrings)
            {
                for (int j = 0; 4 * j + 2 < stackstring.Length; j++)
                {
                    var c = stackstring[4 * j + 1];
                    if (char.IsLetter(c))
                        stacks[j].Push(c);
                }
            }

            var instructionsstrings = lines
            .Skip(lines.FindIndex(line => line == "") + 1)
            .ToList();

            foreach (var instructionstring in instructionsstrings)
            {
                var instruction = instructionstring.Split(' ');
                var times = int.Parse(instruction[1]);
                var from = int.Parse(instruction[3]) - 1;
                var to = int.Parse(instruction[5]) - 1;

                for (int i = 0; i < times; i++)
                {
                    stacks[to].Push(stacks[from].Pop());
                }

                var temp = new List<char>();
                for (int i = 0; i < times; i++)
                {
                    temp.Add(stacks[to].Pop());
                }
                for (int i = 0; i < times; i++)
                {
                    stacks[to].Push(temp[i]);
                }
            }

            Console.WriteLine(path);

            foreach (var stack in stacks)
            {
                Console.Write(stack.Peek());
            }

            Console.WriteLine();
        }

    }
}
