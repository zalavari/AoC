using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day14 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var map = lines
                .Select(line => line.ToCharArray())
                .ToArray();

            var part2 = new List<int>();

            var cycleDictionary = new Dictionary<string, int>();

            int turn = 0;
            int targetTurn = 1000000000;
            while (turn < targetTurn)
            {
                MoveWeightsNorth(map);
                MoveWeightsWest(map);
                MoveWeightsSouth(map);
                MoveWeightsEast(map);

                var text = string.Join("", map.Select(line => new string(line)));
                if (cycleDictionary.TryGetValue(text, out var turnNumber))
                {
                    Console.WriteLine($"Cycle found: {turnNumber}-{turn}");
                    turn = targetTurn - ((targetTurn - turnNumber) % (turn - turnNumber)) + 1;

                    Console.WriteLine($"Skipped to: {turn}");
                    break;
                }
                else
                {
                    cycleDictionary.Add(text, turn);
                    turn++;
                }
            }

            while (turn < targetTurn)
            {
                MoveWeightsNorth(map);
                MoveWeightsWest(map);
                MoveWeightsSouth(map);
                MoveWeightsEast(map);
                turn++;
            }



            var part1 = CountWeights(map);

            Console.WriteLine($"{part1}");
        }


        private int CountWeights(char[][] map)
        {
            int sum = 0;
            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    if (map[row][col] == 'O')
                    {
                        sum += map.Length - row;
                    }
                }
            }
            return sum;
        }

        private void MoveWeightsNorth(char[][] map)
        {
            var changed = true;
            while (changed)
            {
                changed = false;
                for (int row = 1; row < map.Length; row++)
                {
                    for (int col = 0; col < map[row].Length; col++)
                    {
                        if (map[row][col] == 'O' && map[row - 1][col] == '.')
                        {
                            map[row][col] = '.';
                            map[row - 1][col] = 'O';
                            changed = true;
                        }
                    }
                }
            }
        }

        private void MoveWeightsWest(char[][] map)
        {
            var changed = true;
            while (changed)
            {
                changed = false;
                for (int row = 0; row < map.Length; row++)
                {
                    for (int col = 1; col < map[row].Length; col++)
                    {
                        if (map[row][col] == 'O' && map[row][col - 1] == '.')
                        {
                            map[row][col] = '.';
                            map[row][col - 1] = 'O';
                            changed = true;
                        }
                    }
                }
            }
        }

        private void MoveWeightsSouth(char[][] map)
        {
            var changed = true;
            while (changed)
            {
                changed = false;
                for (int row = map.Length - 2; row >= 0; row--)
                {
                    for (int col = 0; col < map[row].Length; col++)
                    {
                        if (map[row][col] == 'O' && map[row + 1][col] == '.')
                        {
                            map[row][col] = '.';
                            map[row + 1][col] = 'O';
                            changed = true;
                        }
                    }
                }
            }
        }

        private void MoveWeightsEast(char[][] map)
        {
            var changed = true;
            while (changed)
            {
                changed = false;
                for (int row = 0; row < map.Length; row++)
                {
                    for (int col = map[row].Length - 2; col >= 0; col--)
                    {
                        if (map[row][col] == 'O' && map[row][col + 1] == '.')
                        {
                            map[row][col] = '.';
                            map[row][col + 1] = 'O';
                            changed = true;
                        }
                    }
                }
            }
        }

        private void PrintMap(char[][] map)
        {
            for (int row = 0; row < map.Length; row++)
            {
                for (int col = 0; col < map[row].Length; col++)
                {
                    Console.Write(map[row][col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
