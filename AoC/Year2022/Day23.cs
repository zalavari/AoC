using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{

    internal class Day23 : ISolvable
    {


        public void Solve(string path)
        {
            Console.WriteLine(path);

            //var elves = new List<(int, int)>();
            var elves = new HashSet<(int, int)>();

            var lines = File.ReadAllLines(path);

            var Height = lines.Length;
            var Width = lines.First().Length;

            for (int i = 0; i < Height; i++)
            {
                var line = lines[i];
                for (int j = 0; j < Width; j++)
                {
                    var x = j;
                    var y = i;
                    if (line[j] == '#')
                        elves.Add((x, y));
                }
            }

            var directions = new List<char>() { 'N', 'S', 'W', 'E' };

            var noMoveCount = 0;
            var round = 0;
            for (round = 0; noMoveCount < elves.Count(); round++)
            {
                //Console.WriteLine($"Round: {round}, NoMoveCount: {noMoveCount}");
                noMoveCount = 0;
                var proposals = new Dictionary<(int, int), List<(int, int)>>();
                foreach (var (x, y) in elves)
                {
                    int d = 0;
                    bool N = elves.Contains((x, y - 1));
                    bool S = elves.Contains((x, y + 1));
                    bool W = elves.Contains((x - 1, y));
                    bool E = elves.Contains((x + 1, y));
                    bool NW = elves.Contains((x - 1, y - 1));
                    bool NE = elves.Contains((x + 1, y - 1));
                    bool SW = elves.Contains((x - 1, y + 1));
                    bool SE = elves.Contains((x + 1, y + 1));

                    if (N || S || W || E || NW || NE || SW || SE)
                    {
                        for (d = 0; d < directions.Count(); d++)
                        {

                            if (directions[d] == 'N')
                            {
                                if (!elves.Contains((x - 1, y - 1)) && !elves.Contains((x, y - 1)) && !elves.Contains((x + 1, y - 1)))
                                {
                                    var s = proposals.TryAdd((x, y - 1), new List<(int, int)>() { (x, y) });
                                    if (!s)
                                        proposals[(x, y - 1)].Add((x, y));

                                    break;
                                }
                            }
                            else if (directions[d] == 'S')
                            {
                                if (!elves.Contains((x - 1, y + 1)) && !elves.Contains((x, y + 1)) && !elves.Contains((x + 1, y + 1)))
                                {
                                    var s = proposals.TryAdd((x, y + 1), new List<(int, int)>() { (x, y) });
                                    if (!s)
                                        proposals[(x, y + 1)].Add((x, y));
                                    break;
                                }
                            }
                            else if (directions[d] == 'W')
                            {
                                if (!elves.Contains((x - 1, y - 1)) && !elves.Contains((x - 1, y)) && !elves.Contains((x - 1, y + 1)))
                                {
                                    var s = proposals.TryAdd((x - 1, y), new List<(int, int)>() { (x, y) });
                                    if (!s)
                                        proposals[(x - 1, y)].Add((x, y));
                                    break;
                                }
                            }
                            else if (directions[d] == 'E')
                            {
                                if (!elves.Contains((x + 1, y - 1)) && !elves.Contains((x + 1, y)) && !elves.Contains((x + 1, y + 1)))
                                {
                                    var s = proposals.TryAdd((x + 1, y), new List<(int, int)>() { (x, y) });
                                    if (!s)
                                        proposals[(x + 1, y)].Add((x, y));
                                    break;
                                }
                            }
                        }
                        if (d == 4)
                        {
                            var s = proposals.TryAdd((x, y), new List<(int, int)>() { (x, y) });
                            if (!s)
                                proposals[(x, y)].Add((x, y));
                        }
                    }
                    else
                    {
                        noMoveCount++;
                        var s = proposals.TryAdd((x, y), new List<(int, int)>() { (x, y) });
                        if (!s)
                            proposals[(x, y)].Add((x, y));
                    }
                }

                var newElves = new HashSet<(int, int)>();
                foreach (var ((x, y), list) in proposals)
                {
                    if (list.Count == 1)
                    {
                        newElves.Add((x, y));
                    }
                    else
                    {
                        foreach (var (x0, y0) in list)
                        {
                            newElves.Add((x0, y0));
                        }
                    }
                }

                elves = newElves;
                directions.Add(directions.First());
                directions = directions.Skip(1).ToList();



                //PrintElves(elves);
            }


            var minX = elves.Min(xy => xy.Item1);
            var minY = elves.Min(xy => xy.Item2);
            var maxX = elves.Max(xy => xy.Item1);
            var maxY = elves.Max(xy => xy.Item2);

            Height = maxY - minY + 1;
            Width = maxX - minX + 1;

            Console.WriteLine($"result: {Height * Width - elves.Count()}");
            Console.WriteLine($"rounds: {round}");
            Console.WriteLine();
        }

        private void PrintElves(HashSet<(int, int)> elves)
        {
            var minX = elves.Min(xy => xy.Item1);
            var minY = elves.Min(xy => xy.Item2);
            var maxX = elves.Max(xy => xy.Item1);
            var maxY = elves.Max(xy => xy.Item2);


            minX = 0;
            minY = 0;
            maxX = 8;
            maxX = 8;


            Console.WriteLine("-------------");
            foreach (var (x, y) in elves)
            {
                Console.WriteLine($"({x},{y})");
            }
            Console.WriteLine("-------------");
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var c = elves.Contains((x, y)) ? '#' : '.';
                    Console.Write(c);
                }

                Console.WriteLine();
            }
            Console.WriteLine("-------------");
            Console.WriteLine();
        }

    }
}