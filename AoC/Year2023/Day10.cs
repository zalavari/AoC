using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day10 : ISolvable
    {

        private class Field
        {
            public char Pipe { get; set; }
            public bool IsPartOfPipeline { get; set; } = false;
        }

        private class Point
        {
            public int Row { get; set; }
            public int Col { get; set; }
        }

        private enum Direction
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            var map = lines.Select(line => line.Select(c => new Field() { Pipe = c }).ToList()).ToList();

            // Calculate starting position
            var start = new Point();
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j].Pipe == 'S')
                    {
                        start.Row = i;
                        start.Col = j;
                    }
                }
            }

            // Determine directions
            var directions = new List<Direction>();
            if (start.Row > 0 && (map[start.Row - 1][start.Col].Pipe == 'F' || map[start.Row - 1][start.Col].Pipe == '|' || map[start.Row - 1][start.Col].Pipe == '7'))
            {
                directions.Add(Direction.Up);
            }
            if (start.Row < map.Count - 1 && (map[start.Row + 1][start.Col].Pipe == 'L' || map[start.Row + 1][start.Col].Pipe == '|' || map[start.Row + 1][start.Col].Pipe == 'J'))
            {
                directions.Add(Direction.Down);
            }
            if (start.Col > 0 && (map[start.Row][start.Col - 1].Pipe == 'F' || map[start.Row][start.Col - 1].Pipe == '-' || map[start.Row][start.Col - 1].Pipe == 'L'))
            {
                directions.Add(Direction.Left);
            }
            if (start.Col < map[start.Row].Count - 1 && (map[start.Row][start.Col + 1].Pipe == 'J' || map[start.Row][start.Col + 1].Pipe == '-' || map[start.Row][start.Col + 1].Pipe == '7'))
            {
                directions.Add(Direction.Right);
            }

            if (directions.Count != 2)
            {
                throw new InvalidOperationException("The type of the part 'S' is not easily identifiable");
            }

            var direction = directions.First();

            // Replace 'S' with its corresponding pipe
            // Only used for the second part
            if (directions.Contains(Direction.Up) && directions.Contains(Direction.Right))
            {
                map[start.Row][start.Col].Pipe = 'L';
            }
            else if (directions.Contains(Direction.Up) && directions.Contains(Direction.Left))
            {
                map[start.Row][start.Col].Pipe = 'J';
            }
            else if (directions.Contains(Direction.Down) && directions.Contains(Direction.Right))
            {
                map[start.Row][start.Col].Pipe = 'F';
            }
            else if (directions.Contains(Direction.Down) && directions.Contains(Direction.Left))
            {
                map[start.Row][start.Col].Pipe = '7';
            }
            else
            {
                throw new InvalidOperationException("The type of the part 'S' is not easily identifiable");
            }

            // Follow the path
            var pipeline = new List<Point>() { start };

            while (true)
            {
                var current = pipeline.Last();
                var next = new Point();

                map[current.Row][current.Col].IsPartOfPipeline = true;

                switch (direction)
                {
                    case Direction.Up:
                        next.Row = current.Row - 1;
                        next.Col = current.Col;
                        break;
                    case Direction.Down:
                        next.Row = current.Row + 1;
                        next.Col = current.Col;
                        break;
                    case Direction.Left:
                        next.Row = current.Row;
                        next.Col = current.Col - 1;
                        break;
                    case Direction.Right:
                        next.Row = current.Row;
                        next.Col = current.Col + 1;
                        break;
                }

                pipeline.Add(next);

                if (next.Row == start.Row && next.Col == start.Col)
                {
                    break;
                }

                switch (direction)
                {
                    case Direction.Up:
                        if (map[next.Row][next.Col].Pipe == 'F')
                        {
                            direction = Direction.Right;
                        }
                        else if (map[next.Row][next.Col].Pipe == '7')
                        {
                            direction = Direction.Left;
                        }
                        else if (map[next.Row][next.Col].Pipe == '|')
                        {
                            direction = Direction.Up;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                        break;
                    case Direction.Down:
                        if (map[next.Row][next.Col].Pipe == 'L')
                        {
                            direction = Direction.Right;
                        }
                        else if (map[next.Row][next.Col].Pipe == 'J')
                        {
                            direction = Direction.Left;
                        }
                        else if (map[next.Row][next.Col].Pipe == '|')
                        {
                            direction = Direction.Down;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                        break;
                    case Direction.Left:
                        if (map[next.Row][next.Col].Pipe == 'F')
                        {
                            direction = Direction.Down;
                        }
                        else if (map[next.Row][next.Col].Pipe == 'L')
                        {
                            direction = Direction.Up;
                        }
                        else if (map[next.Row][next.Col].Pipe == '-')
                        {
                            direction = Direction.Left;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                        break;
                    case Direction.Right:
                        if (map[next.Row][next.Col].Pipe == 'J')
                        {
                            direction = Direction.Up;
                        }
                        else if (map[next.Row][next.Col].Pipe == '7')
                        {
                            direction = Direction.Down;
                        }
                        else if (map[next.Row][next.Col].Pipe == '-')
                        {
                            direction = Direction.Right;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                        break;
                    default:
                        break;
                }
            }



            var countOfInsideFields = 0;
            for (int i = 0; i < map.Count; i++)
            {
                var inside = false;
                var segmentStartedWith = ' ';
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j].IsPartOfPipeline)
                    {
                        if (map[i][j].Pipe == '|')
                        {
                            inside = !inside;
                        }
                        else if (segmentStartedWith == ' ')
                        {
                            segmentStartedWith = map[i][j].Pipe;
                        }
                        else if (map[i][j].Pipe == 'J')
                        {
                            if (segmentStartedWith == 'F')
                            {
                                inside = !inside;
                                segmentStartedWith = ' ';
                            }
                            else if (segmentStartedWith == 'L')
                            {
                                segmentStartedWith = ' ';
                            }
                        }
                        else if (map[i][j].Pipe == '7')
                        {
                            if (segmentStartedWith == 'L')
                            {
                                inside = !inside;
                                segmentStartedWith = ' ';
                            }
                            else if (segmentStartedWith == 'F')
                            {
                                segmentStartedWith = ' ';
                            }
                        }

                    }
                    else
                    {
                        segmentStartedWith = ' ';
                        if (inside)
                        {
                            countOfInsideFields++;
                        }
                    }

                    if (map[i][j].IsPartOfPipeline)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (inside)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(map[i][j].Pipe);
                }
                Console.WriteLine();
            }

            //for (int i = 0; i < map.Count; i++)
            //{
            //    for (int j = 0; j < map[i].Count; j++)
            //    {
            //        if (map[i][j].IsPartOfPipeline)
            //        {
            //            Console.ForegroundColor = ConsoleColor.Red;
            //        }
            //        else
            //        {
            //            Console.ForegroundColor = ConsoleColor.White;
            //        }
            //        Console.Write(map[i][j].Pipe);
            //    }
            //    Console.WriteLine();
            //}


            //Console.WriteLine($"Pipeline: {string.Join(", ", pipeline.Select(p => $"({p.Row},{p.Col})").ToList())}");
            Console.WriteLine($"Part1: {pipeline.Count() / 2}");
            Console.WriteLine($"Part2: {countOfInsideFields}");
            Console.WriteLine();
        }
    }
}
