using System;
using System.IO;
using System.Linq;
using Common;
using Common.Streams;

namespace Day19
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part1(data);
            Part2(data, 100);
        }

        private static void Part1(long[] data)
        {
            int c = 0;
            for(long y = 0; y < 50; y++)
                for(long x = 0; x < 50; x++)
                    if(BeamContainsPoint(data, x, y))
                        c++;
            Console.WriteLine(c);
        }

        private static void Part2(long[] data, int size)
        {
            int x = 0, y = 0;
            while(true)
            {
                var topRight = BeamContainsPoint(data, x+size-1, y);
                var leftBottom = BeamContainsPoint(data, x, y+size-1);
                if(topRight && leftBottom)
                    break;
                if(!topRight) y++;
                if(!leftBottom) x++;
            }
            Console.WriteLine(10000*x+y);
        }

        private static bool BeamContainsPoint(long[] data, long x, long y)
        {
            var program = new IntCodeProgram2(data, 1000000);
            return program.ExecuteTilOutput(x,y) == 1;
        }
    }
}