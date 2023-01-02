using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC.Year2022
{
    internal class Day06 : ISolvable
    {

        public void Solve(string path)
        {
            var lines = File.ReadAllLines(path).ToList();

            var str = lines[0];
            var result = 0;
            var N = 14;

            for (int i = N - 1; i < str.Length; i++)
            {
                var set = new HashSet<char>();
                for (int j = 0; j < N; j++)
                    set.Add(str[i - j]);

                if (set.Count == N)
                {
                    result = i + 1;
                    break;
                }
            }

            Console.WriteLine(path);

            Console.WriteLine($"resultA: {result}");

            Console.WriteLine();
        }

    }
}
