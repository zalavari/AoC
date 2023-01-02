using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{

    internal class Day22 : ISolvable
    {
        private class Tile
        {
            public bool IsBoundary { get; set; }
            public bool IsWall { get; set; }
            public List<(int, int, int)> Neighbours { get; } = new List<(int, int, int)>() { (-1, -1, -1), (-1, -1, -1), (-1, -1, -1), (-1, -1, -1) };
            public string Face { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path);

            var H = lines.Take(lines.Count() - 2).Count() + 2;
            var W = lines.Take(lines.Count() - 2).Max(line => line.Length) + 2;

            int x, y;

            var tiles = new List<List<Tile>>();
            for (y = 0; y < H; y++)
            {
                tiles.Add(new List<Tile>());
                for (x = 0; x < W; x++)
                {
                    tiles[y].Add(new Tile() { IsBoundary = true });
                }
            }

            var lineNumber = 0;
            while (lines[lineNumber] != "")
            {
                var line = lines[lineNumber];
                for (int i = 0; i < line.Length; i++)
                {
                    var tile = tiles[lineNumber + 1][i + 1];
                    var c = line[i];

                    if (c == '#')
                    {
                        tile.IsBoundary = false;
                        tile.IsWall = true;
                    }
                    else if (c == '.')
                    {
                        tile.IsBoundary = false;
                        tile.IsWall = false;
                    }
                }

                lineNumber++;
            }

            for (int X = 1; X < W - 1; X++)
            {
                for (int Y = 1; Y < H - 1; Y++)
                {
                    var tile = tiles[Y][X];
                    tile.Face = $"{Y / 50}{X / 50}";
                }
            }

            for (int Y = 0; Y < H; Y++)
            {
                //Console.WriteLine();
                for (int X = 0; X < W; X++)
                {
                    var p = tiles[Y][X].Face == "" ? "  " : tiles[Y][X].Face;
                    //Console.Write(p);

                }
            }

            //Add basic directions
            for (int X = 1; X < W - 1; X++)
            {
                for (int Y = 1; Y < H - 1; Y++)
                {
                    var tile = tiles[Y][X];
                    x = X;
                    y = Y;

                    //right directions
                    x = X + 1;
                    y = Y;
                    if (!tiles[y][x].IsBoundary)
                        tile.Neighbours[0] = (x, y, 0);

                    //down directions
                    x = X;
                    y = Y + 1;
                    if (!tiles[y][x].IsBoundary)
                        tile.Neighbours[1] = (x, y, 1);

                    //left directions
                    x = X - 1;
                    y = Y;
                    if (!tiles[y][x].IsBoundary)
                        tile.Neighbours[2] = (x, y, 2);

                    //up directions
                    x = X;
                    y = Y - 1;
                    if (!tiles[y][x].IsBoundary)
                        tile.Neighbours[3] = (x, y, 3);




                    ////right directions
                    //x = X + 1;
                    //y = Y;
                    //while (tiles[y][x].IsBoundary)
                    //    x = (x + 1) % W;

                    //tile.Neighbours.Add((x, y));

                    ////down directions
                    //x = X;
                    //y = Y + 1;
                    //while (tiles[y][x].IsBoundary)
                    //    y = (y + 1) % H;

                    //tile.Neighbours.Add((x, y));

                    ////left directions
                    //x = X - 1;
                    //y = Y;
                    //while (tiles[y][x].IsBoundary)
                    //    x = x - 1 >= 0 ? x - 1 : x - 1 + W;

                    //tile.Neighbours.Add((x, y));

                    ////up directions
                    //x = X;
                    //y = Y - 1;
                    //while (tiles[y][x].IsBoundary)
                    //    y = y - 1 >= 0 ? y - 1 : y - 1 + H;

                    //tile.Neighbours.Add((x, y));

                }
            }

            //Add wrapping directions

            int x1, x2, y1, y2;

            y1 = 1;
            x1 = 150;
            y2 = 150;
            x2 = 100;
            for (int i = 0; i < 50; i++)
            {
                tiles[y1 + i][x1].Neighbours[0] = (x2, y2 - i, 2);
                tiles[y2 - i][x2].Neighbours[0] = (x1, y1 + i, 2);
            }

            y1 = 51;
            x1 = 100;
            y2 = 50;
            x2 = 101;
            for (int i = 0; i < 50; i++)
            {
                tiles[y1 + i][x1].Neighbours[0] = (x2 + i, y2, 3);
                tiles[y2][x2 + i].Neighbours[1] = (x1, y1 + i, 2);
            }

            y1 = 151;
            x1 = 50;
            y2 = 150;
            x2 = 51;
            for (int i = 0; i < 50; i++)
            {
                tiles[y1 + i][x1].Neighbours[0] = (x2 + i, y2, 3);
                tiles[y2][x2 + i].Neighbours[1] = (x1, y1 + i, 2);
            }

            y1 = 200;
            x1 = 1;
            y2 = 1;
            x2 = 101;
            for (int i = 0; i < 50; i++)
            {
                tiles[y1][x1 + i].Neighbours[1] = (x2 + i, y2, 1);
                tiles[y2][x2 + i].Neighbours[3] = (x1 + i, y1, 3);
            }


            y1 = 1;
            x1 = 51;
            y2 = 150;
            x2 = 1;
            for (int i = 0; i < 50; i++)
            {
                tiles[y1 + i][x1].Neighbours[2] = (x2, y2 - i, 0);
                tiles[y2 - i][x2].Neighbours[2] = (x1, y1 + i, 0);
            }

            y1 = 151;
            x1 = 1;
            y2 = 1;
            x2 = 51;
            for (int i = 0; i < 50; i++)
            {
                tiles[y1 + i][x1].Neighbours[2] = (x2 + i, y2, 1);
                tiles[y2][x2 + i].Neighbours[3] = (x1, y1 + i, 0);
            }

            y1 = 51;
            x1 = 51;
            y2 = 101;
            x2 = 1;
            for (int i = 0; i < 50; i++)
            {
                tiles[y1 + i][x1].Neighbours[2] = (x2 + i, y2, 1);
                tiles[y2][x2 + i].Neighbours[3] = (x1, y1 + i, 0);
            }


            var instructions = lines.Last()
                .Replace("R", " R ")
                .Replace("L", " L ")
                .Replace("  ", " ")
                .Split(" ");

            var face = 0;
            y = 1;
            x = 1;
            while (tiles[y][x].IsBoundary)
                x++;

            for (int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == "R")
                {
                    face = (face + 1) % 4;

                }
                else if (instructions[i] == "L")
                {
                    face = (face + 3) % 4;

                }
                else
                {
                    var steps = int.Parse(instructions[i]);
                    for (int step = 0; step < steps; step++)
                    {
                        var (nextX, nextY, nextFace) = tiles[y][x].Neighbours[face];
                        if (tiles[nextY][nextX].IsBoundary)
                            throw new InvalidOperationException("Strange things happened");
                        if (tiles[nextY][nextX].IsWall)
                            break;

                        x = nextX;
                        y = nextY;
                        face = nextFace;

                    }
                }


            }





            Console.WriteLine($"resultB: {1000 * y + 4 * x + face}");
            Console.WriteLine();
        }

    }
}