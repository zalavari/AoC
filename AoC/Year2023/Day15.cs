using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day15 : ISolvable
    {
        private class Lens
        {
            public string Label { get; set; }
            public int Focal { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var line = lines[0];
            var sequence = line.Split(',').ToList();

            var boxes = Enumerable.Repeat(0, 256).Select(_ => new List<Lens>()).ToList();

            var part1 = 0;
            foreach (var item in sequence)
            {
                part1 += HASH(item);
            }

            foreach (var item in sequence)
            {
                if (item.Contains("-"))
                {
                    var label = item.Split('-')[0];
                    var boxId = HASH(label);
                    boxes[boxId].RemoveAll(lens => lens.Label == label);
                }
                else if (item.Contains("="))
                {
                    var label = item.Split('=')[0];
                    var focal = int.Parse(item.Split('=')[1]);
                    var boxId = HASH(label);
                    var lens = boxes[boxId].FirstOrDefault(lens => lens.Label == label);
                    if (lens != null)
                    {
                        lens.Focal = focal;
                    }
                    else
                    {
                        boxes[boxId].Add(new Lens() { Label = label, Focal = focal });
                    }
                }
            }

            var power = 0;
            for (int boxId = 0; boxId < boxes.Count; boxId++)
            {
                for (var lensId = 0; lensId < boxes[boxId].Count; lensId++)
                {
                    power += (boxId + 1) * (lensId + 1) * boxes[boxId][lensId].Focal;
                }
            }


            Console.WriteLine(part1);
            Console.WriteLine(power);
        }

        private static int HASH(string input)
        {
            var result = 0;
            foreach (var c in input)
            {
                result = (result + c) * 17 % 256;
            }

            return result;
        }

    }
}
