using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day06 : ISolvable
    {
        private class Map
        {
            public long Source { get; set; }
            public long Target { get; set; }
            public long Length { get; set; }
        }

        private class Range
        {
            public long From { get; set; }
            public long To { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            Regex timesRegex = new Regex(@"Time: (?<times>[\d ]+)");
            Match timesMatch = timesRegex.Match(lines[0]);
            List<long> totalTimes = timesMatch.Groups["times"].Value.Split(" ").Where(s => s.Count() > 0).Select(long.Parse).ToList();

            Regex distancesRegex = new Regex(@"Distance: (?<distances>[\d ]+)");
            Match distancesMatch = distancesRegex.Match(lines[1]);
            List<long> distancesToBeat = distancesMatch.Groups["distances"].Value.Split(" ").Where(s => s.Count() > 0).Select(long.Parse).ToList();

            long wayToBeats = 1;

            for (int i = 0; i < distancesToBeat.Count; i++)
            {
                int wayToBeat = CalculateWaysToBeat(totalTimes[i], distancesToBeat[i]);
                wayToBeats *= wayToBeat;
            }

            Console.WriteLine($"Part 1: {wayToBeats}");

            Regex timeRegex = new Regex(@"Time:(?<time>[\d]+)");
            Match timeMatch = timeRegex.Match(lines[0].Replace(" ", ""));
            long totalTime = long.Parse(timeMatch.Groups["time"].Value);

            Regex distanceRegex = new Regex(@"Distance:(?<distance>[\d]+)");
            Match distanceMatch = distanceRegex.Match(lines[1].Replace(" ", ""));
            long distanceToBeat = long.Parse(distanceMatch.Groups["distance"].Value);


            int wayToBeat2 = CalculateWaysToBeat(totalTime, distanceToBeat);

            Console.WriteLine($"Part 2: {wayToBeat2}");
        }

        private static int CalculateWaysToBeat(long totalTime, long distanceToBeat)
        {
            List<long> distances = new List<long>();

            for (int pushingTime = 0; pushingTime <= totalTime; pushingTime++)
            {
                distances.Add(pushingTime * (totalTime - pushingTime));
            }

            var wayToBeat = distances.Where(d => d > distanceToBeat).Count();
            return wayToBeat;
        }
    }
}
