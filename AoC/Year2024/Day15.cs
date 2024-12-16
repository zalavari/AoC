using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day15 : ISolvable
    {
        private static Dictionary<char, (int x, int y)> directions = new Dictionary<char, (int x, int y)>()
        {
            { '^', ( 0, -1) },
            { 'v', ( 0,  1) },
            { '>', ( 1,  0) },
            { '<', (-1,  0) },
        };

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();
            var separatorIndex = lines.IndexOf("");

            var mapLines = lines.Take(separatorIndex).ToList();
            var instructionLines = lines.Skip(separatorIndex + 1).ToList();

            var map = mapLines.Select(line => line.ToList()).ToList();
            var instructions = string.Join("", instructionLines);

            var mapLinesWide = mapLines.Select(line => line.Replace(".", "..").Replace("#", "##").Replace("O", "[]").Replace("@", "@.")).ToList();
            var mapWide = mapLinesWide.Select(line => line.ToList()).ToList();

            // Second solution works for both parts
            // Part1(map, instructions);
            Part2(map, instructions);
            Part2(mapWide, instructions);

        }

        private static void Part2(List<List<char>> map, string instructions)
        {
            var positionX = 0;
            var positionY = 0;

            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x] == '@')
                    {
                        positionX = x;
                        positionY = y;
                    }
                }
            }

            for (int i = 0; i < instructions.Length; i++)
            {
                var instruction = instructions[i];

                // Uncomment for visual simulation
                //Console.Clear();
                //PrintMap(map);
                //Console.WriteLine($"Next Instruction:  {instruction}");
                //Task.Delay(100).Wait();

                var mapCopy = map.Select(line => line.ToList()).ToList();

                if (CanMove(mapCopy, positionX, positionY, instruction, '@'))
                {
                    map = mapCopy;
                    map[positionY][positionX] = '.';
                    positionX += directions[instruction].x;
                    positionY += directions[instruction].y;
                }
            }

            //Console.WriteLine("Final Map:");
            //PrintMap(map);

            var solution = 0;
            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x] == '[' || map[y][x] == 'O')
                    {
                        solution += (100 * y) + x;
                    }
                }
            }

            Console.WriteLine($"Solution: {solution}");
        }

        private static bool CanMove(List<List<char>> map, int positionX, int positionY, char instruction, char item)
        {
            var direction = directions[instruction];
            var x = positionX + direction.x;
            var y = positionY + direction.y;

            if (map[y][x] == '#')
            {
                return false;
            }

            if (map[y][x] == '.')
            {
                map[y][x] = item;
                return true;
            }

            if (map[y][x] == 'O')
            {
                var canMove = CanMove(map, x, y, instruction, 'O');
                if (canMove)
                {
                    map[y][x] = item;
                    return true;
                }
                return false;
            }

            if (instruction == '<' || instruction == '>')
            {
                var canMove = CanMove(map, x, y, instruction, map[y][x]);
                if (canMove)
                {
                    map[y][x] = item;
                    return true;
                }
                return false;
            }

            if (map[y][x] == '[')
            {
                if (CanMove(map, x, y, instruction, '[') && CanMove(map, x + 1, y, instruction, ']'))
                {
                    map[y][x] = item;
                    map[y][x + 1] = '.';
                    return true;
                }
                return false;
            }

            if (map[y][x] == ']')
            {
                if (CanMove(map, x, y, instruction, ']') && CanMove(map, x - 1, y, instruction, '['))
                {
                    map[y][x] = item;
                    map[y][x - 1] = '.';
                    return true;
                }
                return false;
            }

            Console.WriteLine("There is a problem, this should be unreachable!");
            return false;
        }

        private static void Part1(List<List<char>> map, string instructions)
        {
            var positionX = 0;
            var positionY = 0;

            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x] == '@')
                    {
                        positionX = x;
                        positionY = y;
                    }
                }
            }

            foreach (var instruction in instructions)
            {
                var direction = directions[instruction];
                var x = positionX + direction.x;
                var y = positionY + direction.y;
                while (map[y][x] != '#' && map[y][x] != '.')
                {
                    x += direction.x;
                    y += direction.y;
                }

                if (map[y][x] == '#')
                {
                    continue;
                }

                while (map[y][x] != '@')
                {
                    map[y][x] = map[y - direction.y][x - direction.x];
                    y -= direction.y;
                    x -= direction.x;
                }

                map[positionY][positionX] = '.';
                positionX += direction.x;
                positionY = y + direction.y;
            }

            //Console.WriteLine("Updated Map: ");
            //PrintMap(map);

            var solution = 0;
            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x] == 'O')
                    {
                        solution += (100 * y) + x;
                    }
                }
            }

            Console.WriteLine($"Solution: {solution}");
        }

        private static void PrintMap(List<List<char>> map)
        {
            foreach (var line in map)
            {
                Console.WriteLine(string.Join("", line));
            }
        }

    }
}
