using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AoC.Year2023
{
    internal class Day24 : ISolvable
    {
        private const double lowerBoundary = 200000000000000;
        private const double upperBoundary = 400000000000000;
        //private const double lowerBoundary = 7;
        //private const double upperBoundary = 27;

        private class Hail
        {
            public long[] Position { get; init; }
            public long[] Velocity { get; init; }
        }


        public void Solve(string filePath)
        {
            Console.WriteLine(filePath);
            var lines = File.ReadAllLines(filePath);

            var hails = new List<Hail>();
            foreach (var line in lines)
            {
                var arr = line.Split(new char[] { '@', ',' });
                var hail = new Hail()
                {
                    Position = arr.Take(3).Select(long.Parse).ToArray(),
                    Velocity = arr.Skip(3).Select(long.Parse).ToArray(),
                };
                hails.Add(hail);
            }

            //Part1(hails);

            //Part2(hails);

            //var rock = new Hail()
            //{
            //    Position = new long[] { 12, 13, 15 },
            //    Velocity = new long[] { 1, 1, -1 },
            //};

            for (int i = 0; i < hails.Count; i++)
            {
                for (int j = i + 1; j < hails.Count; j++)
                {
                    if (AreTrajectorisesOnSamePlaneBig(hails[i], hails[j]))
                        Console.WriteLine($"Same plane: ({i} - {j})");
                }
            }

        }

        private void Part1(List<Hail> hails)
        {
            var part1 = 0;
            for (int i = 0; i < hails.Count; i++)
            {
                for (int j = i + 1; j < hails.Count; j++)
                {
                    if (IntersectingTrajectoies(hails[i], hails[j]))
                        part1++;
                }
            }

            Console.WriteLine(part1);
        }

        private bool AreTrajectorisesOnSamePlane(Hail a, Hail b)
        {
            var s = new List<long>();
            s.Add((a.Velocity[1] * b.Velocity[2]) - (a.Velocity[2] * b.Velocity[1]));
            s.Add((a.Velocity[2] * b.Velocity[0]) - (a.Velocity[0] * b.Velocity[2]));
            s.Add((a.Velocity[0] * b.Velocity[1]) - (a.Velocity[1] * b.Velocity[0]));

            var ac = (s[0] * a.Position[0]) + (s[1] * a.Position[1]) + (s[2] * a.Position[2]);
            var bc = (s[0] * b.Position[0]) + (s[1] * b.Position[1]) + (s[2] * b.Position[2]);

            return ac == bc;
        }

        private bool AreTrajectorisesOnSamePlaneBig(Hail a, Hail b)
        {
            var s = new List<BigInteger>();
            s.Add(((BigInteger)a.Velocity[1] * b.Velocity[2]) - ((BigInteger)a.Velocity[2] * b.Velocity[1]));
            s.Add(((BigInteger)a.Velocity[2] * b.Velocity[0]) - ((BigInteger)a.Velocity[0] * b.Velocity[2]));
            s.Add(((BigInteger)a.Velocity[0] * b.Velocity[1]) - ((BigInteger)a.Velocity[1] * b.Velocity[0]));

            var ac = (s[0] * a.Position[0]) + (s[1] * a.Position[1]) + (s[2] * a.Position[2]);
            var bc = (s[0] * b.Position[0]) + (s[1] * b.Position[1]) + (s[2] * b.Position[2]);

            return ac == bc;
        }

        private bool IntersectingTrajectoies(Hail a, Hail b)
        {
            var lambda1 = ((b.Velocity[0] * (a.Position[1] - b.Position[1])) - (b.Velocity[1] * (a.Position[0] - b.Position[0]))) / ((double)((a.Velocity[0] * b.Velocity[1]) - (b.Velocity[0] * a.Velocity[1])));
            var lambda2 = ((a.Velocity[0] * (b.Position[1] - a.Position[1])) - (a.Velocity[1] * (b.Position[0] - a.Position[0]))) / ((double)((b.Velocity[0] * a.Velocity[1]) - (a.Velocity[0] * b.Velocity[1])));

            if (double.IsNaN(lambda1) && double.IsNaN(lambda2))
            {
                Console.WriteLine("Real parallell");
                return false;
            }

            if (lambda1 < 0 || lambda2 < 0)
            {
                Console.WriteLine("Intersecting in the past!");
                return false;
            }

            if (lambda1 == lambda2)
            {
                Console.WriteLine($"Intersecting at time = {lambda1}!");
                return false;
            }

            var intersectionX = a.Position[0] + (a.Velocity[0] * lambda1);
            var intersectionY = a.Position[1] + (a.Velocity[1] * lambda1);

            //var intersectionXb = b.Position[0] + (b.Velocity[0] * lambda2);
            //var intersectionYb = b.Position[1] + (b.Velocity[1] * lambda2);

            if (lowerBoundary <= intersectionX && intersectionX <= upperBoundary
                && lowerBoundary <= intersectionY && intersectionY <= upperBoundary)
            {
                Console.WriteLine("Intersecting within area!");
                return true;
            }

            Console.WriteLine("Intersecting outside of area!");
            return false;

        }

        private void Part2(List<Hail> hails)
        {
            var possiblePositions = new List<Range>
            {
                new Range(),
                new Range(),
                new Range(),
            };
            var possibleVelocities = new List<Range>
            {
                new Range(),
                new Range(),
                new Range(),
            };

            Console.WriteLine("Coordinate X");
            ConstraintRanges(hails, possiblePositions, possibleVelocities);
        }

        private void ConstraintRanges(IEnumerable<Hail> hails, List<Range> position, List<Range> velocity)
        {

            if (position.Count() != 3)
                return;

            if (velocity.Count() != 3)
                return;

            if (!hails.Any())
            {
                Console.WriteLine($"X: {position[0].Start} - {position[0].End}  {velocity[0].Start} - {velocity[0].End}");
                Console.WriteLine($"Y: {position[1].Start} - {position[1].End}  {velocity[1].Start} - {velocity[1].End}");
                Console.WriteLine($"Z: {position[2].Start} - {position[2].End}  {velocity[2].Start} - {velocity[2].End}");
                return;
            }
            var p = hails.First().Position;
            var v = hails.First().Velocity;

            var p1 = position
                .Select((r, dim) => new Range() { Start = r.Start, End = Math.Min(r.End, p[dim]) })
                .Where(r => r.End >= r.Start)
                .ToList();

            var v1 = velocity
                .Select((r, dim) => new Range() { Start = Math.Max(r.Start, v[dim]), End = r.End })
                .Where(r => r.End >= r.Start)
                .ToList();

            ConstraintRanges(hails.Skip(1), p1, v1);

            var p2 = position
                .Select((r, dim) => new Range() { Start = Math.Max(r.Start, p[dim]), End = r.End })
                .Where(r => r.End >= r.Start)
                .ToList();

            var v2 = velocity
                .Select((r, dim) => new Range() { Start = r.Start, End = Math.Min(r.End, v[dim]) })
                .Where(r => r.End >= r.Start)
                .ToList();


            ConstraintRanges(hails.Skip(1), p2, v2);
        }

        //private class Ranges
        //{
        //    public List<Range> Ranges = new List<Range>();
        //}


        private class Range
        {
            public long Start = long.MinValue;
            public long End = long.MaxValue;
        }
    }
}
