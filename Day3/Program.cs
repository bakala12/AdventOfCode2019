using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day3
{
    public class Program
    {
        public static void Main()
        {
            var wires = File.ReadAllLines("input.txt").Select(ParseWire).ToArray();
            Part1(wires);
            Part2(wires);
        }

        private static void Part1(List<WireSegment>[] wires)
        {
            int best = int.MaxValue;
            foreach(var s1 in wires[0])
                foreach(var s2 in wires[1])
                {
                    if(s1.IntersectWith(s2, out Point ip) && ip.ManhattanDistance > 0 && ip.ManhattanDistance < best)
                        best = ip.ManhattanDistance;
                }
            Console.WriteLine(best);
        }

        private static void Part2(List<WireSegment>[] wires)
        {
            int best = int.MaxValue;
            int sum1 = 0, sum2 = 0;
            foreach(var s1 in wires[0])
            {
                sum2 = 0;
                foreach(var s2 in wires[1])
                {
                    if(s1.IntersectWith(s2, out Point ip) && ip.ManhattanDistance > 0)
                    {
                        int l1 = (ip - s1.Start).ManhattanDistance + sum1;
                        int l2 = (ip - s2.Start).ManhattanDistance + sum2;
                        if(l1 + l2 < best)
                            best = l1+l2;
                    }
                    sum2 += s2.Length;
                }
                sum1 += s1.Length;
            }
            Console.WriteLine(best);
        }

        private static List<WireSegment> ParseWire(string line)
        {
            List<WireSegment> segments = new List<WireSegment>();
            int lx = 0, ly = 0;
            foreach(var inst in line.Split(','))
            {
                int length = int.Parse(inst.Substring(1));
                switch(inst[0])
                {
                    case 'U':
                        segments.Add(new VerticalWireSegment(ly, ly+length, lx));
                        ly += length;
                        break;
                    case 'D':
                        segments.Add(new VerticalWireSegment(ly, ly-length, lx));
                        ly -= length;
                        break;
                    case 'L':
                        segments.Add(new HorizontalWireSegment(lx, lx - length, ly));
                        lx -= length;
                        break;
                    case 'R':
                        segments.Add(new HorizontalWireSegment(lx, lx + length, ly));
                        lx += length;
                        break;
                }
            }
            return segments;
        }
    }
}