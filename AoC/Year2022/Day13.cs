using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC.Year2022
{
    internal class Day13 : ISolvable
    {

        public void Solve(string path)
        {
            Console.WriteLine(path);


            var lines = File.ReadAllLines(path).ToList();

            var packets = new List<Packet>();

            var resultA = 0;

            for (int l = 0; l < lines.Count; l += 3)
            {
                var a = Deserialize(lines[l]);
                var b = Deserialize(lines[l + 1]);
                var c = Comparer(a, b);
                if (c < 0)
                    resultA += l / 3 + 1;

                packets.Add(a);
                packets.Add(b);

            }

            var div1 = new Packet(new List<Packet>() { new Packet(new List<Packet>() { new Packet(2) }) });
            var div2 = new Packet(new List<Packet>() { new Packet(new List<Packet>() { new Packet(6) }) });

            packets.Add(div1);
            packets.Add(div2);

            packets.Sort(Comparer);

            var i1 = packets.FindIndex(p => Comparer(div1, p) == 0);
            var i2 = packets.FindIndex(p => Comparer(div2, p) == 0);



            Console.WriteLine();
            Console.WriteLine($"resultA: {resultA}");
            Console.WriteLine($"resultB: {(i1 + 1) * (i2 + 1)}");
            Console.WriteLine();
        }

        private int Comparer(Packet a, Packet b)
        {
            if (a.Value != null && b.Value != null)
                return (int)(a.Value - b.Value);

            if (a.Value != null)
                a = new Packet(new List<Packet>() { new Packet(a.Value) });

            if (b.Value != null)
                b = new Packet(new List<Packet>() { new Packet(b.Value) });

            int i = 0;
            while (i < a.Packets.Count && i < b.Packets.Count)
            {
                var comp = Comparer(a.Packets[i], b.Packets[i]);
                if (comp != 0)
                    return comp;

                i++;
            }

            return a.Packets.Count - b.Packets.Count;
        }


        // credits to: https://leetcode.com/problems/mini-parser/solutions/1398284/c-no-stack-easy-solution/
        private Packet Deserialize(string s)
        {
            if (!s.Contains('['))
            {
                return new Packet(Convert.ToInt32(s));
            }
            var list = new Packet(new List<Packet>());
            s = s.Substring(1, s.Length - 2);
            var i = 0;
            var j = 0;
            var bracketCounter = 0;
            while (i < s.Length)
            {
                if (bracketCounter == 0)
                {
                    j = i;
                }
                while (i < s.Length && s[i] != ',')
                {
                    if (s[i] == '[')
                    {
                        bracketCounter++;
                    }
                    else if (s[i] == ']')
                    {
                        bracketCounter--;
                    }
                    i++;
                }
                if (bracketCounter == 0)
                {
                    list.Add(Deserialize(s.Substring(j, i - j)));
                }
                i++;
            }
            return list;
        }

        string Serialize(Packet p)
        {
            if (p.Value != null)
                return p.Value.ToString();

            return "[" + string.Join(",", p.Packets.Select(Serialize)) + "]";

        }


        private class Packet
        {
            public int? Value { get; }
            public List<Packet> Packets { get; }

            public Packet(int? value)
            {
                Value = value;
                Packets = null;
            }
            public Packet(List<Packet> packets)
            {
                Value = null;
                Packets = packets;
            }



            public void Add(Packet p)
            {
                Packets.Add(p);
            }
        }
    }
}
