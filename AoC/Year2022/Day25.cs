using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{

    internal class Day25 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);


            var lines = File.ReadAllLines(path);

            long sum = 0;

            foreach (var line in lines)
            {
                sum += ParseSNAFU(line);
            }

            foreach (var line in lines)
            {
                var num = ParseSNAFU(line);
                var snafu = ConvertToSNAFU(num);
                if (line != snafu)
                {
                    Console.WriteLine($"These are different: {line} -> {num} -> {snafu}");
                }

            }



            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine($"Sum in SNAFU: {ConvertToSNAFU(sum)}");
            Console.WriteLine();
        }

        private string ConvertToSNAFU(long number)
        {
            var sb = new StringBuilder();
            var map = "012=-";

            while (number > 0)
            {
                int digit = (int)(number % 5);
                sb.Append(map[digit]);
                if (digit > 2)
                {
                    number += 5;
                }
                number /= 5;
            }

            char[] array = sb.ToString().ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        private long ParseSNAFU(string snafu)
        {
            long result = 0;
            for (int i = 0; i < snafu.Count(); i++)
            {
                result *= 5;
                switch (snafu[i])
                {
                    case '=':
                        result += -2;
                        break;
                    case '-':
                        result += -1;
                        break;
                    case '0':
                        result += 0;
                        break;
                    case '1':
                        result += 1;
                        break;
                    case '2':
                        result += 2;
                        break;
                    default:
                        throw new InvalidOperationException();
                }

            }

            return result;
        }



    }
}