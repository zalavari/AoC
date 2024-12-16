using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Year2024
{
    internal class Day09 : ISolvable
    {

        private class File
        {
            public int Size { get; set; }
            public int Id { get; set; }
            public int StartsAt { get; set; }
        }

        private class Space
        {
            public int Size { get; set; }
            public int StartsAt { get; set; }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();

            var numbers = lines[0].Select(c => int.Parse(c.ToString())).ToList();

            var files = new List<File>();
            var spaces = new List<int>();

            for (int i = 0; i < numbers.Count; i++)
            {
                if (i % 2 == 0)
                {
                    files.Add(new File()
                    {
                        Id = i / 2,
                        Size = numbers[i],
                    });
                }
                else if (i % 2 == 1)
                {
                    spaces.Add(numbers[i]);
                }
            }

            Part2(files, spaces);

        }

        private static void Part2(List<File> files, List<int> spacesRaw)
        {
            var diskMap = new List<int>();
            var spaces = new List<Space>();

            for (int i = 0; i < files.Count; i++)
            {
                var nextFile = files[i];
                nextFile.StartsAt = diskMap.Count;
                for (int j = 0; j < nextFile.Size; j++)
                {
                    diskMap.Add(nextFile.Id);
                }

                if (i >= spacesRaw.Count)
                {
                    continue;
                }

                var nextSpace = spacesRaw[i];
                spaces.Add(new Space()
                {
                    Size = nextSpace,
                    StartsAt = diskMap.Count,
                });

                for (int j = 0; j < nextSpace; j++)
                {

                    diskMap.Add(0);
                }
            }

            var checkSum = BigInteger.Zero;
            for (int i = 0; i < diskMap.Count; i++)
            {
                checkSum += diskMap[i] * i;
            }
            Console.WriteLine($"Initial diskmap: {string.Join(" ", diskMap)}");
            Console.WriteLine($"Checksum: {checkSum}");

            spaces = spaces.Where(space => space.Size > 0).OrderBy(space => space.StartsAt).ToList();

            var largestSpace = spaces.Select(x => x.Size).Max();

            while (files.Count > 0)
            {
                var lastFile = files.Last();
                var movedSuccessully = false;

                if (lastFile.Size > largestSpace)
                {
                    files.RemoveAt(files.Count - 1);
                    continue;
                }

                for (int j = 0; j < spaces.Count; j++)
                {
                    if (spaces[j].StartsAt > lastFile.StartsAt)
                    {
                        break;
                    }

                    if (spaces[j].Size >= lastFile.Size)
                    {
                        spaces[j].Size -= lastFile.Size;
                        for (int k = 0; k < lastFile.Size; k++)
                        {
                            diskMap[spaces[j].StartsAt + k] = lastFile.Id;
                            diskMap[lastFile.StartsAt + k] = 0;
                        }

                        spaces[j].StartsAt += lastFile.Size;

                        if (spaces[j].Size == 0)
                        {
                            spaces.RemoveAt(j);
                        }

                        movedSuccessully = true;

                        break;
                    }
                }

                if (!movedSuccessully)
                {
                    largestSpace = lastFile.Size - 1;
                }

                files.RemoveAt(files.Count - 1);
            }

            checkSum = BigInteger.Zero;
            for (int i = 0; i < diskMap.Count; i++)
            {
                checkSum += diskMap[i] * i;
            }
            Console.WriteLine($"Checksum: {checkSum}");
        }

        private static void Part1(List<File> files, List<int> spaces)
        {
            var diskMap = new List<int>();

            while (files.Count > 0)
            {
                var nextFile = files[0];
                files.RemoveAt(0);

                var nextSpace = spaces[0];
                spaces.RemoveAt(0);

                for (int i = 0; i < nextFile.Size; i++)
                {
                    diskMap.Add(nextFile.Id);
                }

                for (int i = 0; i < nextSpace; i++)
                {
                    while (files.Count > 0 && files.Last().Size == 0)
                    {
                        files.RemoveAt(files.Count - 1);
                    }

                    if (files.Count > 0 && files.Last().Size > 0)
                    {
                        var lastFile = files.Last();
                        lastFile.Size--;
                        diskMap.Add(lastFile.Id);
                    }
                }
            }

            var checkSum = BigInteger.Zero;
            for (int i = 0; i < diskMap.Count; i++)
            {
                checkSum += diskMap[i] * i;
            }

            //Console.WriteLine($"Diskmap: {string.Join("", diskMap)}");
            Console.WriteLine($"Checksum: {checkSum}");
        }
    }
}
