using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    //Naive solution, it works great for first part
    internal class Day17_1
    {
        private class MovementProvider
        {
            string descriptor;
            int index = 0;
            public MovementProvider(string descriptor)
            {
                this.descriptor = descriptor;
            }

            public bool BlowsToRight()
            {
                return descriptor[index++ % descriptor.Length] == '>';
            }
        }

        private class Chamber
        {
            public int HighestPoint { get; set; } = 0;

            public List<List<bool>> blocked = new List<List<bool>>();

            public Chamber()
            {
                int height = 10000;

                //Left side
                blocked.Add(new List<bool>());
                blocked.Last().Add(true);
                for (int j = 0; j < height; j++)
                {
                    blocked.Last().Add(true);
                }

                //Middle
                for (int i = 0; i < 7; i++)
                {
                    blocked.Add(new List<bool>());
                    blocked.Last().Add(true);
                    for (int j = 0; j < height; j++)
                    {
                        blocked.Last().Add(false);
                    }
                }

                //Right side
                blocked.Add(new List<bool>());
                blocked.Last().Add(true);
                for (int j = 0; j < height; j++)
                {
                    blocked.Last().Add(true);
                }
            }
        }



        public void Solve(string path)
        {
            Console.WriteLine(path);

            // Parse file and init variables

            var line = File.ReadAllLines(path).First();
            var movementProvider = new MovementProvider(line);
            var chamber = new Chamber();
            var rockProvider = new RockProvider();

            while (rockProvider.ProvidedRocks < 2022)
            {
                var rock = rockProvider.GetNextRock(chamber.HighestPoint);

                var blocked = false;
                while (!blocked)
                {
                    //sideways
                    rock.MoveSideways(chamber, movementProvider.BlowsToRight());

                    //downwards
                    blocked = !rock.MoveDownward(chamber);

                    //solidify
                    if (blocked)
                        rock.Solidify(chamber);

                }


                //Console.WriteLine(PrintChamber(chamber));
            }

            //Console.WriteLine(PrintChamber(chamber));

            Console.WriteLine();
            Console.WriteLine($"resultA: {chamber.HighestPoint}");
            Console.WriteLine($"resultB: {0}");
            Console.WriteLine();
        }

        private class RockProvider
        {
            public int ProvidedRocks { get; private set; }

            public Rock GetNextRock(int heighestPoint)
            {
                ProvidedRocks++;
                //return new Rock1() { X = 3, Y = heighestPoint + 4 };
                switch (ProvidedRocks % 5)
                {
                    case 1: return new Rock1() { X = 3, Y = heighestPoint + 4 };
                    case 2: return new Rock2() { X = 3, Y = heighestPoint + 4 };
                    case 3: return new Rock3() { X = 3, Y = heighestPoint + 4 };
                    case 4: return new Rock4() { X = 3, Y = heighestPoint + 4 };
                    case 0: return new Rock5() { X = 3, Y = heighestPoint + 4 };
                    default: throw new InvalidOperationException("wtf?");
                }
            }
        }

        private abstract class Rock
        {
            public int X, Y;

            public bool MoveSideways(Chamber chamber, bool moveRight)
            {
                if (moveRight)
                {
                    var failure = BlocksWith(chamber, X + 1, Y);
                    if (!failure)
                        X++;
                    return !failure;
                }
                else
                {
                    var failure = BlocksWith(chamber, X - 1, Y);
                    if (!failure)
                        X--;
                    return !failure;
                }
            }

            public bool MoveDownward(Chamber chamber)
            {
                var failure = BlocksWith(chamber, X, Y - 1);
                if (!failure)
                    Y--;

                return !failure;
            }

            protected abstract bool BlocksWith(Chamber chamber, int x, int y);
            public abstract void Solidify(Chamber chamber);
        }

        private class Rock1 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return chamber.blocked[x + 0][y + 0]
                    || chamber.blocked[x + 1][y + 0]
                    || chamber.blocked[x + 2][y + 0]
                    || chamber.blocked[x + 3][y + 0];
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[X + 0][Y] = true;
                chamber.blocked[X + 1][Y] = true;
                chamber.blocked[X + 2][Y] = true;
                chamber.blocked[X + 3][Y] = true;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y);
            }
        }

        private class Rock2 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return chamber.blocked[x + 0][y + 1]
                    || chamber.blocked[x + 1][y + 0]
                    || chamber.blocked[x + 1][y + 1]
                    || chamber.blocked[x + 1][y + 2]
                    || chamber.blocked[x + 2][y + 1];
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[X + 0][Y + 1] = true;
                chamber.blocked[X + 1][Y + 0] = true;
                chamber.blocked[X + 1][Y + 1] = true;
                chamber.blocked[X + 1][Y + 2] = true;
                chamber.blocked[X + 2][Y + 1] = true;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y + 2);
            }
        }

        private class Rock3 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return chamber.blocked[x + 0][y + 0]
                    || chamber.blocked[x + 1][y + 0]
                    || chamber.blocked[x + 2][y + 0]
                    || chamber.blocked[x + 2][y + 1]
                    || chamber.blocked[x + 2][y + 2];
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[X + 0][Y + 0] = true;
                chamber.blocked[X + 1][Y + 0] = true;
                chamber.blocked[X + 2][Y + 0] = true;
                chamber.blocked[X + 2][Y + 1] = true;
                chamber.blocked[X + 2][Y + 2] = true;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y + 2);
            }
        }

        private class Rock4 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return chamber.blocked[x + 0][y + 0]
                    || chamber.blocked[x + 0][y + 1]
                    || chamber.blocked[x + 0][y + 2]
                    || chamber.blocked[x + 0][y + 3];
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[X + 0][Y + 0] = true;
                chamber.blocked[X + 0][Y + 1] = true;
                chamber.blocked[X + 0][Y + 2] = true;
                chamber.blocked[X + 0][Y + 3] = true;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y + 3);
            }
        }

        private class Rock5 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return chamber.blocked[x + 0][y + 0]
                    || chamber.blocked[x + 0][y + 1]
                    || chamber.blocked[x + 1][y + 0]
                    || chamber.blocked[x + 1][y + 1];
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[X + 0][Y + 0] = true;
                chamber.blocked[X + 0][Y + 1] = true;
                chamber.blocked[X + 1][Y + 0] = true;
                chamber.blocked[X + 1][Y + 1] = true;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y + 1);
            }
        }

        string PrintChamber(Chamber chamber)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = chamber.HighestPoint; y >= 0; y--)
            {
                for (int x = 0; x < 9; x++)
                {
                    sb.Append(chamber.blocked[x][y] ? "#" : ".");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
