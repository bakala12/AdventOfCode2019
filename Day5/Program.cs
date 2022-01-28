using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day5
{
    public class Program
    {
        public static void Main()
        {
            var inst = File.ReadAllText("input.txt").Split(',').Select(int.Parse).ToArray();
            Part1(inst.ToList().ToArray());
            Part2(inst.ToList().ToArray());
        }

        private static void Part1(int[] data)
        {
            var program = new IntCodeProgram();
            program.LoadInput(new int[] { 1 });
            program.Run(data);
            var res = program.GetOutput().LastOrDefault();
            Console.WriteLine(res);
        }

        private static void Part2(int[] data)
        {
            var program = new IntCodeProgram();
            program.LoadInput(new int[] { 5 });
            program.Run(data);
            var res = program.GetOutput().LastOrDefault();
            Console.WriteLine(res);
        }
    }   
}