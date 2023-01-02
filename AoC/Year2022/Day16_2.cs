using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{

    //Works for first part, but it is a bit slow
    internal class Day16_2
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

            var statesByTimesByPosition = new List<Dictionary<int, List<State>>>();

            var initPos = valves.Where(v => v.Name == "AA").First().Id;

            var initState = new State()
            {
                Position = initPos,
                ReleasedPressure = 0,
                Opened = 0,
            };


            var initDict = new Dictionary<int, List<State>>();
            initDict.Add(initState.Position, new List<State>() { initState });
            statesByTimesByPosition.Add(initDict);

            for (int i = 0; i <= 30; i++)
            {
                var nextDictionary = new Dictionary<int, List<State>>();
                statesByTimesByPosition.Add(nextDictionary);
                foreach (var (position, states) in statesByTimesByPosition[i])
                {
                    foreach (var state in states)
                    {

                        // We can open the current Valve if it is not opened yet
                        if (valves[state.Position].FlowRate > 0 && (state.Opened & (long)1 << state.Position) == 0)
                        {

                            var nextState = new State()
                            {
                                Position = state.Position,
                                Opened = state.Opened,
                                ReleasedPressure = state.ReleasedPressure + valves[state.Position].FlowRate * (30 - i - 1),
                            };
                            nextState.Opened |= (long)1 << state.Position;

                            AddToDictionary(nextDictionary, nextState);
                        }

                        // We can move to an other valve
                        foreach (var nextPosition in valves[state.Position].Tunnels)
                        {
                            var nextState = new State()
                            {
                                Position = nextPosition,
                                Opened = state.Opened,
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
                if (s.Position == state.Position && s.Opened == state.Opened)
                {
                    s.ReleasedPressure = Math.Max(s.ReleasedPressure, state.ReleasedPressure);
                    return;
                }
            }
            states.Add(state);
        }

        private void AddToDictionary(Dictionary<int, List<State>> dict, State state)
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
            public int Position { get; set; } = 1;
            public int ReleasedPressure { get; set; } = 0;
        }

    }
}
