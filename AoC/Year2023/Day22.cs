using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day22 : ISolvable
    {

        private class Brick
        {
            public int StartX { get; set; }
            public int StartY { get; set; }
            public int StartZ { get; set; }
            public int EndX { get; set; }
            public int EndY { get; set; }
            public int EndZ { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            var bricks = new List<Brick>();

            foreach (var line in lines)
            {
                var brick = new Brick();
                var parts = line.Split('~');
                var start = parts[0].Split(',');
                brick.StartX = int.Parse(start[0]);
                brick.StartY = int.Parse(start[1]);
                brick.StartZ = int.Parse(start[2]);
                var end = parts[1].Split(',');
                brick.EndX = int.Parse(end[0]);
                brick.EndY = int.Parse(end[1]);
                brick.EndZ = int.Parse(end[2]);
                bricks.Add(brick);
            }

            DropBricks(bricks);
            var disintegratable = 0;
            var disintegratable2 = 0;
            var wouldFall = 0;

            Console.WriteLine();
            Console.Write($"\rProgress... {0}/{bricks.Count}    ");
            for (int i = 0; i < bricks.Count; i++)
            {
                var bricksWithoutI = bricks.Where((b, index) => index != i).ToList();
                //if (!CouldDropBricks(bricksWithoutI))
                //{
                //    disintegratable++;
                //}
                var stableBricks = CountStableBricks(bricksWithoutI);
                if (stableBricks == bricksWithoutI.Count)
                {
                    disintegratable2++;
                }
                wouldFall += bricksWithoutI.Count - stableBricks;
                Console.Write($"\rProgress... {i + 1}/{bricks.Count}    ");
            }
            Console.WriteLine();

            Console.WriteLine(disintegratable);
            Console.WriteLine(disintegratable2);
            Console.WriteLine(wouldFall);

        }

        private void DropBricks(List<Brick> bricks)
        {
            var couldGoDown = true;
            while (couldGoDown)
            {
                couldGoDown = false;
                foreach (var brick in bricks)
                {
                    if (brick.StartZ == 1)
                        continue;

                    var below = bricks.Any(b => b.StartX <= brick.EndX && b.EndX >= brick.StartX && b.StartY <= brick.EndY && b.EndY >= brick.StartY && b.EndZ == brick.StartZ - 1);
                    if (!below)
                    {
                        brick.StartZ--;
                        brick.EndZ--;
                        couldGoDown = true;
                    }
                }
            }
        }

        private bool CouldDropBricks(List<Brick> bricks)
        {
            foreach (var brick in bricks)
            {
                if (brick.StartZ == 1)
                    continue;

                var below = bricks.Any(b => b.StartX <= brick.EndX && b.EndX >= brick.StartX && b.StartY <= brick.EndY && b.EndY >= brick.StartY && b.EndZ == brick.StartZ - 1);
                if (!below)
                {
                    return true;
                }
            }
            return false;
        }

        private int CountStableBricks(List<Brick> bricks)
        {
            var stableBricks = bricks.Where(brick => brick.StartZ == 1 || bricks.Any(b => b.StartX <= brick.EndX && b.EndX >= brick.StartX && b.StartY <= brick.EndY && b.EndY >= brick.StartY && b.EndZ == brick.StartZ - 1)).ToList();
            if (stableBricks.Count != bricks.Count)
            {
                return CountStableBricks(stableBricks);
            }
            return stableBricks.Count;
        }

    }
}
