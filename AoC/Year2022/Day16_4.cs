using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    //Fast solution to part one
    internal class Day16_4
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

            foreach (var valve in valves.Where(v => v.FlowRate > 0))
            {
                var queue = new Queue<int>();
                queue.Enqueue(valve.Id);

                var set = new HashSet<int>();
                set.Add(valve.Id);

                valve.Distance.Add(valve.Id, 0);

                while (queue.Any())
                {
                    var current = queue.Dequeue();
                    foreach (var neighbour in valves[current].Tunnels)
                    {
                        if (!set.Contains(neighbour))
                        {
                            queue.Enqueue(neighbour);
                            set.Add(neighbour);
                            valves[neighbour].Distance.Add(valve.Id, valves[current].Distance[valve.Id] + 1);
                        }
                    }
                }
            }

            var initPos = valves.Where(v => v.Name == "AA").First().Id;


            var statesByTime = new List<List<State>>();
            var N = 30;
            for (int i = 0; i < N; i++)
            {
                statesByTime.Add(new List<State>());
            }

            var initState = new State()
            {
                Visited = new HashSet<int> { },
                LastVisited = initPos,
            };

            statesByTime[0].Add(initState);
            var maxPressure = 0;

            for (int time = 0; time < N; time++)
            {
                var maxPressureAtTime = statesByTime[time].Count() == 0 ? 0 : statesByTime[time].Max(s => s.ReleasedPressure);
                if (maxPressure < maxPressureAtTime)
                    maxPressure = maxPressureAtTime;
                Console.WriteLine($"All possible states at the end of minute {time}: {statesByTime[time].Count()}");
                Console.WriteLine($"Max pressure released at the end of minute {time}: {maxPressureAtTime}");
                foreach (var state in statesByTime[time])
                {
                    var currentNode = valves[state.LastVisited];
                    foreach (var (id, dist) in currentNode.Distance)
                    {
                        if (time + dist + 1 < N && !state.Visited.Contains(id))
                        {
                            var newState = new State()
                            {
                                Visited = new HashSet<int>(state.Visited),
                                LastVisited = id,
                                ReleasedPressure = state.ReleasedPressure + valves[id].FlowRate * (N - (time + dist + 1)),
                            };
                            newState.Visited.Add(id);
                            statesByTime[time + dist + 1].Add(newState);
                        }
                    }
                }
            }

            var result = 0;


            Console.WriteLine();
            Console.WriteLine($"result: {maxPressure}");
            Console.WriteLine();
        }

        private class Valve
        {
            public string Name;
            public int Id;
            public int FlowRate;
            public List<int> Tunnels = new List<int>();
            public List<string> TunnelsString = new List<string>();

            public Dictionary<int, int> Distance = new Dictionary<int, int>();
        }

        private class State
        {
            public HashSet<int> Visited { get; set; }
            public int LastVisited { get; set; }
            public int ReleasedPressure { get; set; }
        }

    }
}
