using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{
    internal class Day11 : ISolvable
    {
        private class Monkey
        {
            public string Name { get; set; }

            public long InspectCount { get; private set; } = 0;

            public Queue<long> Items { get; set; }

            public Func<long, long> Operation { get; set; }

            public Func<long, bool> Test { get; set; }

            public int MonkeyIfTrue { get; set; }
            public int MonkeyIfFalse { get; set; }


            public bool Inspect()
            {
                if (Items.Count == 0)
                    return false;

                InspectCount++;
                var item = Items.Dequeue();
                item = Operation(item);

                if (item < 0)
                    ;

                // item /= 3;
                item %= commonMultiple;

                var nextMonkey = Test(item) ? MonkeyIfTrue : MonkeyIfFalse;
                monkeys[nextMonkey].Items.Enqueue(item);
                return true;
            }

        }

        static List<Monkey> monkeys = new List<Monkey>();

        static int commonMultiple = 0;

        public void Solve(string path)
        {
            Console.WriteLine(path);

            monkeys.Clear();
            var divisors = new List<int>();

            var lines = File.ReadAllLines(path).ToList();

            for (int lineNumber = 0; lineNumber < lines.Count; lineNumber += 7)
            {
                var items = lines[lineNumber + 1]
                    .Replace(",", "")
                    .Split(" ")
                    .Skip(4)
                    .Select(long.Parse)
                    .ToList();

                var t0 = lines[lineNumber + 1];
                var t1 = t0.Replace(",", "");
                var t2 = t1.Split(" ").ToList();

                var op = lines[lineNumber + 2].Split(" ");
                Func<long, long> operation = x =>
                {
                    long lhs, rhs;
                    if (op[5] == "old")
                        lhs = x;
                    else
                        lhs = int.Parse(op[5]);

                    if (op[7] == "old")
                        rhs = x;
                    else
                        rhs = int.Parse(op[7]);

                    if (op[6] == "+")
                        return rhs + lhs;
                    else
                        return rhs * lhs;
                };

                var divisor = int.Parse(lines[lineNumber + 3].Split(" ").Last());
                divisors.Add(divisor);
                Func<long, bool> test = x => x % divisor == 0;

                var monkeyIfTrue = int.Parse(lines[lineNumber + 4].Split(" ").Last());
                var monkeyIfFalse = int.Parse(lines[lineNumber + 5].Split(" ").Last());

                var monkey = new Monkey()
                {
                    Name = $"Monkey {lineNumber / 7}",
                    Items = new Queue<long>(items),
                    Operation = operation,
                    Test = test,
                    MonkeyIfTrue = monkeyIfTrue,
                    MonkeyIfFalse = monkeyIfFalse,
                };

                monkeys.Add(monkey);

            }

            commonMultiple = divisors.Aggregate((a, b) => a * b);

            for (int round = 0; round < 10000; round++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Inspect()) ;
                }

            }

            var sortedMonkeys = monkeys.OrderByDescending(x => x.InspectCount).ToList();

            foreach (var monkey in monkeys)
            {
                Console.WriteLine($"{monkey.Name}: {monkey.InspectCount}");
            }


            Console.WriteLine();
            Console.WriteLine($"resultA: {sortedMonkeys[0].InspectCount * sortedMonkeys[1].InspectCount}");
            Console.WriteLine();
        }



    }
}
