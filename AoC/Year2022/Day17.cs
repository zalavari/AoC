using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{

    internal class Day17 : ISolvable
    {
        private class MovementProvider
        {
            string descriptor;
            public int Index { get; private set; } = 0;
            public MovementProvider(string descriptor)
            {
                this.descriptor = descriptor;
            }

            public bool BlowsToRight()
            {
                if (Index == descriptor.Length)
                    Index = 0;
                return descriptor[Index++] == '>';
            }
        }

        private class Chamber
        {
            public int HighestPoint { get; set; } = 0;

            public List<int> blocked = new List<int>();

            public Chamber()
            {
                int height = 1_000_000;
                blocked.Add((1 << 10) - 1);

                for (int j = 0; j < height; j++)
                {
                    blocked.Add(1 << 8 | 1 << 0);
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

            var states = new Dictionary<(int index, int rockType, string pattern), (int height, long numberOfBlocks)>();
            var cycleFound = false;
            long allBlockCount = 1_000_000_000_000;
            long heightToAdd = 0;
            long heightIncrementPerCycle = 0;
            long blockIncrementPerCycle = 0;
            long cycleCount = 0;

            while (rockProvider.ProvidedRocks < allBlockCount)
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
                if (cycleFound || chamber.HighestPoint < 600)
                    continue;

                var index = movementProvider.Index;
                var rockType = (int)(rockProvider.ProvidedRocks % 5);
                var pattern = chamber.blocked
                    .Skip(chamber.HighestPoint - 500)
                    .Take(90)
                    .Select(a => (char)a);

                var patternAsString = new string(pattern.ToArray());

                var result = states.TryAdd((index, rockType, patternAsString), (chamber.HighestPoint, rockProvider.ProvidedRocks));

                if (!result)
                {
                    cycleFound = true;
                    var (hp, bs) = states[(index, rockType, patternAsString)];
                    heightIncrementPerCycle = chamber.HighestPoint - hp;
                    blockIncrementPerCycle = rockProvider.ProvidedRocks - bs;

                    var neededBlockCount = allBlockCount - rockProvider.ProvidedRocks;
                    cycleCount = neededBlockCount / blockIncrementPerCycle;


                    heightToAdd = cycleCount * heightIncrementPerCycle;
                    rockProvider.ProvidedRocks += cycleCount * blockIncrementPerCycle;

                }


                //Console.WriteLine(PrintChamber(chamber));
            }

            //Console.WriteLine(PrintChamber(chamber));

            Console.WriteLine($"heightIncrementPerCycle: {heightIncrementPerCycle}");
            Console.WriteLine($"blockIncrementPerCycle: {blockIncrementPerCycle}");
            Console.WriteLine($"heightToAdd: {heightToAdd} = {cycleCount} * {heightIncrementPerCycle}");

            Console.WriteLine($"chamber.HighestPoint: {chamber.HighestPoint} + {heightToAdd}");

            Console.WriteLine();
            Console.WriteLine($"resultA: {chamber.HighestPoint + heightToAdd}");
            Console.WriteLine($"resultB: {0}");
            Console.WriteLine();
        }



        private class RockProvider
        {
            public long ProvidedRocks { get; set; }

            public Rock GetNextRock(int heighestPoint)
            {
                ProvidedRocks++;
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
                return (chamber.blocked[y + 0] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 0] & 1 << x + 1) != 0
                    || (chamber.blocked[y + 0] & 1 << x + 2) != 0
                    || (chamber.blocked[y + 0] & 1 << x + 3) != 0;
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[Y] |= 1 << X + 0;
                chamber.blocked[Y] |= 1 << X + 1;
                chamber.blocked[Y] |= 1 << X + 2;
                chamber.blocked[Y] |= 1 << X + 3;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y);
            }
        }

        private class Rock2 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return (chamber.blocked[y + 1] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 0] & 1 << x + 1) != 0
                    || (chamber.blocked[y + 1] & 1 << x + 1) != 0
                    || (chamber.blocked[y + 2] & 1 << x + 1) != 0
                    || (chamber.blocked[y + 1] & 1 << x + 2) != 0;
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[Y + 1] |= 1 << X + 0;
                chamber.blocked[Y + 0] |= 1 << X + 1;
                chamber.blocked[Y + 1] |= 1 << X + 1;
                chamber.blocked[Y + 2] |= 1 << X + 1;
                chamber.blocked[Y + 1] |= 1 << X + 2;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y + 2);
            }
        }

        private class Rock3 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return (chamber.blocked[y + 0] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 0] & 1 << x + 1) != 0
                    || (chamber.blocked[y + 0] & 1 << x + 2) != 0
                    || (chamber.blocked[y + 1] & 1 << x + 2) != 0
                    || (chamber.blocked[y + 2] & 1 << x + 2) != 0;
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[Y + 0] |= 1 << X + 0;
                chamber.blocked[Y + 0] |= 1 << X + 1;
                chamber.blocked[Y + 0] |= 1 << X + 2;
                chamber.blocked[Y + 1] |= 1 << X + 2;
                chamber.blocked[Y + 2] |= 1 << X + 2;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y + 2);
            }
        }

        private class Rock4 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return (chamber.blocked[y + 0] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 1] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 2] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 3] & 1 << x + 0) != 0;
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[Y + 0] |= 1 << X + 0;
                chamber.blocked[Y + 1] |= 1 << X + 0;
                chamber.blocked[Y + 2] |= 1 << X + 0;
                chamber.blocked[Y + 3] |= 1 << X + 0;
                chamber.HighestPoint = Math.Max(chamber.HighestPoint, Y + 3);
            }
        }

        private class Rock5 : Rock
        {
            protected override bool BlocksWith(Chamber chamber, int x, int y)
            {
                return (chamber.blocked[y + 0] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 1] & 1 << x + 0) != 0
                    || (chamber.blocked[y + 0] & 1 << x + 1) != 0
                    || (chamber.blocked[y + 1] & 1 << x + 1) != 0;
            }

            public override void Solidify(Chamber chamber)
            {
                chamber.blocked[Y + 0] |= 1 << X + 0;
                chamber.blocked[Y + 1] |= 1 << X + 0;
                chamber.blocked[Y + 0] |= 1 << X + 1;
                chamber.blocked[Y + 1] |= 1 << X + 1;
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
                    sb.Append((chamber.blocked[y] & 1 << x) != 0 ? "#" : ".");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
