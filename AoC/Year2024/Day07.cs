using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day07 : ISolvable
    {


        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path).ToList();
            var solution1 = 0L;

            foreach (var line in lines)
            {
                var temp = line.Split(": ");
                var target = long.Parse(temp[0]);
                var numbers = temp[1].Split(" ").Select(long.Parse).ToList();

                if (IsValid(target, numbers))
                {
                    solution1 += target;
                }
                else
                {
                }
            }

            Console.WriteLine($"Solution 1: {solution1}");
        }

        private bool IsValid(long target, List<long> numbers)
        {
            if (numbers.Count == 0)
            {
                return target == 0;
            }

            if (target < 0)
            {
                return false;
            }

            var number = numbers.Last();
            var newNumbers = new List<long>(numbers);
            newNumbers.RemoveAt(newNumbers.Count - 1);

            if (IsValid(target - number, newNumbers))
            {
                return true;
            }

            if (target % number == 0 && IsValid(target / number, newNumbers))
            {
                return true;
            }

            // For second part only
            target -= number;
            while (number > 0 && target % 10 == 0)
            {
                target /= 10;
                number /= 10;
            }

            if (number == 0 && IsValid(target, newNumbers))
            {
                return true;
            }


            return false;
        }
    }
}
