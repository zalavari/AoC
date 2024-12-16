using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day02 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path);
            var reports = lines.Select(line => line.Split(" ").Select(int.Parse).ToList()).ToList();
            var countOfSafeReports = 0;
            var countOfSafeReportsWithDampener = 0;

            //foreach (var report in reports)
            //{
            //    if (report.First() > report.Last())
            //    {
            //        report.Reverse();
            //    }
            //}

            foreach (var report in reports)
            {
                if (IsSafe(report))
                {
                    countOfSafeReports++;
                }
            }

            foreach (var report in reports)
            {
                if (IsSafe(report))
                {
                    countOfSafeReportsWithDampener++;
                    continue;
                }

                for (int i = 0; i < report.Count; i++)
                {
                    var reportCopy = new List<int>(report);
                    reportCopy.RemoveAt(i);
                    if (IsSafe(reportCopy))
                    {
                        countOfSafeReportsWithDampener++;
                        break;
                    }
                }

                //// This should be faster for long reports, but it is not working for some reason
                //var reportReversed = new List<int>(report);
                //reportReversed.Reverse();
                //if (IsSafeWithDampener(report) || IsSafeWithDampener(reportReversed))
                //{
                //    countOfSafeReportsWithDampener++;
                //}
            }

            Console.WriteLine(countOfSafeReports);
            Console.WriteLine(countOfSafeReportsWithDampener);
        }

        private static bool IsSafeWithDampener(List<int> report)
        {
            var unsafeLevels = report.Where((number, level) => level < report.Count - 1 && (report[level + 1] - number < 1 || report[level + 1] - number > 3)).Select((number, level) => level).ToList();

            if (unsafeLevels.Count > 2)
            {
                return false;
            }

            var toAdd = unsafeLevels.Select(level => level + 1).ToList();
            unsafeLevels.AddRange(toAdd);
            unsafeLevels = unsafeLevels.Distinct().Where(level => level >= 0).ToList();

            foreach (var level in unsafeLevels)
            {
                var potentialSafeReport = new List<int>(report);
                potentialSafeReport.RemoveAt(level);
                if (IsSafe(potentialSafeReport))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsSafe(List<int> reportOriginal)
        {
            var report = new List<int>(reportOriginal);
            if (report.First() > report.Last())
            {
                report.Reverse();
            }

            if (report.Where((number, level) => level < report.Count - 1 && (report[level + 1] - number < 1 || report[level + 1] - number > 3)).Any())
            {
                return false;
            }

            return true;
        }
    }
}
