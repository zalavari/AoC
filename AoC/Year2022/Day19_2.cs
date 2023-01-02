using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    internal class Day19_2
    {

        private class State
        {
            public int[] Producers = new int[] { 1, 0, 0, 0 };
            public int[] Inventory = new int[] { 0, 0, 0, 0 };
            public int Time = 0;

            public override bool Equals(object obj)
            {
                var other = obj as State;
                return Inventory[0] == other.Inventory[0]
                    && Inventory[1] == other.Inventory[1]
                    && Inventory[2] == other.Inventory[2]
                    && Inventory[3] == other.Inventory[3]
                    && Producers[0] == other.Producers[0]
                    && Producers[1] == other.Producers[1]
                    && Producers[2] == other.Producers[2]
                    && Producers[3] == other.Producers[3];
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Producers, Inventory);
            }
        }



        public void Solve(string path)
        {
            Console.WriteLine(path);

            var lines = File.ReadAllLines(path);
            var bluePrints = new List<List<List<int>>>();

            foreach (var line in lines)
            {
                //Regex pattern = new Regex(@"Valve (?<name>\w+) has flow rate=(?<flowrate>\d+); tunnel(s?) lead(s?) to valve(s?) (?<tunnels>(.*))");
                Regex pattern = new Regex(@"Blueprint (?<number>\d+): Each ore robot costs (?<c00>\d+) ore. Each clay robot costs (?<c10>\d+) ore. Each obsidian robot costs (?<c20>\d+) ore and (?<c21>\d+) clay. Each geode robot costs (?<c30>\d+) ore and (?<c32>\d+) obsidian.");
                Match match = pattern.Match(line);
                var costTable = new List<List<int>>();
                for (int botType = 0; botType < 4; botType++)
                {
                    costTable.Add(new List<int>());
                    for (int resourceType = 0; resourceType < 4; resourceType++)
                    {
                        var value = match.Groups[$"c{botType}{resourceType}"].Value;
                        if (string.IsNullOrEmpty(value))
                            costTable[botType].Add(0);
                        else
                            costTable[botType].Add(int.Parse(value));
                    }
                }

                bluePrints.Add(costTable);
                //for (int i = 0; i < 4; i++)
                //{
                //    Console.WriteLine();
                //    for (int j = 0; j < 4; j++)
                //    {
                //        Console.Write(costTable[i][j] + " ");
                //    }
                //}
            }

            var endStates = new List<State>();
            var reachableStates = new Dictionary<State, int>();

            int timeLimit = 24;
            foreach (var bluePrint in bluePrints)
            {
                var queue = new Queue<State>();
                queue.Enqueue(new State());

                while (queue.Any())
                {
                    var state = queue.Dequeue();

                    //If we would wait..
                    var inventoryAtTheEnd = state.Inventory.Zip(state.Producers, (a, b) => a + (timeLimit - state.Time) * b).ToList();
                    endStates.Add(new State()
                    {
                        Producers = state.Producers,
                        Inventory = inventoryAtTheEnd.ToArray(),
                        Time = timeLimit,
                    });

                    //Try to construct a bot asap
                    for (int botType = 0; botType < 4; botType++)
                    {
                        if (bluePrint[botType].Zip(inventoryAtTheEnd, (a, b) => a > b).Any(b => b))
                            continue;

                        //var newInventory = new List<int>(state.Inventory);
                        var newTime = state.Time;

                        var newInventory = state.Inventory.Zip(bluePrint[botType], (a, b) => a - b).ToArray();

                        //var requiredTime = -1*newInventory.Zip(state.Producers, (a, b) => (a-b+1) / b).Min();
                        //if (requiredTime < 0)
                        //    requiredTime = 0;

                        //newInventory = newInventory.Zip(state.Producers, (a, b) => a + b).ToArray();
                        //newTime += requiredTime;

                        while (newInventory.Any(a => a < 0))
                        {
                            // Can be improved
                            newInventory = newInventory.Zip(state.Producers, (a, b) => a + b).ToArray();
                            newTime++;
                        }


                        newInventory = newInventory.Zip(state.Producers, (a, b) => a + b).ToArray();
                        newTime++;

                        var nextState = new State()
                        {
                            Producers = new List<int>(state.Producers).ToArray(),
                            Inventory = newInventory.ToArray(),
                            Time = newTime,
                        };
                        nextState.Producers[botType]++;



                        queue.Enqueue(nextState);
                        //var s = reachableStates.TryAdd(nextState, newTime);

                        //if (s)
                        //{
                        //    queue.Enqueue(nextState);
                        //}
                        //else if (reachableStates[nextState] > newTime)
                        //{
                        //    queue.Enqueue(nextState);
                        //    reachableStates[nextState] = newTime;
                        //}
                    }

                    //Console.WriteLine($"State processed: Time {state.Time}, Bots: {string.Join(", ",state.Producers)}");
                }
                Console.WriteLine(endStates.Max(a => a.Inventory[3]));
            }

            Console.WriteLine($"resultB: {0}");
            Console.WriteLine();
        }
    }
}