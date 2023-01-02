using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{


    internal class Day21 : ISolvable
    {

        private class Monkey
        {
            public string Name { get; set; }
            public long? Value { get; set; } = null;
            public string Monkey1 { get; set; }
            public string Monkey2 { get; set; }

            public string Operand { get; set; }

            public long Job(long x, long y)
            {
                if (Operand == "+")
                    return x + y;
                if (Operand == "-")
                    return x - y;
                if (Operand == "*")
                    return x * y;
                if (Operand == "/")
                    return x / y;

                throw new InvalidOperationException();
            }


        }

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path);
            var monkeys = new Dictionary<string, Monkey>();

            foreach (var line in lines)
            {

                Regex leaf = new Regex(@"(?<name>[a-zA-Z]+): (?<number>\d+)");
                Regex compound = new Regex(@"(?<name>[a-zA-Z]+): (?<monkey1>[a-zA-Z]+) (?<operand>.) (?<monkey2>[a-zA-Z]+)");


                Match match = leaf.Match(line);
                if (match.Success)
                {
                    var name = match.Groups[$"name"].Value;
                    var number = match.Groups[$"number"].Value;
                    monkeys.Add(name, new Monkey() { Name = name, Value = int.Parse(number) });
                }
                else
                {
                    match = compound.Match(line);
                    var name = match.Groups[$"name"].Value;
                    var monkey1 = match.Groups[$"monkey1"].Value;
                    var monkey2 = match.Groups[$"monkey2"].Value;
                    var operand = match.Groups[$"operand"].Value;



                    monkeys.Add(name, new Monkey() { Name = name, Monkey1 = monkey1, Monkey2 = monkey2, Operand = operand });

                }
            }

            Console.WriteLine($"resultA: {GetValue(monkeys["root"], monkeys)}");

            //Console.WriteLine(PrintMonkeyStructure("root", monkeys));

            monkeys["humn"].Value = null;

            var leftValue = GetValues2(monkeys["root"].Monkey1, monkeys);
            var rightValue = GetValues2(monkeys["root"].Monkey2, monkeys);

            if (leftValue == null && rightValue == null)
                throw new InvalidOperationException("Cant be both nulls!");

            if (leftValue == null)
                SetValues2((long)rightValue, monkeys["root"].Monkey1, monkeys);

            if (rightValue == null)
                SetValues2((long)leftValue, monkeys["root"].Monkey2, monkeys);




            Console.WriteLine($"resultB: {monkeys["humn"].Value}");
            Console.WriteLine();
        }

        string PrintMonkeyStructure(string rootName, Dictionary<string, Monkey> monkeys)
        {
            var root = monkeys[rootName];
            if (root.Value != null)
                return root.Value.ToString();

            return "(" + PrintMonkeyStructure(root.Monkey1, monkeys) + " " + root.Operand + " " + PrintMonkeyStructure(root.Monkey2, monkeys) + ")";
        }

        private long GetValue(Monkey root, Dictionary<string, Monkey> monkeys)
        {
            if (root.Value != null)
                return (long)root.Value;

            return root.Job(GetValue(monkeys[root.Monkey1], monkeys), GetValue(monkeys[root.Monkey2], monkeys));
        }

        private long? GetValues2(string name, Dictionary<string, Monkey> monkeys)
        {
            if (name == "humn")
                return null;

            var root = monkeys[name];
            if (root.Value != null)
                return root.Value;

            var leftValue = GetValues2(root.Monkey1, monkeys);
            var rightValue = GetValues2(root.Monkey2, monkeys);
            if (leftValue != null && rightValue != null)
                return root.Value = root.Job((long)leftValue, (long)rightValue);
            else
                return null;

        }

        private void SetValues2(long value, string name, Dictionary<string, Monkey> monkeys)
        {
            var root = monkeys[name];
            root.Value = value;

            if (name == "humn")
                return;

            var leftValue = monkeys[root.Monkey1].Value;
            var rightValue = monkeys[root.Monkey2].Value;

            if (leftValue == null && rightValue == null)
                throw new InvalidOperationException("Cant be both nulls!");

            if (root.Operand == "+")
            {
                if (rightValue == null)
                {
                    SetValues2(value - (long)leftValue, root.Monkey2, monkeys);
                }
                if (leftValue == null)
                {
                    SetValues2(value - (long)rightValue, root.Monkey1, monkeys);
                }
            }
            if (root.Operand == "-")
            {
                if (rightValue == null)
                {
                    SetValues2(-value + (long)leftValue, root.Monkey2, monkeys);
                }
                if (leftValue == null)
                {
                    SetValues2(value + (long)rightValue, root.Monkey1, monkeys);
                }
            }

            if (root.Operand == "*")
            {
                if (rightValue == null)
                {
                    SetValues2(value / (long)leftValue, root.Monkey2, monkeys);
                }
                if (leftValue == null)
                {
                    SetValues2(value / (long)rightValue, root.Monkey1, monkeys);
                }
            }

            if (root.Operand == "/")
            {
                if (rightValue == null)
                {
                    SetValues2((long)leftValue / value, root.Monkey2, monkeys);
                }
                if (leftValue == null)
                {
                    SetValues2(value * (long)rightValue, root.Monkey1, monkeys);
                }
            }

        }


    }
}