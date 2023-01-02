using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    internal class Day14 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);

            // Parse file and init variables

            var lines = File.ReadAllLines(path)
                .Select(line => line.Split(" -> ").Select(a => a.Split(",").Select(int.Parse).ToList()).ToList());

            //var maxX = lines.Max(line => line.Max(p => p[0]));
            var maxX = 1000;
            var maxY = lines.Max(line => line.Max(p => p[1]));

            var map = new List<List<int>>();
            for (int y = 0; y <= maxY; y++)
            {
                map.Add(new List<int>());
                for (int x = 0; x <= maxX; x++)
                {
                    map[y].Add(0);
                }
            }

            map.Add(new List<int>());
            for (int x = 0; x <= maxX; x++)
            {
                map.Last().Add(0);
            }
            map.Add(new List<int>());
            for (int x = 0; x <= maxX; x++)
            {
                map.Last().Add(1);
            }

            //init map
            foreach (var line in lines)
            {
                for (int i = 1; i < line.Count(); i++)
                {
                    var fromX = line[i - 1][0];
                    var fromY = line[i - 1][1];
                    var toX = line[i][0];
                    var toY = line[i][1];

                    if (fromX > toX)
                    {
                        var t = toX;
                        toX = fromX;
                        fromX = t;
                    }

                    if (fromY > toY)
                    {
                        var t = toY;
                        toY = fromY;
                        fromY = t;
                    }

                    for (int x = fromX; x <= toX; x++)
                    {
                        for (int y = fromY; y <= toY; y++)
                        {
                            map[y][x] = 1;
                        }
                    }
                }
            }

            var SX = 500;
            var SY = 0;

            var countSand = 0;
            var fallIntoVoid = false;

            while (!fallIntoVoid)
            {
                var x = SX;
                var y = SY;

                //if cant spawn new sand
                if (map[y][x] != 0)
                {
                    break;
                }

                while (true)
                {
                    //down?
                    /*if (y == maxY)
                    {
                        fallIntoVoid = true;
                        break;
                    }*/

                    if (map[y + 1][x] == 0)
                    {
                        y++;
                        continue;
                    }

                    //down-left?
                    if (map[y + 1][x - 1] == 0)
                    {
                        x--;
                        y++;
                        continue;
                    }

                    //down-right?
                    if (map[y + 1][x + 1] == 0)
                    {
                        x++;
                        y++;
                        continue;
                    }

                    //blocked
                    map[y][x] = 2;
                    countSand++;
                    break;
                }

            }

            File.WriteAllText(@$"C:\Users\marton.zalavari\Documents\AoC\map.txt", PrintMap(map));

            Console.WriteLine();
            Console.WriteLine($"resultA: {countSand}");
            Console.WriteLine($"resultB: {0}");
            Console.WriteLine();
        }

        string PrintMap(List<List<int>> map)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in map)
            {
                foreach (var n in line)
                {
                    sb.Append(n == 0 ? " " : n == 1 ? "#" : "o");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
