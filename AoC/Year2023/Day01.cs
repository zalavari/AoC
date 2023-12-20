using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day01 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var calibrations = new List<int>();

            var spelledOutDigits = new List<string>()
            {
                "0",
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine"
            };

            var spelledOutDigitsWithNumbers = new List<string>()
            {
                "z0ero",
                "o1ne",
                "t2wo",
                "t3hree",
                "f4our",
                "f5ive",
                "s6ix",
                "s7even",
                "e8ight",
                "n9ine"
            };

            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];
                for (int digit = 0; digit < spelledOutDigits.Count; digit++)
                {
                    line = line.Replace(spelledOutDigits[digit], spelledOutDigitsWithNumbers[digit]);
                }

                var c1 = line.First(c => char.IsDigit(c));
                var c2 = line.Last(c => char.IsDigit(c));
                calibrations.Add((10 * int.Parse(c1.ToString())) + int.Parse(c2.ToString()));
                //Console.WriteLine(line);
                //Console.WriteLine($"{c1} {c2}");

            }


            Console.WriteLine($"Sum: {calibrations.Sum()}");

            Console.WriteLine();
        }
    }
}
