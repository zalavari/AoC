using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC.Year2024
{
    internal class Day12 : ISolvable
    {
        private List<(int, int)> directions = new List<(int, int)> { (0, 1), (1, 0), (0, -1), (-1, 0) };
        private List<List<bool>> visited;

        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = System.IO.File.ReadAllLines(path).ToList();

            var map = lines.Select(line => line.ToCharArray().ToList()).ToList();
            visited = Enumerable.Repeat(0, map.Count).Select(_ => Enumerable.Repeat(false, map[0].Count).ToList()).ToList();
            var allPrices = new List<int>();

            for (int row = 0; row < map.Count; row++)
            {
                for (int col = 0; col < map[0].Count; col++)
                {
                    if (!visited[row][col])
                    {
                        Console.WriteLine($"Region at ({row},{col}) (type: {map[row][col]})");
                        var price = VisitRegion(map, row, col);
                        allPrices.Add(price);
                    }
                }
            }

            Console.WriteLine($"Sum of all prices: {allPrices.Sum()}");
            Console.WriteLine();

            // 859022 - too high
        }

        private int VisitRegion(List<List<char>> map, int row, int col)
        {
            var perimeter = 0;
            var area = 0;
            var innerCorners = 0;
            var outerCorners = 0;
            var queue = new Queue<Point>();
            queue.Enqueue(new Point() { X = row, Y = col });
            visited[row][col] = true;
            var type = map[row][col];

            while (queue.Any())
            {
                var point = queue.Dequeue();
                area++;

                for (int dir = 0; dir < 4; dir++)
                {
                    var dir2 = (dir + 1) % 4;

                    var n1Row = point.X + directions[dir].Item1;
                    var n1Col = point.Y + directions[dir].Item2;

                    var n2Row = point.X + directions[dir2].Item1;
                    var n2Col = point.Y + directions[dir2].Item2;

                    var diagRow = point.X + directions[dir].Item1 + directions[dir2].Item1;
                    var diagCol = point.Y + directions[dir].Item2 + directions[dir2].Item2;

                    if (InBounds(map, n1Row, n1Col) && InBounds(map, n2Row, n2Col) && InBounds(map, diagRow, diagCol))
                    {
                        if (map[n1Row][n1Col] == type && map[n2Row][n2Col] == type && map[diagRow][diagCol] != type)
                        {
                            Console.WriteLine($"({point.X},{point.Y}) is corner piece, because ({diagRow}{diagCol}) is a different type.");
                            innerCorners++;
                        }
                    }

                    if ((!InBounds(map, n1Row, n1Col) || map[n1Row][n1Col] != type) && (!InBounds(map, n2Row, n2Col) || map[n2Row][n2Col] != type))
                    {
                        outerCorners++;
                    }
                }


                foreach (var direction in directions)
                {
                    var newRow = point.X + direction.Item1;
                    var newCol = point.Y + direction.Item2;
                    if (InBounds(map, newRow, newCol) && !visited[newRow][newCol] && map[newRow][newCol] == type)
                    {
                        queue.Enqueue(new Point() { X = newRow, Y = newCol });
                        visited[newRow][newCol] = true;
                    }
                    else if (!InBounds(map, newRow, newCol) || map[newRow][newCol] != type)
                    {
                        perimeter++;
                    }
                }
            }

            var price = area * (innerCorners + outerCorners);
            Console.WriteLine($"Area: {area}");
            Console.WriteLine($"Perimeter: {perimeter}");
            Console.WriteLine($"Inner corners: {innerCorners}");
            Console.WriteLine($"Outer corners: {outerCorners}");
            Console.WriteLine($"Price: {price}");
            return price;
            //  return area * perimeter;
        }

        private static bool InBounds(List<List<char>> map, int row, int col)
        {
            return row >= 0 && row < map.Count && col >= 0 && col < map[0].Count;
        }
    }
}
