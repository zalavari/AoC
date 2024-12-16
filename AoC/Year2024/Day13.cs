using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2024
{
    internal class Day13 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();

            var allCosts = new List<long>();

            for (int i = 0; i < lines.Count; i += 4)
            {
                var lineA = lines[i];
                var lineB = lines[i + 1];
                var linePrize = lines[i + 2];

                Regex patternA = new Regex(@"Button A: X\+(?<ax>[0-9]+), Y\+(?<ay>[0-9]+)");
                Match match = patternA.Match(lineA);
                int ax = int.Parse(match.Groups["ax"].Value);
                int ay = int.Parse(match.Groups["ay"].Value);

                Regex patternB = new Regex(@"Button B: X\+(?<bx>[0-9]+), Y\+(?<by>[0-9]+)");
                match = patternB.Match(lineB);
                int bx = int.Parse(match.Groups["bx"].Value);
                int by = int.Parse(match.Groups["by"].Value);

                Regex patternPrize = new Regex(@"Prize: X=(?<px>[0-9]+), Y=(?<py>[0-9]+)");
                match = patternPrize.Match(linePrize);
                long px = int.Parse(match.Groups["px"].Value) + 10000000000000;
                long py = int.Parse(match.Groups["py"].Value) + 10000000000000;

                long cost = GetMinimalCost(ax, ay, bx, by, px, py);
                allCosts.Add(cost);
            }

            Console.WriteLine($"Sum of all costs: {allCosts.Sum()}");
            Console.WriteLine();
        }

        private long GetMinimalCost(long ax, long ay, long bx, long by, long px, long py)
        {
            //Console.WriteLine($"{ax}a+{bx}b = {px}");
            var gcdX = GCD(ax, bx);
            if (px % gcdX != 0)
            {
                Console.WriteLine("No solutions!");
                return 0;
            }
            ax /= gcdX;
            bx /= gcdX;
            px /= gcdX;


            //Console.WriteLine($"{ay}a+{by}b = {py}");
            var gcdY = GCD(ay, by);
            if (py % gcdY != 0)
            {
                Console.WriteLine("No solutions!");
                return 0;
            }
            ay /= gcdY;
            by /= gcdY;
            py /= gcdY;


            //Console.WriteLine("After simplification: ");
            //Console.WriteLine($"{ax}a+{bx}b = {px}");
            //Console.WriteLine($"{ay}a+{by}b = {py}");


            // Find a0 and b0 to satisfy ax*a0+bx*b0=px
            var (a0, b0) = GetBezoutCoefficients(ax, bx);
            a0 *= px;
            b0 *= px;

            //Console.WriteLine($"{ax}*{a0}+{bx}*{b0} = {px}");
            //Console.WriteLine($"{(ax * a0) + (bx * b0)} = {px}");
            //Console.WriteLine($"{(ax * a0) + (bx * b0) - px} = {0}");

            var solutionsx = new List<(long, long)>();

            var k = 0L;
            if (a0 < 0)
            {
                k = (Math.Abs(a0) / bx) + 1;
                a0 += k * bx;
                b0 -= k * ax;
            }
            else
            {
                k = a0 / bx;
                a0 -= k * bx;
                b0 += k * ax;
            }

            // a = a0x + k * bx;
            // b = b0x - k * ax;


            // a = a0y + l * by;
            // b = b0y - l * ay;



            while (b0 >= 0)
            {

                solutionsx.Add((a0, b0));

                solutionsx.Add((a0, b0));
                a0 += bx;
                b0 -= ax;
            }

            // Find a0 and b0 to satisfy ay*a0+by*b0=py
            (a0, b0) = GetBezoutCoefficients(ay, by);
            a0 *= py;
            b0 *= py;

            //Console.WriteLine($"{ay}*{a0}+{by}*{b0} = {py}");
            //Console.WriteLine($"{(ay * a0) + (by * b0)} = {py}");
            //Console.WriteLine($"{(ay * a0) + (by * b0) - py} = {0}");

            var solutionsy = new List<(long, long)>();

            k = 0;
            if (a0 < 0)
            {
                k = (Math.Abs(a0) / by) + 1;
                a0 += k * by;
                b0 -= k * ay;
            }
            else
            {
                k = a0 / by;
                a0 -= k * by;
                b0 += k * ay;
            }

            while (b0 >= 0)
            {
                solutionsy.Add((a0, b0));
                a0 += by;
                b0 -= ay;
            }

            var cost = long.MaxValue;
            for (int i = 0; i < solutionsx.Count; i++)
            {
                for (int j = 0; j < solutionsy.Count; j++)
                {
                    var (x, y) = solutionsx[i];
                    var (z, w) = solutionsy[j];
                    if (x == z && y == w)
                    {
                        //Console.WriteLine($"Solution: {x}a+{y}b = {px}");
                        //Console.WriteLine($"Solution: {x}a+{y}b = {py}");
                        if ((3 * x) + y < cost)
                        {
                            cost = (3 * x) + y;
                        }
                    }
                }
            }

            if (cost == long.MaxValue)
            {
                Console.WriteLine("No common solutions!");
                return 0;
            }

            Console.WriteLine($"Cost: {cost}");
            return cost;
        }

        private static long GCD(long a, long b)
        {
            if (a < b)
            {
                return GCD(b, a);
            }

            if (b == 0)
            {
                return a;
            }
            return GCD(b, a % b);
        }

        private static (long, long) GetBezoutCoefficients(long a, long b)
        {
            if (b == 0)
            {
                return (1, 0);
            }
            var (x, y) = GetBezoutCoefficients(b, a % b);
            return (y, x - (a / b * y));
        }
    }
}
