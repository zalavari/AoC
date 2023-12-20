using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day09 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var lasts = new List<long>();
            var firsts = new List<long>();

            foreach (var line in lines)
            {
                var list = line.Split(' ').Select(long.Parse).ToList();

                var allLists = new List<List<long>>
                {
                    list
                };

                var i = 0;
                while (allLists[i].Any(x => x != 0))
                {
                    var newList = new List<long>();
                    for (int j = 0; j < allLists[i].Count - 1; j++)
                    {
                        newList.Add(allLists[i][j + 1] - allLists[i][j]);
                    }
                    allLists.Add(newList);
                    i++;
                }

                while (i > 0)
                {
                    var diff = allLists[i].Last();
                    allLists[i - 1].Add(allLists[i - 1].Last() + diff);

                    diff = allLists[i].First();
                    allLists[i - 1].Insert(0, allLists[i - 1].First() - diff);
                    i--;
                }

                lasts.Add(allLists[0].Last());
                firsts.Add(allLists[0].First());
            }

            Console.WriteLine($"Part1: {lasts.Sum()}");
            Console.WriteLine($"Part2: {firsts.Sum()}");
            Console.WriteLine();
        }
    }
}
