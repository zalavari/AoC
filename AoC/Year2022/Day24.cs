using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{


    internal class Day24 : ISolvable
    {
        [Flags]
        private enum Blizzard
        {
            None = 0,
            Up = 1 << 1,
            Down = 1 << 2,
            Left = 1 << 3,
            Right = 1 << 4,
        }

        private class State
        {
            public int Y { get; set; }
            public int X { get; set; }
            public int Time { get; set; }
        }
        public void Solve(string path)
        {
            Console.WriteLine(path);


            var lines = File.ReadAllLines(path);

            var height = lines.Length - 2;
            var width = lines.First().Length - 2;
            var initValley = new List<List<Blizzard>>();

            var period = GetLCM(height, width);

            for (int i = 0; i < height; i++)
            {
                initValley.Add(new List<Blizzard>());
                var line = lines[i + 1];
                for (int j = 0; j < width; j++)
                {
                    var x = j;
                    var y = i;
                    Blizzard b = Blizzard.None;
                    if (line[j + 1] == '^')
                        b |= Blizzard.Up;

                    if (line[j + 1] == 'v')
                        b |= Blizzard.Down;

                    if (line[j + 1] == '<')
                        b |= Blizzard.Left;

                    if (line[j + 1] == '>')
                        b |= Blizzard.Right;

                    initValley[i].Add(b);
                }
            }

            var valleys = new List<List<List<Blizzard>>>();
            valleys.Add(initValley);
            //PrintValley(height, width, initValley);
            for (int i = 1; i < period; i++)
            {
                valleys.Add(GenerateNextMinuteValley(height, width, valleys.Last()));

                //PrintValley(height, width, valleys.Last());
            }
            //PrintValley(height, width, GenerateNextMinuteValley(height, width, valleys.Last()));

            var initState = new State()
            {
                X = 0,
                Y = -1,
                Time = 0,
            };

            var s1 = SearchPath(height, width, period, initState, width - 1, height - 1, valleys);

            s1.Time++;
            s1.Y++;

            var s2 = SearchPath(height, width, period, s1, 0, 0, valleys);

            s2.Time++;
            s2.Y--;

            var s3 = SearchPath(height, width, period, s2, width - 1, height - 1, valleys);



            Console.WriteLine($"rounds: {0}");
            Console.WriteLine();
        }

        private State SearchPath(int height, int width, int period, State initState, int endx, int endy, List<List<List<Blizzard>>> valleys)
        {
            var reachedStates = new HashSet<(int, int, int)>();

            var queue = new Queue<State>();
            queue.Enqueue(initState);

            reachedStates.Add((initState.Y, initState.X, initState.Time));

            while (true)
            {
                var state = queue.Dequeue();
                var nextValley = valleys[(state.Time + 1) % period];

                if (state.X == endx && state.Y == endy)
                {
                    Console.WriteLine($"Path found in time {state.Time + 1}");
                    return state;
                }

                //Special case, when we are just departing
                if (state.Y == -1 || state.Y == height)
                {
                    //Stay
                    var nextState0 = new State()
                    {
                        Y = state.Y,
                        X = state.X,
                        Time = state.Time + 1,
                    };

                    if (!reachedStates.Contains((nextState0.Y, nextState0.X, nextState0.Time % period)))
                    {
                        reachedStates.Add((nextState0.Y, nextState0.X, nextState0.Time % period));
                        queue.Enqueue(nextState0);
                    }

                    //Move down
                    if (state.Y + 1 < height && nextValley[state.Y + 1][state.X] == Blizzard.None)
                    {
                        var nextState = new State()
                        {
                            Y = state.Y + 1,
                            X = state.X,
                            Time = state.Time + 1,
                        };

                        if (!reachedStates.Contains((nextState.Y, nextState.X, nextState.Time % period)))
                        {
                            reachedStates.Add((nextState.Y, nextState.X, nextState.Time % period));
                            queue.Enqueue(nextState);
                        }
                    }

                    //Move up
                    if (state.Y - 1 >= 0 && nextValley[state.Y - 1][state.X] == Blizzard.None)
                    {
                        var nextState = new State()
                        {
                            Y = state.Y - 1,
                            X = state.X,
                            Time = state.Time + 1,
                        };

                        if (!reachedStates.Contains((nextState.Y, nextState.X, nextState.Time % period)))
                        {
                            reachedStates.Add((nextState.Y, nextState.X, nextState.Time % period));
                            queue.Enqueue(nextState);
                        }
                    }

                    continue;
                }

                //Stay
                if (nextValley[state.Y][state.X] == Blizzard.None)
                {
                    var nextState = new State()
                    {
                        Y = state.Y,
                        X = state.X,
                        Time = state.Time + 1,
                    };

                    if (!reachedStates.Contains((nextState.Y, nextState.X, nextState.Time % period)))
                    {
                        reachedStates.Add((nextState.Y, nextState.X, nextState.Time % period));
                        queue.Enqueue(nextState);
                    }
                }

                //Move down
                if (state.Y + 1 < height && nextValley[state.Y + 1][state.X] == Blizzard.None)
                {
                    var nextState = new State()
                    {
                        Y = state.Y + 1,
                        X = state.X,
                        Time = state.Time + 1,
                    };

                    if (!reachedStates.Contains((nextState.Y, nextState.X, nextState.Time % period)))
                    {
                        reachedStates.Add((nextState.Y, nextState.X, nextState.Time % period));
                        queue.Enqueue(nextState);
                    }
                }

                //Move up
                if (state.Y - 1 >= 0 && nextValley[state.Y - 1][state.X] == Blizzard.None)
                {
                    var nextState = new State()
                    {
                        Y = state.Y - 1,
                        X = state.X,
                        Time = state.Time + 1,
                    };

                    if (!reachedStates.Contains((nextState.Y, nextState.X, nextState.Time % period)))
                    {
                        reachedStates.Add((nextState.Y, nextState.X, nextState.Time % period));
                        queue.Enqueue(nextState);
                    }
                }

                //Move left
                if (state.X - 1 >= 0 && nextValley[state.Y][state.X - 1] == Blizzard.None)
                {
                    var nextState = new State()
                    {
                        Y = state.Y,
                        X = state.X - 1,
                        Time = state.Time + 1,
                    };

                    if (!reachedStates.Contains((nextState.Y, nextState.X, nextState.Time % period)))
                    {
                        reachedStates.Add((nextState.Y, nextState.X, nextState.Time % period));
                        queue.Enqueue(nextState);
                    }
                }

                //Move right
                if (state.X + 1 < width && nextValley[state.Y][state.X + 1] == Blizzard.None)
                {
                    var nextState = new State()
                    {
                        Y = state.Y,
                        X = state.X + 1,
                        Time = state.Time + 1,
                    };

                    if (!reachedStates.Contains((nextState.Y, nextState.X, nextState.Time % period)))
                    {
                        reachedStates.Add((nextState.Y, nextState.X, nextState.Time % period));
                        queue.Enqueue(nextState);
                    }
                }

            }
        }

        private List<List<Blizzard>> GenerateNextMinuteValley(int height, int width, List<List<Blizzard>> valley)
        {
            var nextValley = new List<List<Blizzard>>();
            for (int i = 0; i < height; i++)
            {
                nextValley.Add(new List<Blizzard>());
                for (int j = 0; j < width; j++)
                {
                    nextValley[i].Add(Blizzard.None);
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (valley[i][j].HasFlag(Blizzard.Up))
                        nextValley[(i - 1 + height) % height][j] |= Blizzard.Up;


                    if (valley[i][j].HasFlag(Blizzard.Down))
                        nextValley[(i + 1 + height) % height][j] |= Blizzard.Down;

                    if (valley[i][j].HasFlag(Blizzard.Left))
                        nextValley[i][(j - 1 + width) % width] |= Blizzard.Left;

                    if (valley[i][j].HasFlag(Blizzard.Right))
                        nextValley[i][(j + 1 + width) % width] |= Blizzard.Right;
                }
            }

            return nextValley;

        }

        private void PrintValley(int height, int width, List<List<Blizzard>> valley)
        {

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var c = 0;
                    if (valley[i][j].HasFlag(Blizzard.Up))
                        c++;

                    if (valley[i][j].HasFlag(Blizzard.Down))
                        c++;

                    if (valley[i][j].HasFlag(Blizzard.Left))
                        c++;

                    if (valley[i][j].HasFlag(Blizzard.Right))
                        c++;

                    Console.Write(c == 0 ? "." : c.ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine();


        }

        //Ineffective, but it is enough for us atm
        private int GetLCM(int a, int b)
        {
            int c = 1;
            while (c % a != 0 || c % b != 0) c++;

            return c;
        }



    }
}