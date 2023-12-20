using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day04 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var winsPart1 = new List<int>();
            var winsPart2 = new List<int>();
            var copies = Enumerable.Repeat(1, lines.Length).ToList();

            for (int i = 0; i < lines.Length; i++)
            {
                var winPart1 = 1;
                var winPart2 = 0;
                var line = lines[i];
                Regex pattern = new Regex(@"Card( +)(?<id>\d+): (?<winningNumbers>[\d ]+) \| (?<ourNumbers>[\d ]+)");
                Match match = pattern.Match(line);

                List<int> winningNumbers = match.Groups["winningNumbers"].Value.Split(" ").Where(s => s.Count() > 0).Select(int.Parse).ToList();
                List<int> ourNumbers = match.Groups["ourNumbers"].Value.Split(" ").Where(s => s.Count() > 0).Select(int.Parse).ToList();

                foreach (var number in ourNumbers)
                {
                    if (winningNumbers.Contains(number))
                    {
                        winPart1 *= 2;
                        winPart2++;
                    }
                }

                for (int j = i + 1; j <= i + winPart2 && j < lines.Length; j++)
                {
                    copies[j] += copies[i];
                }

                winsPart1.Add(winPart1 / 2);
                winsPart2.Add(winPart2 * copies[i]);
            }

            //Console.WriteLine(string.Join(", ", parts));
            Console.WriteLine(winsPart1.Sum());
            Console.WriteLine(copies.Sum());
        }

    }
}
