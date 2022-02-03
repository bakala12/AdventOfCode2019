using System;
using System.IO;
using Common;
using Common.Streams;

namespace Day13
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Part1(data);
            Part2(data);
        }

        private static void Part1(long[] data)
        {
            var inStream = new InputStream(new long[0]);
            var outStream = new OutputStream();
            var computer = new IntCodeProgram(inStream, outStream);
            computer.Run(data, capacity: 1000000);
            var output = computer.GetOutput().ToArray();
            int blocks = 0;
            for(int i = 2; i < output.Length; i+=3)
            {
                var x = output[i-2];
                var y = output[i-1];
                var t = output[i];
                if(x >= 0 && t == 2)
                    blocks++;
            }
            Console.WriteLine(blocks);
        }

        private static void Part2(long[] data)
        {
            data[0] = 2;
            var computer = new IntCodeProgram2(data, 10000000);
            int score = 0;
            int xPad = -1, xBall = -1;
            while(true)
            {
                int joy = Math.Sign(xBall - xPad);
                computer.Step(joy, out long x, out long y, out long tile);
                if(x == 99)
                    break;
                if(x == -1 && y == 0)
                { 
                    score = (int)tile;
                }
                else if(tile == 3)
                    xPad = (int)x;
                else if(tile == 4)
                    xBall = (int)x;
            }
            Console.WriteLine(score);
        }
    }

    public static class IntProgram2Extensions
    {
        public static void Step(this IntCodeProgram2 program, int input, out long x, out long y, out long tile)
        {
            var istream = new InputStream(new long[] { input });
            x = program.ExecuteTilOutput(istream);
            y = program.ExecuteTilOutput(istream);
            tile = program.ExecuteTilOutput(istream);
        }   
    }
}