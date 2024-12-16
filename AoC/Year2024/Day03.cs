using System;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC.Year2024
{
    internal class Day03 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path);
            var result1 = 0;

            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"mul\((?<number1>[0-9]{1,3}),(?<number2>[0-9]{1,3})\)");
                MatchCollection matches = pattern.Matches(line);

                foreach (Match match in matches)
                {
                    var number1 = int.Parse(match.Groups["number1"].Value);
                    var number2 = int.Parse(match.Groups["number2"].Value);
                    result1 += number1 * number2;
                }
            }


            var result2 = 0;
            var enabled = true;
            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"mul\((?<number1>[0-9]{1,3}),(?<number2>[0-9]{1,3})\)|(?<operation>don't\(\)|do\(\))");
                MatchCollection matches = pattern.Matches(line);

                foreach (Match match in matches)
                {
                    if (match.Groups["operation"].Value == "do()")
                    {
                        enabled = true;
                    }
                    else if (match.Groups["operation"].Value == "don't()")
                    {
                        enabled = false;
                    }
                    else if (enabled)
                    {
                        var number1 = int.Parse(match.Groups["number1"].Value);
                        var number2 = int.Parse(match.Groups["number2"].Value);
                        result2 += number1 * number2;
                    }
                }
            }

            Console.WriteLine(result1);
            Console.WriteLine(result2);
        }


    }
}
