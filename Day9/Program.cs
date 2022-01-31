using System;
using System.IO;
using System.Linq;
using Common;

namespace Day9
{
    public class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllText("input.txt").Split(',').Select(long.Parse).ToArray();
            Run(data, 1);
            Run(data, 2);
        }

        private static void Run(long[] data, int input)
        {
            var program = new IntCodeProgram();
            program.LoadInput(new long[] { input });
            program.Run(data, capacity: 100000000);
            var output = program.GetOutput();
            Console.WriteLine(output.Single());
        }
    }
}