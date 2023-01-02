using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
    //Fast solution for second part
    internal class Day16 : ISolvable
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
            var N = 26;
            for (int i = 0; i < N; i++)
            {
                statesByTime.Add(new List<State>());
            }

            var initState = new State()
            {
                Visited = 0,
                LastVisited1 = initPos,
                LastVisited2 = initPos,
                JobEndsAt1 = 0,
                JobEndsAt2 = 0,
            };

            statesByTime[0].Add(initState);
            var maxPressure = 0;

            for (int time = 0; time < N; time++)
            {
                Console.WriteLine($"Count of states beginning the {time}th cycle: {statesByTime[time].Count}");
                var maxPressureAtTime = statesByTime[time].Count() == 0 ? 0 : statesByTime[time].Max(s => s.ReleasedPressure);
                if (maxPressure < maxPressureAtTime)
                    maxPressure = maxPressureAtTime;

                for (int i = 0; i < statesByTime[time].Count; i++)
                {
                    var state = statesByTime[time][i];
                    if (state.JobEndsAt1 == time)
                    {
                        var currentNode = valves[state.LastVisited1];
                        foreach (var (id, dist) in currentNode.Distance)
                        {
                            var jobEnds = time + dist + 1;
                            if (jobEnds < N && (state.Visited & (long)1 << id) == 0)
                            {
                                var newState = new State()
                                {
                                    Visited = state.Visited,
                                    LastVisited1 = id,
                                    LastVisited2 = state.LastVisited2,
                                    JobEndsAt1 = jobEnds,
                                    JobEndsAt2 = state.JobEndsAt2,
                                    ReleasedPressure = state.ReleasedPressure + valves[id].FlowRate * (N - jobEnds),
                                };
                                newState.Visited |= (long)1 << id;
                                var nextPlace = Math.Min(newState.JobEndsAt1, newState.JobEndsAt2);
                                statesByTime[nextPlace].Add(newState);
                            }
                        }
                    }
                    else if (state.JobEndsAt2 == time)
                    {
                        var currentNode = valves[state.LastVisited2];
                        foreach (var (id, dist) in currentNode.Distance)
                        {
                            var jobEnds = time + dist + 1;
                            if (jobEnds < N && (state.Visited & (long)1 << id) == 0)
                            {
                                var newState = new State()
                                {
                                    Visited = state.Visited,
                                    LastVisited1 = state.LastVisited1,
                                    LastVisited2 = id,
                                    JobEndsAt1 = state.JobEndsAt1,
                                    JobEndsAt2 = jobEnds,
                                    ReleasedPressure = state.ReleasedPressure + valves[id].FlowRate * (N - jobEnds),
                                };
                                newState.Visited |= (long)1 << id;
                                var nextPlace = Math.Min(newState.JobEndsAt1, newState.JobEndsAt2);
                                statesByTime[nextPlace].Add(newState);
                            }
                        }
                    }

                }
                Console.WriteLine($"Count of states ending the {time}th cycle: {statesByTime[time].Count}");

            }

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
            public long Visited { get; set; }
            public int LastVisited1 { get; set; }
            public int LastVisited2 { get; set; }
            public int JobEndsAt1 { get; set; }
            public int JobEndsAt2 { get; set; }

            public int ReleasedPressure { get; set; }
        }

    }
}
