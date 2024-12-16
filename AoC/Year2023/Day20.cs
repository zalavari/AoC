using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Year2023
{
    internal class Day20 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);

            Part1(lines);
        }
        private enum State
        {
            Low,
            High,
        }
        private abstract class Module
        {
            public string Name { get; set; }
            public List<string> Outputs { get; set; }
            public abstract List<ProcessingStep> Process(string source, State impulse);
        }

        private class BroadcasterModule : Module
        {
            public override List<ProcessingStep> Process(string source, State impulse)
            {
                return Outputs.Select(output => new ProcessingStep
                {
                    SourceModuleName = Name,
                    DestinationModuleName = output,
                    State = impulse
                }).ToList();
            }
        }

        private class FlipFlopModule : Module
        {
            public State State { get; set; }
            public override List<ProcessingStep> Process(string source, State impulse)
            {
                if (impulse == State.High)
                    return new List<ProcessingStep>();

                State = State == State.Low ? State.High : State.Low;

                return Outputs.Select(output => new ProcessingStep
                {
                    SourceModuleName = Name,
                    DestinationModuleName = output,
                    State = State,
                }).ToList();
            }

        }

        private class ConjunctionModule : Module
        {
            public Dictionary<string, State> Inputs { get; set; }
            public override List<ProcessingStep> Process(string source, State impulse)
            {
                Inputs[source] = impulse;

                var state = Inputs.Values.All(v => v == State.High) ? State.Low : State.High;

                return Outputs.Select(output => new ProcessingStep
                {
                    SourceModuleName = Name,
                    DestinationModuleName = output,
                    State = state,
                }).ToList();
            }
        }

        private class ProcessingStep
        {
            public string SourceModuleName { get; set; }
            public string DestinationModuleName { get; set; }
            public State State { get; set; }

        }

        private static void Part1(string[] lines)
        {
            var modules = new Dictionary<string, Module>();
            foreach (var line in lines)
            {
                Regex pattern = new Regex(@"(?<type>.{1})(?<name>[a-z0-9]+) -> (?<outputs>.*)");
                Match match = pattern.Match(line);
                var type = match.Groups["type"].Value;
                var name = match.Groups["name"].Value;
                var outputs = match.Groups["outputs"].Value.Split(", ");

                if (type == "b")
                {
                    var broadcaster = new BroadcasterModule
                    {
                        Name = "broadcaster",
                        Outputs = new List<string>(outputs)
                    };
                    modules.Add(broadcaster.Name, broadcaster);
                }
                else if (type == "&")
                {
                    var conjunction = new ConjunctionModule
                    {
                        Name = name,
                        Outputs = new List<string>(outputs),
                        Inputs = new Dictionary<string, State>()
                    };
                    modules.Add(conjunction.Name, conjunction);
                }
                else if (type == "%")
                {
                    var flipFlop = new FlipFlopModule
                    {
                        Name = name,
                        Outputs = new List<string>(outputs),
                        State = State.Low
                    };
                    modules.Add(flipFlop.Name, flipFlop);
                }
            }

            // Initialize conjuction modules inputs
            foreach (var output in modules.Values)
            {
                if (output is ConjunctionModule conjunction)
                {
                    foreach (var otherModule in modules.Values)
                    {
                        if (otherModule.Outputs.Contains(output.Name))
                        {
                            conjunction.Inputs.Add(otherModule.Name, State.Low);
                        }
                    }
                }
            }
            //PrintModules(modules);

            var processedSteps = new List<ProcessingStep>();

            for (int i = 0; i < 1000; i++)
            {
                PrintModuleStates(modules, i);
                var queue = new Queue<ProcessingStep>();
                queue.Enqueue(new ProcessingStep
                {
                    SourceModuleName = "button",
                    DestinationModuleName = "broadcaster",
                    State = State.Low
                });

                while (queue.Any())
                {
                    var step = queue.Dequeue();
                    modules.TryGetValue(step.DestinationModuleName, out var module);
                    processedSteps.Add(step);

                    //Console.WriteLine($"{step.SourceModuleName} -{step.State}-> {step.DestinationModuleName}");

                    if (module is null)
                    {
                        // Console.WriteLine($"DestinationModule is not found!");
                        continue;
                    }

                    var steps = module.Process(step.SourceModuleName, step.State);
                    foreach (var s in steps)
                    {
                        queue.Enqueue(s);
                    }

                }

            }

            var lowImpulses = processedSteps.Count(step => step.State == State.Low);
            var highImpulses = processedSteps.Count(step => step.State == State.High);

            Console.WriteLine($"Low impulses: {lowImpulses}");
            Console.WriteLine($"High impulses: {highImpulses}");
            Console.WriteLine($"Product: {lowImpulses * highImpulses}");

            // Done by hand, see file: Year2023/input/input20f.txt
            // after renaming and organizing the input, it is more clear what the modules do
            // LCM of {3797,4003,3881,3823} 
            Console.WriteLine($"Part2: {225514321828633}");
        }

        private static void PrintModuleStates(Dictionary<string, Module> modules, int i)
        {
            Console.WriteLine();
            Console.WriteLine($"Step: {i}");
            foreach (var module in modules.Values)
            {
                Console.Write($"{module.Name}");
                if (module is ConjunctionModule conjunction)
                {
                    var p = string.Join(", ", conjunction.Inputs.Select(input => input.Value == State.Low ? 0 : 1));
                    Console.Write($"& {p}");
                }
                else if (module is FlipFlopModule flipFlop)
                {
                    var p = flipFlop.State == State.Low ? 0 : 1;
                    Console.Write($"% {p}");
                }
                Console.WriteLine();
            }
        }

        private static void PrintModules(List<Module> modules)
        {

            // Print modules
            foreach (var module in modules)
            {
                Console.WriteLine($"{module.Name}");
                Console.WriteLine($"\t{string.Join(", ", module.Outputs)}");

                if (module is ConjunctionModule conjunction)
                {
                    Console.WriteLine($"\t{string.Join(", ", conjunction.Inputs.Keys)}");
                }
            }
        }
    }
}
