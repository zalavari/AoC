using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day18 : ISolvable
    {
        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        private class Instruction
        {
            public long X { get; set; }
            public long Y { get; set; }
            public Direction Direction { get; set; }
            public long Steps { get; set; }
        }

        private class Cell
        {
            public bool IsOutside { get; set; }
            public bool IsTrench { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            Part1(lines);

            Part2(lines);
        }

        private static void Part2(string[] lines)
        {
            // Parse instructions
            var instructions = new List<Instruction>();
            instructions.Add(new Instruction { X = 0, Y = 0, Direction = Direction.Up, Steps = 0 });

            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"(?<direction>[UDLR]+) (?<steps>[\d]+) \(#(?<stepsInHex>[0-9a-f]+)(?<direction2>[0-9]{1})\)");
                Match match = pattern.Match(line);
                var stepsInHex = match.Groups["stepsInHex"].Value;
                var directionInt = int.Parse(match.Groups["direction2"].Value);

                long steps = Convert.ToInt64(stepsInHex, 16);
                switch (directionInt)
                {
                    case 3:
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X,
                            Y = instructions[instructions.Count - 1].Y + steps,
                            Direction = Direction.Up,
                            Steps = steps,
                        });
                        break;
                    case 1:
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X,
                            Y = instructions[instructions.Count - 1].Y - steps,
                            Direction = Direction.Down,
                            Steps = steps,
                        });
                        break;
                    case 2:
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X - steps,
                            Y = instructions[instructions.Count - 1].Y,
                            Direction = Direction.Left,
                            Steps = steps,
                        });
                        break;
                    case 0:
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X + steps,
                            Y = instructions[instructions.Count - 1].Y,
                            Direction = Direction.Right,
                            Steps = steps,
                        });
                        break;
                }
            }

            var circumference = instructions.Sum(i => i.Steps);

            var sum = 0L;
            for (int i = 0; i < instructions.Count - 1; i++)
            {
                var x1 = instructions[i].X;
                var y1 = instructions[i].Y;
                var x2 = instructions[i + 1].X;
                var y2 = instructions[i + 1].Y;
                sum += (x1 * y2) - (x2 * y1);
            }

            var area = Math.Abs(sum) / 2;

            //Console.WriteLine($"Circumference: {circumference}");
            //Console.WriteLine($"Area: {area}");
            Console.WriteLine($"Inside2: {area + (circumference / 2) + 1}");
        }

        private static void Part1(string[] lines)
        {

            // Parse instructions
            var instructions = new List<Instruction>();
            instructions.Add(new Instruction { X = 0, Y = 0, Direction = Direction.Up, Steps = 0 });

            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"(?<direction>[UDLR]+) (?<steps>[\d]+) \((?<color>#[0-9a-f]+)\)");
                Match match = pattern.Match(line);
                var directionString = match.Groups["direction"].Value;
                var steps = int.Parse(match.Groups["steps"].Value);
                var color = match.Groups["color"].Value;
                switch (directionString)
                {
                    case "U":
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X,
                            Y = instructions[instructions.Count - 1].Y + steps,
                            Direction = Direction.Up,
                            Steps = steps,
                        });
                        break;
                    case "D":
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X,
                            Y = instructions[instructions.Count - 1].Y - steps,
                            Direction = Direction.Down,
                            Steps = steps,
                        });
                        break;
                    case "L":
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X - steps,
                            Y = instructions[instructions.Count - 1].Y,
                            Direction = Direction.Left,
                            Steps = steps,
                        });
                        break;
                    case "R":
                        instructions.Add(new Instruction
                        {
                            X = instructions[instructions.Count - 1].X + steps,
                            Y = instructions[instructions.Count - 1].Y,
                            Direction = Direction.Right,
                            Steps = steps,
                        });
                        break;
                }
            }


            var circumference = instructions.Sum(i => i.Steps);

            var sum = 0L;
            for (int i = 0; i < instructions.Count - 1; i++)
            {
                var x1 = instructions[i].X;
                var y1 = instructions[i].Y;
                var x2 = instructions[i + 1].X;
                var y2 = instructions[i + 1].Y;
                sum += (x1 * y2) - (x2 * y1);
            }

            var area = Math.Abs(sum) / 2;

            //Console.WriteLine($"Circumference: {circumference}");
            //Console.WriteLine($"Area: {area}");
            Console.WriteLine($"Inside1: {area + (circumference / 2) + 1}");

            // Build grid
            // Add one row/column padding and using offset
            var offsetX = -instructions.Min(i => i.X) + 1;
            var offsetY = -instructions.Min(i => i.Y) + 1;
            var W = instructions.Max(i => i.X) + offsetX + 2;
            var H = instructions.Max(i => i.Y) + offsetY + 2;
            //Console.WriteLine($"Dimensions: W={W}, H={H}, offsetX={offsetX}, offsetY={offsetY}");

            instructions.RemoveAt(0);
            var grid = new Cell[W, H];
            for (int i = 0; i < W; i++)
                for (int j = 0; j < H; j++)
                    grid[i, j] = new Cell();

            var x = 0;
            var y = 0;
            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Steps; i++)
                {
                    switch (instruction.Direction)
                    {
                        case Direction.Up:
                            y++;
                            break;
                        case Direction.Down:
                            y--;
                            break;
                        case Direction.Left:
                            x--;
                            break;
                        case Direction.Right:
                            x++;
                            break;
                    }
                    grid[x + offsetX, y + offsetY] = new Cell()
                    {
                        IsTrench = true,
                    };
                }
            }

            //PrintGrid(grid);

            var queue = new Queue<Point>();
            queue.Enqueue(new Point(0, 0));
            grid[0, 0].IsOutside = true;

            while (queue.Any())
            {
                var p = queue.Dequeue();

                var nextPoints = new List<Point>
                {
                    new Point(p.X + 1, p.Y),
                    new Point(p.X - 1, p.Y),
                    new Point(p.X, p.Y + 1),
                    new Point(p.X, p.Y - 1)
                };

                foreach (var nextPoint in nextPoints)
                {
                    if (nextPoint.X < 0 || nextPoint.Y < 0 ||
                        nextPoint.X >= W || nextPoint.Y >= H)
                        continue;

                    var nextCell = grid[nextPoint.X, nextPoint.Y];

                    if (!nextCell.IsOutside && !nextCell.IsTrench)
                    {
                        nextCell.IsOutside = true;
                        queue.Enqueue(nextPoint);
                    }
                }
            }


            //PrintGrid(grid);

            var inside = 0;
            for (int i = 1; i < W - 1; i++)
                for (int j = 1; j < H - 1; j++)
                    inside += grid[i, j].IsOutside ? 0 : 1;

            Console.WriteLine($"Inside lagoon: {inside}");
        }

        private static void PrintGrid(Cell[,] grid)
        {
            for (var y = grid.GetLength(1) - 1; y >= 0; y--)
            {
                for (var x = grid.GetLength(0) - 1; x >= 0; x--)
                {
                    var cell = grid[x, y];
                    if (cell.IsOutside)
                        Console.ForegroundColor = ConsoleColor.Blue;

                    if (cell.IsTrench)
                        Console.Write('#');
                    else
                        Console.Write('.');


                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();

            }
        }
    }
}
