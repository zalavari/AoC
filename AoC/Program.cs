using System;

namespace AoC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Solve(19);
        }

        private static void SolveAll()
        {
            for (int number = 1; number < 25; number++)
            {
                Solve(number);
            }
        }

        private static void Solve(int number)
        {
            Type type = Type.GetType($"AoC.Year2023.Day{number.ToString("D2")}");
            ISolvable problem = (ISolvable)Activator.CreateInstance(type);
            problem.Solve(@$"C:\Users\marton.zalavari\source\repos\AoC\AoC\Year2023\input\input{number}ex.txt");
            problem.Solve(@$"C:\Users\marton.zalavari\source\repos\AoC\AoC\Year2023\input\input{number}.txt");
        }
    }
}
