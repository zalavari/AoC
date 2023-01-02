using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    internal class Day20 : ISolvable
    {



        public void Solve(string path)
        {
            Console.WriteLine(path);

            var key = 811589153;
            var timesToMix = 10;
            var numbers = File.ReadAllLines(path).Select(long.Parse).Select(x => x * key).ToList();
            var places = Enumerable.Range(0, numbers.Count).ToList();
            var N = places.Count;

            for (int mix = 0; mix < timesToMix; mix++)
            {
                for (int i = 0; i < N; i++)
                {
                    var place = places.FindIndex(x => x == i);

                    var number = numbers[place];

                    var newPlace = (int)((place + number) % (N - 1));
                    if (newPlace < 0)
                        newPlace += N - 1;



                    for (int j = place; j < newPlace; j++)
                    {
                        numbers[j] = numbers[j + 1];
                        places[j] = places[j + 1];
                    }

                    for (int j = place; j > newPlace; j--)
                    {
                        numbers[j] = numbers[j - 1];
                        places[j] = places[j - 1];
                    }

                    numbers[newPlace] = number;
                    places[newPlace] = i;

                    //Console.WriteLine();
                    //Console.WriteLine($"Iteration: {i}");
                    //Console.WriteLine($"Numbers: {string.Join(", ", numbers)}");
                    //Console.WriteLine($"Places: {string.Join(", ", places)}");

                }
            }


            var zeroPlace = numbers.FindIndex(x => x == 0);

            var num1000 = numbers[(zeroPlace + 1000) % N];
            var num2000 = numbers[(zeroPlace + 2000) % N];
            var num3000 = numbers[(zeroPlace + 3000) % N];




            Console.WriteLine($"resultA: {num1000}+{num2000}+{num3000}={num1000 + num2000 + num3000}");


            Console.WriteLine($"resultB: {0}");
            Console.WriteLine();
        }
    }
}