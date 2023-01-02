﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    internal class Day16_1
    {
        public void Solve(string path)
        {
            Console.WriteLine(path);

            var map = new Dictionary<string, Valve>();
            var lines = File.ReadAllLines(path).ToList();

            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"Valve (?<name>\w+) has flow rate=(?<flowrate>\d+); tunnel(s?) lead(s?) to valve(s?) (?<tunnels>(.*))");
                Match match = pattern.Match(line);
                string name = match.Groups["name"].Value;
                int flowrate = int.Parse(match.Groups["flowrate"].Value);
                List<string> tunnels = match.Groups["tunnels"].Value.Split(", ").ToList();

                map.Add(name, new Valve() { Name = name, FlowRate = flowrate, Tunnels = tunnels });
            }

            var result = 0;

            var statesByTimesByPosition = new List<Dictionary<string, List<State>>>();

            var initState = new State()
            {
                Position = "AA",
                ReleasedPressure = 0,
            };


            var initDict = new Dictionary<string, List<State>>();
            initDict.Add(initState.Position, new List<State>() { initState });
            statesByTimesByPosition.Add(initDict);

            for (int i = 0; i <= 30; i++)
            {
                var nextDictionary = new Dictionary<string, List<State>>();
                statesByTimesByPosition.Add(nextDictionary);
                foreach (var (position, states) in statesByTimesByPosition[i])
                {
                    foreach (var state in states)
                    {

                        // We can open the current Valve if it is not opened yet
                        if (map[state.Position].FlowRate > 0 && !state.Opened.Contains(state.Position))
                        {
                            var nextState = new State()
                            {
                                Position = state.Position,
                                Opened = new List<string>(state.Opened),
                                ReleasedPressure = state.ReleasedPressure + map[state.Position].FlowRate * (30 - i - 1),
                            };
                            nextState.Opened.Add(state.Position);

                            AddToDictionary(nextDictionary, nextState);
                        }

                        // We can move to an other valve
                        foreach (var nextPosition in map[state.Position].Tunnels)
                        {
                            var nextState = new State()
                            {
                                Position = nextPosition,
                                Opened = new List<string>(state.Opened),
                                ReleasedPressure = state.ReleasedPressure,
                            };

                            AddToDictionary(nextDictionary, nextState);
                        }
                    }

                }
                Console.WriteLine($"All possible states at the end of minute {i + 1}: {statesByTimesByPosition.Last().Sum(kvp => kvp.Value.Count())}");
                Console.WriteLine($"Max pressure released at the end of minute {i + 1}: {statesByTimesByPosition.Last().Max(kvp => kvp.Value.Max(state => state.ReleasedPressure))}");

            }

            for (int i = 0; i < statesByTimesByPosition.Count; i++)
            {
                var stateByTimes = statesByTimesByPosition[i];
                Console.WriteLine($"Max pressure released at the end of minute {i}: {stateByTimes.Max(kvp => kvp.Value.Max(state => state.ReleasedPressure))}");

            }

            Console.WriteLine();
            Console.WriteLine($"result: {result}");
            Console.WriteLine();
        }

        private void AddToList(List<State> states, State state)
        {
            foreach (var s in states)
            {
                if (s.Position == state.Position && s.Opened.Count() == state.Opened.Count() && s.Opened.Intersect(state.Opened).Count() == s.Opened.Count())
                {
                    s.ReleasedPressure = Math.Max(s.ReleasedPressure, state.ReleasedPressure);
                    return;
                }
            }
            states.Add(state);
        }

        private void AddToDictionary(Dictionary<string, List<State>> dict, State state)
        {
            dict.TryGetValue(state.Position, out var states);
            if (states == null)
            {
                states = new List<State>();
                dict.Add(state.Position, states);
            }

            foreach (var s in states)
            {
                if (s.Opened.Count() == state.Opened.Count() && s.Opened.Intersect(state.Opened).Count() == s.Opened.Count())
                {
                    s.ReleasedPressure = Math.Max(s.ReleasedPressure, state.ReleasedPressure);
                    return;
                }
            }
            states.Add(state);
        }

        private class Valve
        {
            public string Name;
            public int FlowRate;
            public List<string> Tunnels = new List<string>();
        }

        private class State
        {
            public List<string> Opened { get; set; } = new List<string>();
            public string Position { get; set; } = "";
            public int ReleasedPressure { get; set; } = 0;
        }

    }
}
