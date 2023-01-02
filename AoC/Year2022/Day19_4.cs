using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    internal class Day19_4
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
            var answer1 = 0;
            var blueprintNumber = 1;

            int timeLimit = 24;
            foreach (var bluePrint in bluePrints)
            {

                var reachableStatesByTime = new List<List<State>>();

                reachableStatesByTime.Add(new List<State>());
                for (int time = 0; time < timeLimit; time++)
                {
                    reachableStatesByTime.Add(new List<State>());
                }
                reachableStatesByTime.First().Add(new State());

                for (int time = 0; time < timeLimit; time++)
                {

                    foreach (State state in reachableStatesByTime[time])
                    {

                        //If we would just wait..
                        var inventoryAtTheEnd = new int[] {
                        state.Inventory[0] + (timeLimit - state.Time) * state.Producers[0],
                        state.Inventory[1] + (timeLimit - state.Time) * state.Producers[1],
                        state.Inventory[2] + (timeLimit - state.Time) * state.Producers[2],
                        state.Inventory[3] + (timeLimit - state.Time) * state.Producers[3],
                        };

                        endStates.Add(new State()
                        {
                            Producers = state.Producers,
                            Inventory = inventoryAtTheEnd,
                            Time = timeLimit,
                        });

                        //Try to construct a bot asap
                        for (int botType = 0; botType < 4; botType++)
                        {
                            if (bluePrint[botType][0] > inventoryAtTheEnd[0] ||
                                bluePrint[botType][1] > inventoryAtTheEnd[1] ||
                                bluePrint[botType][2] > inventoryAtTheEnd[2] ||
                                bluePrint[botType][3] > inventoryAtTheEnd[3])
                                continue;

                            var newTime = state.Time;

                            var newInventory = new int[] {
                            state.Inventory[0] - bluePrint[botType][0],
                            state.Inventory[1] - bluePrint[botType][1],
                            state.Inventory[2] - bluePrint[botType][2],
                            state.Inventory[3] - bluePrint[botType][3],
                        };

                            var requiredTime = 0;
                            for (int i = 0; i < 4; i++)
                            {
                                if (state.Producers[i] == 0)
                                    continue;

                                var requiredTimeForResource = -1 * (newInventory[i] - state.Producers[i] + 1) / state.Producers[i];
                                if (requiredTime < requiredTimeForResource)
                                    requiredTime = requiredTimeForResource;
                            }

                            requiredTime++;

                            for (int i = 0; i < 4; i++)
                            {
                                newInventory[i] += requiredTime * state.Producers[i];
                            }

                            newTime += requiredTime;

                            //while (newInventory.Any(a => a < 0))
                            //{
                            //    // Can be improved
                            //    newInventory[0] += state.Producers[0];
                            //    newInventory[1] += state.Producers[1];
                            //    newInventory[2] += state.Producers[2];
                            //    newInventory[3] += state.Producers[3];
                            //    newTime++;
                            //}

                            //newInventory[0] += state.Producers[0];
                            //newInventory[1] += state.Producers[1];
                            //newInventory[2] += state.Producers[2];
                            //newInventory[3] += state.Producers[3];
                            //newTime++;

                            if (newTime > timeLimit)
                                continue;

                            var nextState = new State()
                            {
                                Producers = new List<int>(state.Producers).ToArray(),
                                Inventory = newInventory,
                                Time = newTime,
                            };
                            nextState.Producers[botType]++;

                            if (reachableStatesByTime[newTime].Any(s =>
                            s.Producers[0] >= nextState.Producers[0] &&
                            s.Producers[1] >= nextState.Producers[1] &&
                            s.Producers[2] >= nextState.Producers[2] &&
                            s.Producers[3] >= nextState.Producers[3] &&
                            s.Inventory[0] >= nextState.Inventory[0] &&
                            s.Inventory[1] >= nextState.Inventory[1] &&
                            s.Inventory[2] >= nextState.Inventory[2] &&
                            s.Inventory[3] >= nextState.Inventory[3]
                                ))
                                continue;

                            reachableStatesByTime[newTime].Add(nextState);
                        }

                    }
                    //Console.WriteLine($"State processed: Time {state.Time}, Bots: {string.Join(", ",state.Producers)}");
                    for (int time2 = 0; time2 < timeLimit; time2++)
                    {
                        Console.WriteLine($"{time}-{time2}-{reachableStatesByTime[time2].Count()}");
                    }

                }
                var maxObsi = endStates.Max(a => a.Inventory[3]);
                Console.WriteLine($"{blueprintNumber}: {maxObsi}");
                answer1 += blueprintNumber++ * maxObsi;


            }

            Console.WriteLine($"resultA: {answer1}");
            Console.WriteLine($"resultB: {0}");
            Console.WriteLine();
        }
    }
}