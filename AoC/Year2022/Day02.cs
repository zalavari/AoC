using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    internal class Day02 : ISolvable
    {

        public void Solve(string path)
        {
            var lines = File.ReadAllLines(path);
            var rounds = lines.Select(line => line.Split(" ")).ToList();

            var sumA = 0;
            var sumB = 0;
            foreach (var round in rounds)
            {
                var r = 0;
                if (round[0] == "A" && round[1] == "Y" || round[0] == "B" && round[1] == "Z" || round[0] == "C" && round[1] == "X")
                {
                    r += 6;
                }
                if (round[0] == "A" && round[1] == "X" || round[0] == "B" && round[1] == "Y" || round[0] == "C" && round[1] == "Z")
                {
                    r += 3;
                }
                if (round[1] == "X")
                {
                    r += 1;
                }
                if (round[1] == "Y")
                {
                    r += 2;
                }
                if (round[1] == "Z")
                {
                    r += 3;
                }
                sumA += r;


                r = 0;
                if (round[1] == "X")
                {
                    r += 0;
                    if (round[0] == "A")
                    {
                        r += 3;
                    }
                    if (round[0] == "B")
                    {
                        r += 1;
                    }
                    if (round[0] == "C")
                    {
                        r += 2;
                    }

                }
                if (round[1] == "Y")
                {
                    r += 3;
                    if (round[0] == "A")
                    {
                        r += 1;
                    }
                    if (round[0] == "B")
                    {
                        r += 2;
                    }
                    if (round[0] == "C")
                    {
                        r += 3;
                    }
                }
                if (round[1] == "Z")
                {
                    r += 6;
                    if (round[0] == "A")
                    {
                        r += 2;
                    }
                    if (round[0] == "B")
                    {
                        r += 3;
                    }
                    if (round[0] == "C")
                    {
                        r += 1;
                    }
                }
                sumB += r;
            }


            Console.WriteLine(path);
            Console.WriteLine($"SumA: {sumA}");
            Console.WriteLine($"SumB: {sumB}");

            Console.WriteLine();
        }
    }
}
