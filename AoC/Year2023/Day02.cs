using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day02 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            var maxRed = 12;
            var maxGreen = 13;
            var maxBlue = 14;
            var validGames = new List<int>();
            var powers = new List<int>();

            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"Game (?<id>\d+): (?<games>(.*))");
                Match match = pattern.Match(line);
                int id = int.Parse(match.Groups["id"].Value);
                List<string> games = match.Groups["games"].Value.Split("; ").ToList();
                var valid = true;
                var minRed = 0;
                var minGreen = 0;
                var minBlue = 0;
                foreach (var game in games)
                {
                    var bluePatter = new Regex(@"(?<blueCount>\d+) blue");
                    var blueMatch = bluePatter.Match(game);
                    var blueCount = blueMatch.Groups["blueCount"].Success ? int.Parse(blueMatch.Groups["blueCount"].Value) : 0;

                    var redPatter = new Regex(@"(?<redCount>\d+) red");
                    var redMatch = redPatter.Match(game);
                    var redCount = redMatch.Groups["redCount"].Success ? int.Parse(redMatch.Groups["redCount"].Value) : 0;

                    var greenPatter = new Regex(@"(?<greenCount>\d+) green");
                    var greenMatch = greenPatter.Match(game);
                    var greenCount = greenMatch.Groups["greenCount"].Success ? int.Parse(greenMatch.Groups["greenCount"].Value) : 0;

                    if (redCount > maxRed || greenCount > maxGreen || blueCount > maxBlue)
                    {
                        valid = false;
                    }
                    if (redCount > minRed)
                    {
                        minRed = redCount;
                    }
                    if (greenCount > minGreen)
                    {
                        minGreen = greenCount;
                    }
                    if (blueCount > minBlue)
                    {
                        minBlue = blueCount;
                    }
                }
                if (valid)
                {
                    validGames.Add(id);
                }
                powers.Add(minRed * minGreen * minBlue);

            }
            Console.WriteLine($"Sum of valid game ids: {validGames.Sum()}");
            Console.WriteLine($"Sum of powers of games: {powers.Sum()}");
        }
    }
}
