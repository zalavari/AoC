using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day06 : ISolvable
    {
        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path).ToList();
            var map = lines.Select(x => x.ToList()).ToList();

            var currentPosition = new Point(0, 0);
            var currentDirection = Direction.Up;
            var solution2 = 0;

            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j] == '>')
                    {
                        currentPosition = new Point(i, j);
                        currentDirection = Direction.Right;
                    }
                    else if (map[i][j] == '<')
                    {
                        currentPosition = new Point(i, j);
                        currentDirection = Direction.Left;
                    }
                    else if (map[i][j] == '^')
                    {
                        currentPosition = new Point(i, j);
                        currentDirection = Direction.Up;
                    }
                    else if (map[i][j] == 'v')
                    {
                        currentPosition = new Point(i, j);
                        currentDirection = Direction.Down;
                    }
                }
            }

            var (countOfVisited, _) = Patrol(map, currentPosition, currentDirection);

            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j] != '.')
                    {
                        continue;
                    }

                    map[i][j] = '#';

                    var (_, stuckInLoop) = Patrol(map, currentPosition, currentDirection);

                    if (stuckInLoop)
                    {
                        solution2++;
                    }

                    map[i][j] = '.';
                }
            }


            Console.WriteLine(countOfVisited);
            Console.WriteLine(solution2);
        }

        private (int countOfVisited, bool stuckInLoop) Patrol(List<List<char>> map, Point currentPosition, Direction currentDirection)
        {
            var visited = new HashSet<(Direction, Point)>();

            while (InBoundary(map, currentPosition.X, currentPosition.Y) && !visited.Contains((currentDirection, currentPosition)))
            {
                visited.Add((currentDirection, currentPosition));
                var nextPosition = new Point(currentPosition.X, currentPosition.Y);
                switch (currentDirection)
                {
                    case Direction.Up:
                        nextPosition.X--;
                        break;
                    case Direction.Down:
                        nextPosition.X++;
                        break;
                    case Direction.Left:
                        nextPosition.Y--;
                        break;
                    case Direction.Right:
                        nextPosition.Y++;
                        break;
                }

                if (InBoundary(map, nextPosition.X, nextPosition.Y) && map[nextPosition.X][nextPosition.Y] == '#')
                {
                    currentDirection = Rotate90(currentDirection);
                }
                else
                {
                    currentPosition = nextPosition;
                }
            }

            var countOfVisited = visited.Select(x => x.Item2).Distinct().Count();
            return (countOfVisited, InBoundary(map, currentPosition.X, currentPosition.Y));
        }

        private bool InBoundary(List<List<char>> mtx, int x, int y)
        {
            return x >= 0 && x < mtx.Count && y >= 0 && y < mtx[0].Count;
        }

        private Direction Rotate90(Direction direction)
        {
            return direction switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new InvalidEnumArgumentException()
            };
        }
    }
}
