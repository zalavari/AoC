using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day05 : ISolvable
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
            var text = File.ReadAllText(path);

            var sections = text.Split("\n\n");


            Regex pattern = new Regex(@"seeds: (?<seeds>[\d ]+)");
            Match match = pattern.Match(sections[0]);
            List<long> seeds = match.Groups["seeds"].Value.Split(" ").Where(s => s.Count() > 0).Select(long.Parse).ToList();
            List<List<Map>> maps = new List<List<Map>>();

            foreach (var section in sections.Skip(1))
            {
                maps.Add(new List<Map>());
                foreach (var line in section.Split("\n").Skip(1))
                {
                    var map = new Map();
                    var parts = line.Split(" ");
                    map.Source = long.Parse(parts[1]);
                    map.Target = long.Parse(parts[0]);
                    map.Length = long.Parse(parts[2]);
                    maps.Last().Add(map);
                }
            }

            var numbers = new List<List<long>>();
            numbers.Add(seeds);

            foreach (var map in maps)
            {
                var newNumbers = new List<long>();
                foreach (var number in numbers.Last())
                {
                    var foundMap = false;
                    foreach (var mapItem in map)
                    {
                        if (mapItem.Source <= number && number <= mapItem.Source + mapItem.Length)
                        {
                            newNumbers.Add(mapItem.Target + number - mapItem.Source);
                            foundMap = true;
                            break;
                        }
                    }
                    if (!foundMap)
                    {
                        newNumbers.Add(number);
                    }
                }
                numbers.Add(newNumbers);
            }

            Console.WriteLine($"Part 1: {numbers.Last().Min()}");

            var ranges = new List<List<Range>>();
            ranges.Add(new List<Range>());
            for (int i = 0; i < seeds.Count; i += 2)
            {
                ranges.Last().Add(new Range() { From = seeds[i], To = seeds[i] + seeds[i + 1] - 1 });
            }

            foreach (var map in maps)
            {
                ExtendWithIdenticalIfNotDefined(map);
                var newRanges = new List<Range>();
                foreach (var range in ranges.Last())
                {
                    foreach (var mapItem in map)
                    {
                        if (range.To < mapItem.Source || mapItem.Source + mapItem.Length < range.From)
                            continue;

                        var from = Math.Max(range.From, mapItem.Source);
                        var to = Math.Min(range.To, mapItem.Source + mapItem.Length);

                        newRanges.Add(new Range() { From = mapItem.Target + from - mapItem.Source, To = mapItem.Target + to - mapItem.Source });
                    }
                }
                ranges.Add(newRanges);
            }

            Console.WriteLine($"Part 1: {ranges.Last().Select(r => r.From).Min()}");
        }

        private void ExtendWithIdenticalIfNotDefined(List<Map> map)
        {
            var identicalRanges = new List<Range>();
            identicalRanges.Add(new Range() { From = 0, To = long.MaxValue / 2 });
            foreach (var mapItem in map)
            {
                var newIdenticalRanges = new List<Range>();
                foreach (var range in identicalRanges)
                {
                    if (range.To < mapItem.Source || mapItem.Source + mapItem.Length < range.From)
                    {
                        newIdenticalRanges.Add(range);
                        continue;
                    }

                    var from = Math.Max(range.From, mapItem.Source);
                    var to = Math.Min(range.To, mapItem.Source + mapItem.Length);

                    if (from > range.From)
                    {
                        newIdenticalRanges.Add(new Range() { From = range.From, To = from - 1 });
                    }

                    if (to < range.To)
                    {
                        newIdenticalRanges.Add(new Range() { From = to + 1, To = range.To });
                    }
                }
                identicalRanges = newIdenticalRanges;
            }

            map.AddRange(identicalRanges.Select(r => new Map() { Source = r.From, Target = r.From, Length = r.To - r.From + 1 }));
        }
    }
}
