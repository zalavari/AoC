using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{

    internal class Day07 : ISolvable
    {

        private class Filee
        {
            public string Name { get; set; }
            public int Size { get; set; }
        }

        private class Directory
        {
            public string Name { get; set; }
            public List<Directory> Subdirectories { get; set; } = new List<Directory>();
            public List<Filee> Files { get; set; } = new List<Filee>();

            public int GetSize()
            {
                return Subdirectories.Sum(dir => dir.GetSize()) + Files.Sum(file => file.Size);
            }

        }

        public void Solve(string path)
        {
            var lines = System.IO.File.ReadAllLines(path).ToList();

            var directories = new Dictionary<string, Directory>();
            var currentPath = new List<string>();

            int i = 0;
            while (i < lines.Count())
            {
                var cmd = lines[i++].Split(" ");


                var currentKey = string.Join("/", currentPath);
                directories.TryGetValue(currentKey, out var currentDir);

                if (cmd[0] != "$")
                    throw new InvalidOperationException();

                if (cmd[1] == "cd")
                {
                    if (cmd[2] == "..")
                    {
                        currentPath.RemoveAt(currentPath.Count() - 1);
                    }
                    else
                    {
                        currentPath.Add(cmd[2]);
                        var key = string.Join("/", currentPath);
                        Directory dir;
                        directories.TryGetValue(key, out dir);
                        if (dir == null)
                        {
                            dir = new Directory()
                            {
                                Name = cmd[2],
                            };
                            directories.Add(key, dir);
                            currentDir?.Subdirectories.Add(dir);
                        }
                    }
                }
                else if (cmd[1] == "ls")
                {

                    while (i < lines.Count() && lines[i].Split(" ")[0] != "$")
                    {
                        var subItem = lines[i].Split(" ");

                        if (subItem[0] == "dir")
                        {
                            var path2 = new List<string>(currentPath);
                            path2.Add(subItem[1]);
                            var key2 = string.Join("/", path2);

                            Directory dir;
                            directories.TryGetValue(key2, out dir);
                            if (dir == null)
                            {
                                dir = new Directory()
                                {
                                    Name = subItem[1],
                                };
                                directories.Add(key2, dir);
                                currentDir.Subdirectories.Add(dir);
                            }

                        }
                        else
                        {
                            var file = new Filee()
                            {
                                Name = subItem[1],
                                Size = int.Parse(subItem[0]),
                            };
                            currentDir.Files.Add(file);
                        }

                        i++;
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }


            var totalSize = directories["/"].GetSize();
            var needFreeUp = totalSize - 40000000;

            var result = 0;
            var resultB = totalSize;
            foreach (var kvp in directories)
            {
                var size = kvp.Value.GetSize();
                // Console.WriteLine($"Key:{kvp.Key}, Size:{size}");
                if (size < 100000)
                    result += size;

                if (size > needFreeUp && size < resultB)
                    resultB = size;
            }



            Console.WriteLine(path);

            Console.WriteLine($"totalSize: {totalSize}");
            Console.WriteLine($"needFreeUp: {needFreeUp}");
            Console.WriteLine($"resultA: {result}");
            Console.WriteLine($"resultB: {resultB}");

            Console.WriteLine();
        }



    }
}
