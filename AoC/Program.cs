using System;

namespace AoC
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SolveAll();
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
            Type type = Type.GetType($"AoC.Year2022.Day{number.ToString("D2")}");
            ISolvable problem = (ISolvable)Activator.CreateInstance(type);
            //problem.Solve(@$"C:\Users\marton.zalavari\Documents\AoC\input{number}ex.txt");
            problem.Solve(@$"C:\Users\marton.zalavari\source\repos\AoC_2022\AoC_2022\input{number}.txt");
        }
    }
}
