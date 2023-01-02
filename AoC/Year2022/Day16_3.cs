using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    //Works for second part but very slow
    internal class Day16_3
    {
        public void Solve(string path)
        {
            Console.WriteLine(path);

            var valves = new List<Valve>();
            var lines = File.ReadAllLines(path).ToList();

            var currentId = 0;
            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"Valve (?<name>\w+) has flow rate=(?<flowrate>\d+); tunnel(s?) lead(s?) to valve(s?) (?<tunnels>(.*))");
                Match match = pattern.Match(line);
                string name = match.Groups["name"].Value;
                int flowrate = int.Parse(match.Groups["flowrate"].Value);
                List<string> tunnels = match.Groups["tunnels"].Value.Split(", ").ToList();


                valves.Add(new Valve() { Name = name, Id = currentId, FlowRate = flowrate, TunnelsString = tunnels });
                currentId++;
            }

            foreach (var valve in valves)
            {
                valve.Tunnels = valve.TunnelsString.Select(tunnelString => valves.Where(v => v.Name == tunnelString).First().Id).ToList();
            }

            var result = 0;

            var statesByTimesByPosition = new List<Dictionary<(int, int), List<State>>>();

            var initPos = valves.Where(v => v.Name == "AA").First().Id;

            var initState = new State()
            {
                Position = (initPos, initPos),
                ReleasedPressure = 0,
                Opened = 0,
            };

            var initDict = new Dictionary<(int, int), List<State>>();
            initDict.Add(initState.Position, new List<State>() { initState });
            statesByTimesByPosition.Add(initDict);

            var N = 26;

            for (int i = 0; i < N; i++)
            {
                var nextDictionary = new Dictionary<(int, int), List<State>>();
                statesByTimesByPosition.Add(nextDictionary);
                foreach (var (position, states) in statesByTimesByPosition[i])
                {
                    foreach (var state in states)
                    {

                        // Open at first position
                        if (valves[state.Position.Item1].FlowRate > 0 && (state.Opened & (long)1 << state.Position.Item1) == 0)
                        {
                            // Open at second position
                            if (state.Position.Item1 != state.Position.Item2 && valves[state.Position.Item2].FlowRate > 0 && (state.Opened & (long)1 << state.Position.Item2) == 0)
                            {
                                var nextState = new State()
                                {
                                    Position = state.Position,
                                    Opened = state.Opened,
                                    ReleasedPressure = state.ReleasedPressure,
                                };
                                nextState.Opened |= (long)1 << state.Position.Item1;
                                nextState.Opened |= (long)1 << state.Position.Item2;

                                nextState.ReleasedPressure += valves[state.Position.Item1].FlowRate * (N - i - 1);
                                nextState.ReleasedPressure += valves[state.Position.Item2].FlowRate * (N - i - 1);

                                AddToDictionary(nextDictionary, nextState);
                            }

                            // Move on second position
                            foreach (var nextPosition2 in valves[state.Position.Item2].Tunnels)
                            {
                                var nextState = new State()
                                {
                                    Position = (state.Position.Item1, nextPosition2),
                                    Opened = state.Opened,
                                    ReleasedPressure = state.ReleasedPressure,
                                };

                                nextState.Opened |= (long)1 << state.Position.Item1;
                                nextState.ReleasedPressure += valves[state.Position.Item1].FlowRate * (N - i - 1);

                                AddToDictionary(nextDictionary, nextState);
                            }
                        }

                        // Move on first position
                        foreach (var nextPosition1 in valves[state.Position.Item1].Tunnels)
                        {

                            // Open at second position
                            if (valves[state.Position.Item2].FlowRate > 0 && (state.Opened & (long)1 << state.Position.Item2) == 0)
                            {
                                var nextState = new State()
                                {
                                    Position = (nextPosition1, state.Position.Item2),
                                    Opened = state.Opened,
                                    ReleasedPressure = state.ReleasedPressure,
                                };

                                nextState.Opened |= (long)1 << state.Position.Item2;
                                nextState.ReleasedPressure += valves[state.Position.Item2].FlowRate * (N - i - 1);

                                AddToDictionary(nextDictionary, nextState);
                            }

                            // Move on second position
                            foreach (var nextPosition2 in valves[state.Position.Item2].Tunnels)
                            {
                                var nextState = new State()
                                {
                                    Position = (nextPosition1, nextPosition2),
                                    Opened = state.Opened,
                                    ReleasedPressure = state.ReleasedPressure,
                                };

                                AddToDictionary(nextDictionary, nextState);
                            }
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

        private void AddToDictionary(Dictionary<(int, int), List<State>> dict, State state)
        {
            dict.TryGetValue(state.Position, out var states);
            if (states == null)
            {
                states = new List<State>();
                dict.Add(state.Position, states);
            }

            foreach (var s in states)
            {
                if (s.Opened == state.Opened)
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
            public int Id;
            public int FlowRate;
            public List<int> Tunnels = new List<int>();
            public List<string> TunnelsString = new List<string>();
            public long GetFlag() => (long)1 << Id;
        }

        private class State
        {
            public long Opened { get; set; } = 0;
            public (int, int) Position { get; set; }
            public int ReleasedPressure { get; set; } = 0;
        }

    }
}
